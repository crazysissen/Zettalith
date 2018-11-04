using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    public struct Layer
    {
        public enum Main
        {
            AbsoluteBottom, Background, Main, Overlay, GUI, AbsoluteTop
        }

        public const int
            MAINCOUNT = 6;

        public const float
            LAYERINTERVAL = float.Epsilon,
            MAININTERVAL = 1 / MAINCOUNT,
            HALFINTERVAL = MAININTERVAL * 0.5f;

        public float LayerDepth => 1 - ((int)main * MAININTERVAL + HALFINTERVAL + LAYERINTERVAL * layer);

        public Main main;
        public int layer;

        public Layer(Main main, int layer)
        {
            this.main = main;
            this.layer = layer;

            Layer laer = (Main.Main, 1);
        }


        public static implicit operator Layer((Main tupleMain, int tupleLayer) tuple)
        {
            return new Layer(tuple.tupleMain, tuple.tupleLayer);
        }
    }
}
