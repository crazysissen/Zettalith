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
            HEIGHTDISTANCE = 0.6875f;

        static readonly Color
            defaultHighlightColor = new Color(0, 255, 215, 255);

        GUI.Collection collection, battleGUI, logisticsGUI, setupGUI;

        Renderer.Text splash, essence;
        Renderer.Text[] mana;
        Renderer.SpriteScreen dim, bottomPanel, topPanel, essencePanel;

        List<Point> highlightSquares;

        TimerTable table;

        public GameRendering(Player player, bool host)
        {
            dim = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 50), Load.Get<Texture2D>("Square"), new Rectangle(Point.Zero, Settings.GetResolution), new Color(60, 60, 60, 160));


            CreateBattleGUI();
            CreateLogisticsGUI();
            CreateSetupGUI();

            collection.Add(dim, splash, essencePanel, essence);
        }

        public void Render(float deltaTime)
        {
            highlights.Clear();
        }


        void CreateBattleGUI()
        {

        }

        void CreateLogisticsGUI()
        {
            // TODO Add logistics UI
        }

        void CreateSetupGUI()
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
