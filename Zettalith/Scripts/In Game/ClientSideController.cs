using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Zettalith
{
    class ClientSideController
    {
        public static int
            placeHeight = 2 + Ztuff.placeHeightIncrease;

        const int 
            DIAMETER = 7;

        public const float
            HEIGHTDISTANCE = 0.6875f,
            SPLASHSIZE = 6.0f,
            DRAGDISTANCE = 4.0f,
            LIFTDISTANCE = 0.4f;

        static public readonly Color
            defaultHighlightColor = new Color(0, 255, 215, 255),
            defaultEnemyHighlightColor = new Color(255, 40, 0, 255),
            movementHighlightColor = new Color(145, 200, 80, 155),
            movementSelectedHighlightColor = new Color(160, 255, 45, 255),
            meleeHighlightColor = new Color(230, 110, 20, 200),
            meleeSelectedColor = new Color(255, 140, 35, 255),
            pieceEnemyHighlightColor = new Color(255, 172, 180, 255),
            pieceHighlightColor = new Color(172, 255, 242, 255),
            pieceCoveredColor = new Color(172, 255, 242, 80),
            pieceGhostColor = new Color(255, 255, 255, 120),
            dimColor = new Color(0, 0, 0, 160);

        public static Vector2 TopLeftCorner { get; private set; }
        public static Vector2 BottomRightCorner { get; private set; }

        public static ParticleMap Particles { get; private set; }

        public static Vector2 MousePositionAbsolute => RendererController.Camera.ScreenToWorldPosition(Input.MousePosition.ToVector2()); 
        public static Vector2 MousePosition => new Vector2(MousePositionAbsolute.X, MousePositionAbsolute.Y / HEIGHTDISTANCE);
        public static Point MousePoint => MousePosition.RoundToPoint();

        public bool MoveableCamera { get; set; } = true;
        public bool SetupComplete { get; private set; }

        public Renderer.Sprite[,] tiles, tileFronts, backgrounds;
        public Renderer.Animator[,] highlights;
        public CloudManager cloudManager;
        public EffectCache MyEffectCache;

        public Vector2[,] backgroundPositions;

        InGameHUD hud;
        GUI.Collection collection, battleGUI, logisticsGUI, endGUI, perkGUI, buffGUI, bonusGUI, managementGUI;

        Renderer.Text splash;
        Renderer.Sprite ghost;
        Renderer.SpriteScreen dim, bottomPanel, topPanel, essencePanel;

        SoundEffect song;

        Point[] movementHighlight, meleeHighlight;
        List<Point> highlightSquares;
        Vector2 previousGamePosition;
        Point previousScreenPosition = new Point();
        TilePiece interactionPiece, highlightedPiece;
        TimerTable splashTable;
        PlayerLocal player;
        InGameController controller;
        InGamePiece dragOutPiece;
        CameraMovement cameraMovement;
        InfoBox infoBox;
        PieceStats pieceStats;
        Point mouseDownPosition, rightMouseDownPosition;

        List<(TilePiece piece, TimerTable table, Renderer.Animator[] animators)> animatingPieces;

        public ClientSideController(PlayerLocal player, InGameController controller, bool host, bool start)
        {
            this.player = player;
            this.controller = controller;

            dim = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 50), Load.Get<Texture2D>("Square"), new Rectangle(Point.Zero, Settings.GetResolution), dimColor);

            string splashText = start ? "You Start" : "Opponent Starts";
            splash = new Renderer.Text(new Layer(MainLayer.GUI, 50), Font.Bold, splashText, SPLASHSIZE, 0, Settings.GetHalfResolution.ToVector2(), 0.5f * Font.Bold.MeasureString(splashText), defaultHighlightColor);

            collection = new GUI.Collection();

            Vector2 res = Settings.GetResolution.ToVector2();

            battleGUI = new GUI.Collection();
            logisticsGUI = new GUI.Collection();
            perkGUI = new GUI.Collection();
            buffGUI = new GUI.Collection();
            bonusGUI = new GUI.Collection();
            managementGUI = new GUI.Collection();
            endGUI = new GUI.Collection() { Origin = (res * 0.5f).ToPoint() };

            RendererController.GUI.Add(collection);
            collection.Add(perkGUI, buffGUI, bonusGUI, battleGUI, logisticsGUI, endGUI, managementGUI);

            hud = new InGameHUD(collection, perkGUI, buffGUI, bonusGUI, battleGUI, logisticsGUI, endGUI, managementGUI, controller, this, player);

            infoBox = new InfoBox(collection);

            Particles = new ParticleMap(collection, InGameController.Grid.xLength * 32, InGameController.Grid.yLength * 22, MainLayer.Main, TileObject.DefaultLayer(host ? 0 : InGameController.Grid.yLength - 1).layer + 1, TileObject.DefaultLayer(host ? InGameController.Grid.yLength - 1 : 0).layer + 2, true);

            GUI.Collection pieceStatsCollection = new GUI.Collection();
            pieceStats = new PieceStats(pieceStatsCollection);
            collection.Add(pieceStatsCollection);

            splashTable = new TimerTable(new float[] { 1, 2 });
            animatingPieces = new List<(TilePiece piece, TimerTable table, Renderer.Animator[] animators)>();

            CreateMap(InGameController.Grid);

            Vector2 distance = new Vector2(60, 20 * HEIGHTDISTANCE);

            Vector2
                topLeft = InGameController.IsHost ? Vector2.Zero : new Vector2(-InGameController.Grid.xLength, -InGameController.Grid.yLength * HEIGHTDISTANCE),
                bottomRight = InGameController.IsHost ? new Vector2(InGameController.Grid.xLength, InGameController.Grid.yLength * HEIGHTDISTANCE): Vector2.Zero;

            TopLeftCorner = topLeft;
            BottomRightCorner = bottomRight;

            cloudManager = new CloudManager(50, 3, topLeft - distance, bottomRight + distance + new Vector2(0, 10), 1.5f, 2, new Vector2(0.02f, 0.04f), 2, topLeft.X + (bottomRight.X - topLeft.X) * 0.5f);
            cloudManager.FastForward(1000, 0.05f);

            cameraMovement = new CameraMovement();

            MyEffectCache = new EffectCache();

            song = Load.Get<SoundEffect>("Altitude");
            if (!XNAController.LocalGameClient)
            {
                Sound.PlaySong(song);
            }
        }

        public void CreateMap(Grid grid)
        {
            Random r = new Random();

            Texture2D tileTexture = Load.Get<Texture2D>("Tile"), highlightTexture = Load.Get<Texture2D>("TileHighlightSheet"), frontTexture = Load.Get<Texture2D>("TileFront"), backgroundTexture = Load.Get<Texture2D>("Carpet");
            Texture2D[] tileTextures =
            {
                Load.Get<Texture2D>("Tile1"),
                Load.Get<Texture2D>("Tile2"),
                Load.Get<Texture2D>("Tile3"),
                Load.Get<Texture2D>("Tile4")
            };

            tiles = new Renderer.Sprite[grid.xLength, grid.yLength];
            tileFronts = new Renderer.Sprite[grid.xLength, grid.yLength];
            highlights = new Renderer.Animator[grid.xLength, grid.yLength];
            backgrounds = new Renderer.Sprite[DIAMETER, DIAMETER];
            backgroundPositions = new Vector2[DIAMETER, DIAMETER];

            Vector2 centre = TopLeftCorner + (BottomRightCorner - TopLeftCorner) * 0.5f, dimension = 2 * new Vector2(backgroundTexture.Width, backgroundTexture.Height) / Camera.WORLDUNITPIXELS, origin = centre - dimension * 0.5f * DIAMETER;

            for (int x = 0; x < DIAMETER; ++x)
            {
                for (int y = 0; y < DIAMETER; ++y)
                {
                    backgroundPositions[x, y] = origin + dimension * new Vector2(x, y);
                    backgrounds[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, -10000), backgroundTexture, backgroundPositions[x, y], Vector2.One, Color.White, 0, new Vector2(backgroundTexture.Width, backgroundTexture.Height) * 0.5f, SpriteEffects.None);
                }
            }

            for (int x = 0; x < grid.xLength; ++x)
            {
                for (int y = 0; y < grid.yLength; ++y)
                {
                    bool host = InGameController.IsHost;

                    tiles[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, (host ? (y - grid.yLength) : (-y)) - 1), tileTextures[r.Next(4)], new Vector2(x, y * HEIGHTDISTANCE), Vector2.One, Color.White, 0, new Vector2(16, 11), SpriteEffects.None);
                    tileFronts[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, (host ? (y - grid.yLength) : (-y)) - 1), frontTexture, new Vector2(x, (y + (host ? 0.5f : -0.5f)) * HEIGHTDISTANCE), Vector2.One, Color.White, 0, new Vector2(16, 0), SpriteEffects.None);
                    highlights[x, y] = new Renderer.Animator(new Layer(MainLayer.Background, (host ? (y - grid.yLength) : (-y))), highlightTexture, new Point(32, 22), new Vector2(x, y * HEIGHTDISTANCE).ToRender(), Vector2.One, new Vector2(16, 11), 0, Color.White, 0.05f, 0, true, SpriteEffects.None);

                    tiles[x, y].Active = grid[x, y] != null;
                    tileFronts[x, y].Active = grid[x, y] != null;

                    if (!InGameController.IsHost)
                    {
                        tiles[x, y].Position *= -1;
                        tileFronts[x, y].Position *= -1;
                    }
                }
            }
        }

        public void Update(float deltaTime, InGameState gameState)
        {
            if (Input.LeftMouse)
            {
                Particles.Beam(new Vector2(0, 0), RendererController.Camera.ScreenToWorldPosition(Input.MousePosition.ToVector2()), Color.Yellow, new Color(Color.Red, 0f), 30);
            }

            AnimatePieces(deltaTime);
            Pieces(deltaTime, gameState == InGameState.Battle);

            infoBox.Update(deltaTime);

            if (gameState == InGameState.Battle)
            {
                UpdateHandPieces();
            }

            cloudManager.Update(deltaTime);

            hud.Update(deltaTime);

            SplashUpdate(deltaTime, gameState);

            if (MoveableCamera)
            {
                cameraMovement.Update(RendererController.Camera, Input.MousePosition, highlightedPiece == null && RendererFocus.OnArea(new Rectangle(MousePoint, Point.Zero), Layer.Default), deltaTime);
            }

            previousScreenPosition = Input.MousePosition;
            UpdateHighlights();
        }

        public void UpdateBackground()
        {
            if (backgrounds == null)
                return;

            for (int x = 0; x < DIAMETER; x++)
            {
                for (int y = 0; y < DIAMETER; y++)
                {
                    backgrounds[x, y].Position = backgroundPositions[x, y] + RendererController.Camera.Position * 0.9f * new Vector2(1, 0);
                }
            }
        }

        public void UpdateStats()
        {
            if (pieceStats == null)
                return;

            pieceStats.Update();
        }

        public void UpdateParticles(float deltaTime)
        {
            if (Particles == null)
                return;

            Vector2 offset = new Vector2(0.5f, 0.5f * HEIGHTDISTANCE);
            if (InGameController.IsHost)
            {
                offset *= -1;
            }

            Particles.Update(TopLeftCorner + offset, BottomRightCorner + offset, true, deltaTime);
        }

        public void ComputeSendLogistics()
        {
            controller.Execute(GameAction.EndTurn, true, MyEffectCache);
            MyEffectCache = new EffectCache();
        }

        public void ComputeRecieveLogistics(object arg)
        {
            EffectCache anEffectCache = arg as EffectCache;

            for (int i = 0; i < anEffectCache.AListOfSints.Count; ++i)
            {
                if (InGameController.Grid[anEffectCache.AListOfSints[i].IntB] != null || anEffectCache.AListOfSints[i].IntB < 0)
                {
                    PerkBuffBonusEffects.EffectArray[anEffectCache.AListOfSints[i].IntA](anEffectCache.AListOfSints[i].IntB);
                }
            }

            controller.TurnSwitch();
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

            
        }

        void CreateLogisticsGUI()
        {
            logisticsGUI = new GUI.Collection();
            battleGUI.Active = false;

            // TODO Add logistics UI
        }

        public void CloseSetup()
        {

        }

        public void CloseBattle()
        {
            battleGUI.Active = false;
        }

        public void CloseLogistics()
        {
            logisticsGUI.Active = false;
            perkGUI.Active = false;
            buffGUI.Active = false;
            bonusGUI.Active = false;
            if (managementGUI.Active == true)
            {
                int manaToIncrease = new Random().Next(0, 3);
                InGameController.Local.BaseMana = new Mana(InGameController.Local.BaseMana.Red + (manaToIncrease == 0 ? 1 : 0), InGameController.Local.BaseMana.Green + (manaToIncrease == 1 ? 1 : 0), InGameController.Local.BaseMana.Blue + (manaToIncrease == 2 ? 1 : 0));
                managementGUI.Active = false;
            }
            Ztuff.pickingPiece = false;
            Ztuff.RestoreFromBuff();
        }

        public void OpenBattle()
        {
            battleGUI.Active = true;
            logisticsGUI.Active = false;

            DrawPieceFromDeck();

            splash.String = new StringBuilder("Your Turn");
            splash.Origin = splash.Font.MeasureString("Your Turn") * 0.5f;
            splashTable = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void OpenLogistics()
        {
            battleGUI.Active = false;
            logisticsGUI.Active = false;
            managementGUI.Active = true;

            splash.String = new StringBuilder("Opponent's Turn");
            splash.Origin = splash.Font.MeasureString("Opponent's Turn") * 0.5f;
            splashTable = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void ForceOpenLogistics()
        {
            logisticsGUI.Active = true;
        }

        public void OpenPerks()
        {
            logisticsGUI.Active = false;
            perkGUI.Active = true;
        }

        public void OpenBuff()
        {
            logisticsGUI.Active = false;
            buffGUI.Active = true;
        }

        public void OpenBonus()
        {
            logisticsGUI.Active = false;
            bonusGUI.Active = true;
        }

        public void ClosePerks()
        {
            perkGUI.Active = false;
            logisticsGUI.Active = true;
        }

        public void CloseBuffsAndBonus()
        {
            buffGUI.Active = false;
            bonusGUI.Active = false;
            logisticsGUI.Active = true;
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

            hud.EndHUD.Open();
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
            bool leftMouse = Input.LeftMouse, leftMouseDown = Input.LeftMouseDown, rightMouse = Input.RightMouse, rightMouseDown = Input.RightMouseDown;
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
                    highlightedPieces[i].Renderer.Color = highlightedPieces[i].Player == InGameController.PlayerIndex ? pieceHighlightColor : pieceEnemyHighlightColor;
                    highlightedPiece = highlightedPieces[i];

                    continue;
                }

                highlightedPieces[i].Renderer.Color = pieceCoveredColor;
            }

            if (highlightedPiece != null && interactionPiece == null)
            {
                AddHighlight(highlightedPiece.Player == InGameController.PlayerIndex ? defaultHighlightColor : defaultEnemyHighlightColor, highlightedPiece.Position);
            }

            if (!leftMouse && interactionPiece != null)
            {
                float distance = (Input.MousePosition.ToVector2() - mouseDownPosition.ToVector2()).Length();

                if (moveable && movementHighlight != null)
                {
                    if (movementHighlight.Contains(MousePoint.ToRender()))
                    {
                        if (InGameController.LocalMana >= interactionPiece.Piece.Bottom.MoveCost)
                        {
                            player.ExecuteMovement(interactionPiece, MousePoint.ToRender());
                        }
                    }
                }

                if (moveable && meleeHighlight != null)
                {
                    if (meleeHighlight.Contains(MousePoint.ToRender()))
                    {
                        try
                        {
                            Point targetPoint = MousePoint.ToRender();
                            player.ExecuteMelee(interactionPiece, InGameController.Grid.GetObject(targetPoint.X, targetPoint.Y) as TilePiece);
                        }
                        catch { }
                    }
                }

                if (distance < DRAGDISTANCE)
                {
                    if (Ztuff.pickingPiece == true)
                    {
                        MyEffectCache.AListOfSints.Add(new Sints(Ztuff.incomingEffect, interactionPiece.GridIndex));
                        Ztuff.RestoreFromBuff();
                        Ztuff.pickingPiece = false;
                    }
                    else if (interactionPiece.Player == InGameController.PlayerIndex)
                    {
                        player.RequestAction(interactionPiece);
                    }
                }

                ghost?.Destroy();
                ghost = null;
                meleeHighlight = null;
                movementHighlight = null;
                interactionPiece = null;
            }

            if (leftMouseDown)
            {
                if (highlightedPiece != null)
                {
                    interactionPiece = highlightedPiece;
                    mouseDownPosition = Input.MousePosition;
                }

                infoBox.Close();
            }

            if (rightMouseDown)
            {
                if (highlightedPiece != null)
                {
                    infoBox.Set(highlightedPiece);
                    infoBox.Open();
                }
                else
                {
                    infoBox.Close();
                }
            }

            if (moveable)
            {
                BoardMove(leftMouseDown, leftMouse, highlightedPiece);
            }
        }

        void BoardMove(bool leftMouseDown, bool leftMouse, TilePiece highlightedPiece)
        {
            if (leftMouse && interactionPiece != null)
            {
                float distance = (Input.MousePosition.ToVector2() - mouseDownPosition.ToVector2()).Length();

                AddHighlight(Color.White, interactionPiece.Position.ToRender());

                if (movementHighlight != null && movementHighlight.Length > 0 && !interactionPiece.Piece.HasMoved)
                {
                    foreach (Point point in movementHighlight)
                    {
                        AddHighlight(point == MousePoint.ToRender() ? movementSelectedHighlightColor : movementHighlightColor, point);
                    }
                }

                if (meleeHighlight != null && meleeHighlight.Length > 0 && !interactionPiece.Piece.HasAttacked)
                {
                    foreach (Point point in meleeHighlight)
                    {
                        AddHighlight(point == MousePoint.ToRender() ? meleeSelectedColor : meleeHighlightColor, point);
                    }
                }

                if (distance > DRAGDISTANCE && interactionPiece.Player == InGameController.PlayerIndex)
                {
                    if (ghost == null)
                    {
                        ghost = new Renderer.Sprite(Layer.Default, interactionPiece.Renderer.Texture, MousePosition, Vector2.One, pieceGhostColor, 0, interactionPiece.Renderer.Origin, SpriteEffects.None);
                    }

                    if (meleeHighlight == null)
                    {
                        meleeHighlight = player.RequestMelee(interactionPiece);
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
            hud.BattleHUD.AddPiece(newPiece);

            UpdateHandPieces();
        }

        void UpdateHandPieces()
        {
            InGamePiece removePiece = null;

            if (dragOutPiece != null)
            {
                List<Point> placementTiles = PlacementTiles();

                if (Input.LeftMouse)
                {
                    if (placementTiles.Contains(MousePoint.ToRender()))
                    {
                        placementTiles.Remove(MousePoint.ToRender());
                        AddHighlight(movementSelectedHighlightColor, MousePoint.ToRender());
                    }

                    AddHighlight(defaultHighlightColor, placementTiles.ToArray());

                    if (ghost == null)
                    {
                        Vector2 origin = new Vector2(dragOutPiece.Texture.Width - 13, dragOutPiece.Texture.Height - 9);
                        ghost = new Renderer.Sprite(Layer.Default, dragOutPiece.Texture, MousePositionAbsolute, Vector2.One, pieceGhostColor, 0, origin, SpriteEffects.None);
                    }

                    ghost.Position = MousePositionAbsolute;
                    ghost.Layer = new Layer(MainLayer.Main, TileObject.DefaultLayer((int)(MousePosition.Y)).layer + 1);
                }
                else
                {
                    ghost?.Destroy();
                    ghost = null;

                    if (/*InGameController.Grid.Vacant(MousePoint.ToRender().X, MousePoint.ToRender().Y) && */placementTiles.Contains(MousePoint.ToRender()))
                    {
                        if (InGameController.LocalMana >= dragOutPiece.GetCost)
                        {
                            removePiece = dragOutPiece;
                            player.PlacePiece(dragOutPiece, MousePoint.ToRender().X, MousePoint.ToRender().Y);

                            InGameController.LocalMana -= dragOutPiece.GetCost;
                        }
                            
                        dragOutPiece = null;
                    }
                    else
                    {
                        dragOutPiece = null;
                    }
                }
            }

            hud.BattleHUD.UpdateHand(removePiece, ref dragOutPiece);
        }

        List<Point> PlacementTiles()
        {
            List<Point> points = new List<Point>();
            bool isHost = InGameController.IsHost ? true : false;

            for (int i = 0; i < InGameController.Grid.xLength; ++i)
            {
                for (int j = 0; j < placeHeight; ++j)
                {
                    if (isHost)
                    {
                        if (InGameController.Grid.Vacant(i, InGameController.Grid.yLength - j - 1))
                        {
                            points.Add(new Point(i, InGameController.Grid.yLength - j - 1));
                        }
                    }
                    else
                    {
                        if (InGameController.Grid.Vacant(i, j))
                        {
                            points.Add(new Point(i, j));
                        }
                    }
                }
            }

            return points;
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

            bool endScreen = gameState == InGameState.End;

            if (!splashTable.Complete)
            {
                int state = splashTable.Update(deltaTime);
                float currentProgress = splashTable.CurrentStepProgress;
                
                if (state == 0)
                {
                    splash.Scale = Vector2.One * (Easing.EaseOutCubic(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (currentProgress)));

                    if (endScreen)
                    {
                        //bReturn.SetPseudoDefaultColors(new Color(1.0f, 1.0f, 1.0f, currentProgress));
                    }
                }

                if (state == 1)
                {
                    splash.Scale = Vector2.One * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = dimColor;

                    if (endScreen)
                    {
                        splashTable.End();
                        //bReturn.SetPseudoDefaultColors(Color.White);
                    }
                }

                if (state == 2)
                {
                    splash.Scale = Vector2.One * (1 - Easing.EaseInBack(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (1 - currentProgress)));
                }
            }
            else if (dim.Color.A != 0 && !endScreen)
            {
                splash.Scale = Vector2.Zero;
                dim.Color = Color.Transparent;
            }
        }

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
