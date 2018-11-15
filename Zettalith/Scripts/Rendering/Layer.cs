using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    public enum MainLayer
    {
        AbsoluteBottom, Background, Main, Overlay, GUI, AbsoluteTop
    }

    public struct Layer
    {
        public const int
            MAINCOUNT = 6;

        public const float
            LAYERINTERVAL = float.Epsilon,
            MAININTERVAL = 1 / MAINCOUNT,
            HALFINTERVAL = MAININTERVAL * 0.5f;

        public float LayerDepth => 1 - ((int)main * MAININTERVAL + HALFINTERVAL + LAYERINTERVAL * layer);

        public MainLayer main;
        public int layer;

        public Layer(MainLayer main, int layer)
        {
            this.main = main;
            this.layer = layer;
        }

        public static Layer Default => new Layer(MainLayer.Main, 0);

        public static implicit operator Layer((MainLayer tupleMain, int tupleLayer) tuple)
            => new Layer(tuple.tupleMain, tuple.tupleLayer);
    }
}
