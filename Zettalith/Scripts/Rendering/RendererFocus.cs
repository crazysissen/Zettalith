using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    sealed class RendererFocus
    {
        enum FocusType { BlockUnder, BlockArea, BlockUnderArea }

        public bool Active { get; set; } = true;

        public Layer? Layer { get; set; }
        public Rectangle? Rectangle { get; set; }
        public bool? OutsideArea { get; set; }

        private readonly FocusType type;

        /// <summary>Block anything under a specific layer</summary>
        public RendererFocus(Layer layer)
        {
            foci.Add(this);
            type = FocusType.BlockUnder;

            Layer = layer;

            Rectangle = null;
            OutsideArea = null;
        }

        /// <summary>Block anything according to an area (outside the area if the bool is true, inside otherwise)</summary>
        public RendererFocus(Rectangle rectangle, bool outsideArea)
        {
            foci.Add(this);
            type = FocusType.BlockArea;

            Rectangle = rectangle;
            OutsideArea = outsideArea;

            Layer = null;
        }

        /// <summary>Block anything under a layer according to an area (outside the area if the bool is true, inside otherwise). Good for a gui element that overlaps other interactibles, where everything under the menu is blocked.</summary>
        public RendererFocus(Layer layer, Rectangle rectangle, bool outsideArea)
        {
            foci.Add(this);
            type = FocusType.BlockUnderArea;

            Layer = layer;
            Rectangle = rectangle;
            OutsideArea = outsideArea;
        }

        public void Remove()
        {
            foci.Remove(this);
        }

        #region Static Application

        private static List<RendererFocus> foci = new List<RendererFocus>();

        public static bool OnArea(Rectangle area, Layer layer)
        {
            MouseState state = Mouse.GetState();

            bool initial = area.Contains(state.Position);

            if (!initial)
            {
                return false;
            }

            foreach (RendererFocus focus in foci)
            {
                if (!focus.Active)
                {
                    continue;
                }

                switch (focus.type)
                {
                    case FocusType.BlockUnder:
                        initial = layer.LayerDepth >= focus.Layer.Value.LayerDepth;
                        break;

                    case FocusType.BlockArea:
                        initial = focus.OutsideArea.Value ? !focus.Rectangle.Value.Contains(state.Position) : focus.Rectangle.Value.Contains(state.Position);
                        break;

                    case FocusType.BlockUnderArea:
                        initial = layer.LayerDepth >= 
                            focus.Layer.Value.LayerDepth &&
                            focus.OutsideArea.Value ? focus.Rectangle.Value.Contains(state.Position) : !focus.Rectangle.Value.Contains(state.Position);
                        break;
                }

                if (!initial)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
