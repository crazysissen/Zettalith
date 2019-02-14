using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class GameRendering
    {
        public const float
            HEIGHTDISTANCE = 0.6875f,
            SPLASHSIZE = 6;

        static readonly Color
            defaultHighlightColor = new Color(0, 255, 215, 255),
            dimColor = new Color(0, 0, 0, 160);

        public static Vector2 MousePositionAbsolute => RendererController.Camera.ScreenToWorldPosition(In.MousePosition.ToVector2());
        public static Vector2 MousePosition => new Vector2(MousePositionAbsolute.X, MousePositionAbsolute.Y / HEIGHTDISTANCE);
        public static Point MousePoint => MousePosition.RoundToPoint();

        public bool MoveableCamera { get; set; } = true;
        public bool SetupComplete { get; private set; }

        public Renderer.Sprite[,] tiles, highlights;

        GUI.Collection collection, battleGUI, logisticsGUI, setupGUI;

        Renderer.Text splash, essence;
        Renderer.Text[] mana;
        Renderer.SpriteScreen dim, bottomPanel, topPanel, essencePanel;
        List<(Renderer.SpriteScreen renderer, InGamePiece piece)> handPieces;
        GUI.Button bEndTurn;

        List<Point> highlightSquares;
        Vector2 previousPosition;

        TimerTable table;
        Player player;

        Point handStart, handEnd;

        public GameRendering(Player player, bool host, bool start)
        {
            this.player = player;

            dim = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 50), Load.Get<Texture2D>("Square"), new Rectangle(Point.Zero, Settings.GetResolution), dimColor);

            string splashText = start ? "You Start" : "Opponent Starts";
            splash = new Renderer.Text(new Layer(MainLayer.GUI, 50), Font.Bold, splashText, SPLASHSIZE, 0, Settings.GetHalfResolution.ToVector2(), 0.5f * Font.Bold.MeasureString(splashText), defaultHighlightColor);

            CreateBattleGUI();
            CreateLogisticsGUI();
            CreateSetupGUI();

            collection = new GUI.Collection();
            collection.Add(dim, splash, essencePanel, essence);

            RendererController.GUI.Add(collection);
            collection.Add(battleGUI, logisticsGUI, setupGUI);

            table = new TimerTable(new float[] { 1, 2 });
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
                    tiles[x, y] = new Renderer.Sprite(new Layer(MainLayer.Background, y - grid.yLength), tileTexture, new Vector2(x, y * HEIGHTDISTANCE), Vector2.One, Color.White, 0, new Vector2(16, 11), SpriteEffects.None);

                    if (!InGameController.IsHost)
                    {
                        tiles[x, y].Position *= -1;
                    }
                }
            }
        }

        public void Render(float deltaTime)
        {
            if (!SetupComplete)
            {
                int state = table.Update(deltaTime);
                float currentProgress = table.CurrentStepProgress;

                if (table.Complete)
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

            if (!table.Complete)
            {
                int state = table.Update(deltaTime);
                float currentProgress = table.CurrentStepProgress;

                if (state == 0)
                {
                    splash.Scale = Vector2.One * (Easing.EaseOutCubic(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (currentProgress)));
                }

                if (state == 1)
                {
                    splash.Scale = Vector2.One * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                    dim.Color = dimColor;
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



            if (MoveableCamera)
            {
                Vector2 currentPosition = MousePositionAbsolute;

                if (In.MouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    RendererController.Camera.Position += new Vector2(previousPosition.X - currentPosition.X, previousPosition.Y - currentPosition.Y);
                }

                previousPosition = MousePositionAbsolute;
            }

            queuedHighlights.Clear();
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

            CreateMap(InGameController.Grid);

            SetupComplete = true;

            InGameController.Main.SetupEnd();
        }

        void CreateBattleGUI()
        {
            battleGUI = new GUI.Collection();

            int width = (int)(Settings.GetResolution.X * (401f / 480f)), height = (int)(Settings.GetResolution.Y * (24f / 270f)),
                buttonWidth = (int)(Settings.GetResolution.X * (51f / 480f)), buttonHeight = (int)(Settings.GetResolution.Y * (25f / 270f));

            bottomPanel = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("BottomPanel"), new Rectangle(Settings.GetHalfResolution.X - width / 2, Settings.GetResolution.Y - height, width, height));

            bEndTurn = new GUI.Button(new Layer(MainLayer.GUI, 1), new Rectangle(Settings.GetHalfResolution.X - buttonWidth / 2, (int)(Settings.GetResolution.Y * 0.885f), buttonWidth, buttonHeight), Load.Get<Texture2D>("EndTurnButton"), Load.Get<Texture2D>("EndTurnButtonHover"), Load.Get<Texture2D>("EndTurnButtonClick")) { ScaleEffect = true };
            bEndTurn.OnClick += player.EndTurn;

            handStart = new Point((int)(Settings.GetResolution.X * 0.15f), Settings.GetResolution.Y - height / 2);
            handStart = new Point((int)(Settings.GetResolution.X * 0.40f), Settings.GetResolution.Y - height / 2);

            battleGUI.Add(bottomPanel, bEndTurn);
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

        public void CloseSetup()
        {

        }

        public void OpenBattle()
        {
            battleGUI.Active = true;
            logisticsGUI.Active = false;

            splash.String = new StringBuilder("Your Turn");
            table = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void OpenLogistics()
        {
            battleGUI.Active = false;
            logisticsGUI.Active = true;

            splash.String = new StringBuilder("Opponent's Turn");
            table = new TimerTable(new float[] { 0.4f, 0.6f, 0.3f });
        }

        public void DrawPiece(InGamePiece piece)
        {
            //handPieces.Add((new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 1), piece.Texture, ec), piece));
        }

        //private Rectangle GetTargetRectangle(int index, int count)
        //{
        //    //return new Rectangle(handStart.ToVector2().)
        //}

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
