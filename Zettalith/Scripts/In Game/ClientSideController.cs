﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class ClientSideController
    {
        public const float
            HEIGHTDISTANCE = 0.6875f,
            SPLASHSIZE = 6.0f,
            DRAGDISTANCE = 4.0f,
            LIFTDISTANCE = 0.4f;

        static readonly Color
            defaultHighlightColor = new Color(0, 255, 215, 255),
            movementHighlightColor = new Color(255, 200, 0, 155),
            movementSelectedHighlightColor = new Color(255, 200, 0, 255),
            pieceHighlightColor = new Color(172, 255, 242, 255),
            pieceCoveredColor = new Color(172, 255, 242, 80),
            pieceGhostColor = new Color(255, 255, 255, 120),
            dimColor = new Color(0, 0, 0, 160);

        int handPieceHeight, handPieceWidth;

        public static Vector2 MousePositionAbsolute => RendererController.Camera.ScreenToWorldPosition(In.MousePosition.ToVector2()); 
        public static Vector2 MousePosition => new Vector2(MousePositionAbsolute.X, MousePositionAbsolute.Y / HEIGHTDISTANCE);
        public static Point MousePoint => MousePosition.RoundToPoint();

        public bool MoveableCamera { get; set; } = true;
        public bool SetupComplete { get; private set; }

        public Renderer.Sprite[,] tiles, highlights;

        GUI.Collection collection, battleGUI, logisticsGUI, setupGUI, endGUI;

        Renderer.Text splash, essence;
        Renderer.Text[] mana;
        Renderer.Sprite ghost;
        Renderer.SpriteScreen dim, bottomPanel, topPanel, essencePanel;
        List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)> handPieces;
        GUI.Button bEndTurn;

        Point[] movementHighlight;
        List<Point> highlightSquares;
        Vector2 previousGamePosition;
        Point previousScreenPosition = new Point();
        TilePiece interactionPiece, highlightedPiece;
        TimerTable splashTable;
        PlayerLocal player;
        InGameController controller;
        InGamePiece dragOutPiece;
        CameraMovement cameraMovement;
        Point handStart, handEnd, mouseDownPosition;

        List<(TilePiece piece, TimerTable table, Renderer.Animator[] animators)> animatingPieces;

        public ClientSideController(PlayerLocal player, InGameController controller, bool host, bool start)
        {
            this.player = player;
            this.controller = controller;

            cameraMovement = new CameraMovement();

            handPieces = new List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)>();
            handPieceHeight = Settings.GetResolution.Y / 5;

            dim = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 50), Load.Get<Texture2D>("Square"), new Rectangle(Point.Zero, Settings.GetResolution), dimColor);

            string splashText = start ? "You Start" : "Opponent Starts";
            splash = new Renderer.Text(new Layer(MainLayer.GUI, 50), Font.Bold, splashText, SPLASHSIZE, 0, Settings.GetHalfResolution.ToVector2(), 0.5f * Font.Bold.MeasureString(splashText), defaultHighlightColor);

            mana = new Renderer.Text[3];
            mana[0] = new Renderer.Text(Layer.GUI, Font.Bold, "Blue: " + controller.LocalMana.Blue, 3.5f, 0, new Vector2(10, 70));
            mana[1] = new Renderer.Text(Layer.GUI, Font.Bold, "Green: " + controller.LocalMana.Green, 3.5f, 0, new Vector2(10, 100));
            mana[2] = new Renderer.Text(Layer.GUI, Font.Bold, "Red: " + controller.LocalMana.Red, 3.5f, 0, new Vector2(10, 130));
            UpdateManaGUI();

            CreateBattleGUI();
            CreateLogisticsGUI();
            CreateSetupGUI();

            collection = new GUI.Collection();
            collection.Add(dim, splash, essencePanel, essence);

            RendererController.GUI.Add(collection);
            collection.Add(battleGUI, logisticsGUI, setupGUI, mana[0], mana[1], mana[2]);

            splashTable = new TimerTable(new float[] { 1, 2 });
            animatingPieces = new List<(TilePiece piece, TimerTable table, Renderer.Animator[] animators)>();

            CreateMap(InGameController.Grid);
        }

        public void CreateMap(Grid grid)
        {
            Texture2D tileTexture = Load.Get<Texture2D>("Tile"), highlightTexture = Load.Get<Texture2D>("TileHighlight");

            tiles = new Renderer.Sprite[grid.xLength, grid.yLength];
            highlights = new Renderer.Sprite[grid.xLength, grid.yLength];

            for (int x = 0; x < grid.xLength; ++x)
            {
                for (int y = 0; y < grid.yLength; ++y)
                {
                    tiles[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, y - grid.yLength - 1), tileTexture, new Vector2(x, y * HEIGHTDISTANCE), Vector2.One, Color.White, 0, new Vector2(16, 11), SpriteEffects.None);
                    highlights[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, y - grid.yLength), highlightTexture, new Vector2(x, y * HEIGHTDISTANCE).ToRender(), Vector2.One, Color.White, 0, new Vector2(16, 11), SpriteEffects.None);

                    if (!InGameController.IsHost)
                    {
                        tiles[x, y].Position *= -1;
                    }
                }
            }
        }

        public void Update(float deltaTime, InGameState gameState)
        {
            Pieces(deltaTime, gameState == InGameState.Battle);

            if (gameState == InGameState.Battle)
            {
                UpdateHandPieces();
            }

            SplashUpdate(deltaTime, gameState);

            UpdateManaGUI();

            if (MoveableCamera)
            {
                cameraMovement.Update(RendererController.Camera, In.MousePosition, highlightedPiece == null && RendererFocus.OnArea(new Rectangle(MousePoint, Point.Zero), Layer.Default), deltaTime);
            }

            previousScreenPosition = In.MousePosition;
            UpdateHighlights();
        }

        void EndSetup()
        {
            splash.Scale = Vector2.Zero;
            dim.Color = Color.Transparent;

            if (InGameController.PlayerIndex == InGameController.StartPlayer)
            {
                OpenBattle();
            }
            else
            {
                OpenLogistics();
            }

            SetupComplete = true;

            InGameController.Main.SetupEnd();
        }

        void CreateBattleGUI()
        {
            battleGUI = new GUI.Collection();

            int width = (int)(Settings.GetResolution.X * (401f / 480f)), height = (int)(Settings.GetResolution.Y * (24f / 270f)),
                buttonWidth = (int)(Settings.GetResolution.X * (51f / 480f)), buttonHeight = (int)(Settings.GetResolution.Y * (25f / 270f));

            bottomPanel = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("BottomPanel"), new Rectangle(Settings.GetHalfResolution.X - width / 2, Settings.GetResolution.Y - height, width, height));

            bEndTurn = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle(Settings.GetHalfResolution.X - buttonWidth / 2, (int)(Settings.GetResolution.Y * 0.885f), buttonWidth, buttonHeight), Load.Get<Texture2D>("EndTurnButton"), Load.Get<Texture2D>("EndTurnButtonHover"), Load.Get<Texture2D>("EndTurnButtonClick")) { ScaleEffect = true };
            bEndTurn.OnClick += player.EndTurn;

            handStart = new Point((int)(Settings.GetResolution.X * 0.15f), Settings.GetResolution.Y - height * 2);
            handEnd = new Point((int)(Settings.GetResolution.X * 0.30f), Settings.GetResolution.Y - height * 2);

            GUI.Button button = new GUI.Button(Layer.GUI, new Rectangle(10, 10, 160, 60));
            button.AddText("Draw Piece", 3, true, Color.Black, Font.Bold);
            button.OnClick += DrawPieceFromDeck;

            battleGUI.Add(bottomPanel, bEndTurn, button);
            battleGUI.Active = false;
        }

        void CreateLogisticsGUI()
        {
            logisticsGUI = new GUI.Collection();
            battleGUI.Active = false;

            // TODO Add logistics UI
        }

        void CreateSetupGUI()
        {
            setupGUI = new GUI.Collection();
        }

        void UpdateManaGUI()
        {
            mana[0].String = new StringBuilder("Blue: " + controller.LocalMana.Blue);
            mana[1].String = new StringBuilder("Green: " + controller.LocalMana.Green);
            mana[2].String = new StringBuilder("Red: " + controller.LocalMana.Red);
        }

        public void CloseSetup()
        {

        }

        public void OpenBattle()
        {
            battleGUI.Active = true;
            logisticsGUI.Active = false;

            splash.String = new StringBuilder("Your Turn");
            splash.Origin = splash.Font.MeasureString("Your Turn") * 0.5f;
            splashTable = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void OpenLogistics()
        {
            battleGUI.Active = false;
            logisticsGUI.Active = true;

            splash.String = new StringBuilder("Opponent's Turn");
            splash.Origin = splash.Font.MeasureString("Opponent's Turn") * 0.5f;
            splashTable = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void OpenEnd(bool winner)
        {
            battleGUI.Active = false;
            logisticsGUI.Active = false;

            string endString = "Game End: " + (winner ? "You Won!" : "You Lose!");

            splash.String = new StringBuilder(endString);
            splash.Origin = splash.Font.MeasureString(endString) * 0.5f;
            splash.Position = new Vector2(Settings.GetHalfResolution.X, Settings.GetResolution.Y / 4);
            splashTable = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });

            float screenWidth = 0.35f, screenHeight = 0.4f;

            Vector2 res = Settings.GetResolution.ToVector2();

            endGUI = new GUI.Collection()
            {
                Origin = (res * 0.5f).RoundToPoint()
            };

            GUI.Button bReturn = new GUI.Button(Layer.GUI, new Rectangle((int)(-res.X * 0.15f), (int)(-res.Y * 0.05f), (int)(res.X * 0.3f), (int)(res.Y * 0.1f)));
        }

        public void DrawPiece(InGamePiece piece)
        {
            //handPieces.Add((new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 1), piece.Texture, ec), piece));
        }

        public void PlacePieceAnimation(TilePiece piece)
        {
            TimerTable newTable = new TimerTable(new float[] { 0.2f, 0.3f, 0.1f });

            animatingPieces.Add((piece, newTable, null));
        }

        void Pieces(float deltaTime, bool moveable)
        {
            AnimatePieces(deltaTime);

            bool leftMouse = In.LeftMouse, leftMouseDown = In.LeftMouseDown;
            List<TilePiece> highlightedPieces = new List<TilePiece>();
            highlightedPiece = null;

            for (int i = 0; i < InGameController.Grid.Objects.Length; i++)
            {
                TileObject piece = InGameController.Grid.Objects[i];

                if (piece is TilePiece)
                {
                    TilePiece tPiece = piece as TilePiece;

                    if (tPiece.Renderer == null)
                    {
                        continue;
                    }

                    if (RendererFocus.OnArea(tPiece.Renderer.GetArea(), tPiece.Renderer.Layer))
                    {
                        highlightedPieces.Add(tPiece);
                    }
                    else
                    {
                        tPiece.Renderer.Color = Color.White;
                    }
                }
            }

            highlightedPieces = highlightedPieces.OrderBy(o => o.Renderer.Layer.LayerDepth).ToList();

            // No further activation or interaction is allowed when an ability is active
            if (player.ActionPiece != null)
            {
                return;
            }

            for (int i = 0; i < highlightedPieces.Count; i++)
            {
                if (i == 0)
                {
                    highlightedPieces[i].Renderer.Color = pieceHighlightColor;
                    highlightedPiece = highlightedPieces[i];

                    continue;
                }

                highlightedPieces[i].Renderer.Color = pieceCoveredColor;
            }

            if (highlightedPiece != null && interactionPiece == null)
            {
                AddHighlight(defaultHighlightColor, highlightedPiece.Position);
            }

            if (moveable)
            {
                BoardMove(leftMouseDown, leftMouse, highlightedPiece);
            }
        }

        void BoardMove(bool leftMouseDown, bool leftMouse, TilePiece highlightedPiece)
        {
            if (!leftMouse && interactionPiece != null)
            {
                float distance = (In.MousePosition.ToVector2() - mouseDownPosition.ToVector2()).Length();

                if (movementHighlight != null)
                {
                    if (movementHighlight.Contains(MousePoint.ToRender()))
                    {
                        if (InGameController.Main.LocalMana >= interactionPiece.Piece.Bottom.MoveCost)
                        {
                            player.ExecuteMovement(interactionPiece, MousePoint.ToRender());
                        }
                        else
                        {

                        }
                    }
                }

                if (distance < DRAGDISTANCE && interactionPiece.Player == InGameController.PlayerIndex)
                {
                    player.RequestAction(interactionPiece);
                }

                ghost?.Destroy();
                ghost = null;
                movementHighlight = null;
                interactionPiece = null;
            }

            if (highlightedPiece != null)
            {
                if (leftMouseDown)
                {
                    interactionPiece = highlightedPiece;
                    mouseDownPosition = In.MousePosition;
                }
            }

            if (leftMouse && interactionPiece != null)
            {
                float distance = (In.MousePosition.ToVector2() - mouseDownPosition.ToVector2()).Length();

                AddHighlight(Color.White, interactionPiece.Position.ToRender());

                if (movementHighlight != null && movementHighlight.Length > 0)
                {
                    foreach (Point point in movementHighlight)
                    {
                        AddHighlight(point == MousePoint.ToRender() ? movementSelectedHighlightColor : movementHighlightColor, point);
                    }
                }

                if (distance > DRAGDISTANCE && interactionPiece.Player == InGameController.PlayerIndex)
                {
                    if (ghost == null)
                    {
                        ghost = new Renderer.Sprite(Layer.Default, interactionPiece.Renderer.Texture, MousePosition, Vector2.One, pieceGhostColor, 0, interactionPiece.Renderer.Origin, SpriteEffects.None);
                    }

                    if (movementHighlight == null)
                    {
                        movementHighlight = player.RequestMovement(interactionPiece);
                    }

                    ghost.Position = MousePositionAbsolute;
                    ghost.Layer = new Layer(MainLayer.Main, TileObject.DefaultLayer((int)(MousePosition.Y)).layer + 1);
                }
            }
        }

        void AnimatePieces(float deltaTime)
        {
            for (int i = animatingPieces.Count - 1; i >= 0; --i)
            {
                (TilePiece piece, TimerTable table, Renderer.Animator[] animators) item = animatingPieces[i];

                if (item.animators != null)
                {
                    if (item.animators[0].Complete)
                    {
                        animatingPieces[i].animators[0].Destroy();
                        animatingPieces[i].animators[1].Destroy();

                        animatingPieces.RemoveAt(i);
                        return;
                    }

                    continue;
                }

                int state = item.table.Update(deltaTime);
                float currentProgress = item.table.CurrentStepProgress;

                if (item.table.Complete)
                {
                    animatingPieces[i].piece.Renderer.Position = item.piece.SupposedPosition;

                    animatingPieces[i] = (item.piece, item.table, new Renderer.Animator[]
                    {
                        new Renderer.Animator(new Layer(MainLayer.Main, item.piece.Renderer.Layer.layer - 1), Load.Get<Texture2D>("DustAnimationBack"), new Point(96, 66), item.piece.Renderer.Position, Vector2.One, new Vector2(48f, 33f), 0, Color.White, 0.08f, 0, false, SpriteEffects.None),
                        new Renderer.Animator(new Layer(MainLayer.Main, item.piece.Renderer.Layer.layer + 1), Load.Get<Texture2D>("DustAnimationFront"), new Point(96, 66), item.piece.Renderer.Position, Vector2.One, new Vector2(48f, 33f), 0, Color.White, 0.08f, 0, false, SpriteEffects.None)
                    });

                    continue;
                }

                if (state == 0)
                {
                    item.piece.Renderer.Position = new Vector2(item.piece.SupposedPosition.X, item.piece.SupposedPosition.Y - LIFTDISTANCE * Easing.EaseOutCubic(currentProgress));
                }

                if (state == 1)
                {
                    item.piece.Renderer.Position = new Vector2(item.piece.SupposedPosition.X, item.piece.SupposedPosition.Y - LIFTDISTANCE);
                }

                if (state == 2)
                {
                    item.piece.Renderer.Position = new Vector2(item.piece.SupposedPosition.X, item.piece.SupposedPosition.Y - LIFTDISTANCE * (1 - currentProgress));
                }
            }
        }

        void WarnMana()
        {

        }

        public void DrawPieceFromDeck()
        {
            if (player.Deck.Pieces.Count <= 0)
            {
                return;
            }

            InGamePiece newPiece = player.Deck.Draw();
            Renderer.SpriteScreen sc = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 1), newPiece.Texture, new Rectangle());

            handPieces.Add((sc, newPiece, new RendererFocus(new Layer(MainLayer.GUI, 1), new Rectangle(), false)));

            UpdateHandPieces();

            battleGUI.Add(sc);
        }

        void UpdateHandPieces()
        {
            InGamePiece removePiece = null;

            if (dragOutPiece != null)
            {
                if (In.LeftMouse)
                {
                    AddHighlight(movementSelectedHighlightColor, MousePoint.ToRender());
                }
                else
                {
                    if (InGameController.Grid.Vacant(MousePoint.ToRender().X, MousePoint.ToRender().Y))
                    {
                        if (controller.LocalMana > dragOutPiece.GetCost)
                        {
                            removePiece = dragOutPiece;
                            player.PlacePiece(dragOutPiece, MousePoint.ToRender().X, MousePoint.ToRender().Y);

                            controller.LocalMana -= dragOutPiece.GetCost;
                        }
                            
                        dragOutPiece = null;
                    }
                }
            }

            for (int i = handPieces.Count - 1; i >= 0; --i)
            {
                (Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus) item = handPieces[i];

                if (item.piece == removePiece)
                {
                    handPieces.RemoveAt(i);
                    item.renderer.Destroy();
                    item.focus.Remove();
                    battleGUI.Remove(item.renderer);
                    continue;
                }

                Rectangle rectangle = GetTargetRectangle(i, handPieces.Count);

                bool color = false;

                if (dragOutPiece == null && rectangle.Contains(In.MousePosition) && !(i < handPieces.Count - 1 && GetTargetRectangle(i + 1, handPieces.Count).Contains(In.MousePosition)))
                {
                    color = true;
                    if (In.LeftMouseDown)
                    {
                        dragOutPiece = item.piece;
                    }
                }

                item.renderer.Color = color ? pieceHighlightColor : Color.White;
                item.renderer.Layer = new Layer(MainLayer.GUI, 1 + i);
                item.renderer.Transform = rectangle;
                item.focus.Rectangle = rectangle;
            }
        }

        void SplashUpdate(float deltaTime, InGameState gameState)
        {
            if (!SetupComplete)
            {
                int state = splashTable.Update(deltaTime);
                float currentProgress = splashTable.CurrentStepProgress;

                if (splashTable.Complete)
                {
                    EndSetup();
                }
                else
                {
                    if (state == 1)
                    {
                        splash.Scale = Vector2.One * (1 - Easing.EaseInBack(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                        dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (1 - currentProgress)));
                    }
                }

                return;
            }

            if (!splashTable.Complete)
            {
                int state = splashTable.Update(deltaTime);
                float currentProgress = splashTable.CurrentStepProgress;

                bool endScreen = gameState == InGameState.End;

                if (state == 0)
                {
                    splash.Scale = Vector2.One * (Easing.EaseOutCubic(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (currentProgress)));
                }

                if (state == 1)
                {
                    splash.Scale = Vector2.One * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = dimColor;

                    if (endScreen)
                    {
                        splashTable.End();
                    }
                }

                if (state == 2)
                {
                    splash.Scale = Vector2.One * (1 - Easing.EaseInBack(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (1 - currentProgress)));
                }
            }
            else if (dim.Color.A != 0)
            {
                splash.Scale = Vector2.Zero;
                dim.Color = Color.Transparent;
            }
        }

        private Rectangle GetTargetRectangle(int index, int count) 
            => new Rectangle(handStart.ToVector2().Lerp(handEnd.ToVector2(), count == 1 ? 0 : index / (count - 1.0f)).RoundToPoint(), 
                new Point((int)(handPieceHeight * ((float)handPieces[index].piece.Texture.Width / handPieces[index].piece.Texture.Height)), handPieceHeight));

        #region Tile Highlighting

        static List<(Point, Color)> queuedHighlights = new List<(Point, Color)>();

        public static void AddHighlight(params Point[] points)
        {
            AddHighlight(defaultHighlightColor, points);
        }

        public static void AddHighlight(Color color, params Point[] points)
        {
            foreach (Point point in points)
            {
                queuedHighlights.Add((point, color));
            }
        }

        void UpdateHighlights()
        {
            int xL = highlights.GetLength(0), yL = highlights.GetLength(1);

            Color[,] highlightColors = new Color[xL, yL];
            Color d = default(Color);

            foreach ((Point p, Color c) item in queuedHighlights)
            {
                if (InGameController.Grid.InBounds(item.p.X, item.p.Y))
                {
                    highlightColors[item.p.X, item.p.Y] = item.c;
                }
            }

            for (int x = 0; x < xL; x++)
            {
                for (int y = 0; y < yL; y++)
                {
                    if (highlightColors[x, y] == d)
                    {
                        highlights[x, y].Color = Color.Transparent;

                        continue;
                    }

                    highlights[x, y].Color = highlightColors[x, y];
                }
            }

            queuedHighlights.Clear();
        }

        #endregion

        #region Texture Loading

        static Dictionary<(byte a, byte b, byte c), Texture2D> LoadedTextures { get; set; }

        public static Texture2D GetTexture(byte top, byte middle, byte bottom)
        {
            if (LoadedTextures == null)
            {
                LoadedTextures = new Dictionary<(byte a, byte b, byte c), Texture2D>();
            }

            if (LoadedTextures.ContainsKey((top, middle, bottom)))
            {
                return LoadedTextures[(top, middle, bottom)];
            }

            Texture2D newTexture = ImageProcessing.CombinePiece(Subpieces.FromIndex(top).Texture, Subpieces.FromIndex(middle).Texture, Subpieces.FromIndex(bottom).Texture);
            LoadedTextures.Add((top, middle, bottom), newTexture);
            return newTexture;
        }

        #endregion
    }
}
