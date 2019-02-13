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

        GUI.Collection battleGUI, logisticsGUI, setupGUI;

        List<Point> highlightSquares;

        TimerTable table;

        public GameRendering(Player player)
        {


            CreateBattleGUI();
            CreateLogisticsGUI();
            CreateSetupGUI();
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

        }

        void CreateSetupGUI()
        {
            
        }

        #region Tile Highlighting

        static List<(Point, Color)> highlights = new List<(Point, Color)>();

        public static void AddHighlight(Point[] points, Color color)
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
