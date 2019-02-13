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
        const float
            HEIGHTDISTANCE = 0.6875f,
            SPLASHSIZE = 6;

        static readonly Color
            defaultHighlightColor = new Color(0, 255, 215, 255),
            dimColor = new Color (0, 0, 0, 160);

        public bool SetupComplete { get; private set; }

        GUI.Collection collection, battleGUI, logisticsGUI, setupGUI;

        Renderer.Text splash, essence;
        Renderer.Text[] mana;
        Renderer.SpriteScreen dim, bottomPanel, topPanel, essencePanel;

        List<Point> highlightSquares;

        TimerTable table;

        public GameRendering(Player player, bool host, bool start)
        {
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

        public void Render(float deltaTime)
        {
            if (!SetupComplete)
            {
                int state = table.Update(deltaTime);
                float currentProgress = table.CurrentStepProgress;

                if (table.Complete)
                {
                    splash.Scale = Vector2.Zero;
                    dim.Color = Color.Transparent;
                }
                else
                {
                    if (state == 1)
                    {
                        splash.Scale = Vector2.One * (1 - Easing.EaseInBack(currentProgress)) * SPLASHSIZE * Renderer.FONTSIZEMULTIPLIER;
                        dim.Color = new Color(dimColor.R, dimColor.G, dimColor.B, (byte)(dimColor.A * (1 - currentProgress)));
                    }
                }
            }

            highlights.Clear();
        }


        void CreateBattleGUI()
        {
            battleGUI = new GUI.Collection();
        }

        void CreateLogisticsGUI()
        {
            logisticsGUI = new GUI.Collection();

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

        }

        public void OpenLogistics()
        {

        }

        #region Tile Highlighting

        static List<(Point, Color)> highlights = new List<(Point, Color)>();

        public static void AddHighlight(params Point[] points)
        {
            AddHighlight(defaultHighlightColor, points);
        }

        public static void AddHighlight(Color color, params Point[] points)
        {
            foreach (Point point in points)
            {
                highlights.Add((point, color));
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
