using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    static class Font
    {
        public static SpriteFont Default { get; private set; }
        public static SpriteFont Bold { get; private set; }
        public static SpriteFont Italic { get; private set; }
        public static SpriteFont Styled { get; private set; }
        public static SpriteFont Small { get; private set; }

        public static void Initialize()
        {
            Default = Load.Get<SpriteFont>("FontDefault");
            Bold = Load.Get<SpriteFont>("FontDefaultBold");
            Italic = Load.Get<SpriteFont>("FontDefaultItalic");
            Styled = Load.Get<SpriteFont>("FontStyled");
            Small = Load.Get<SpriteFont>("FontSmall");
        }
    }
}
