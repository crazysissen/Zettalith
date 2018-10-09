using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Scripts
{
    class GElement
    {
        Rectangle transform;
    }

    abstract class Label : GElement
    {
        SpriteFont font;
    }

    abstract class Button : GElement
    {

    }

    abstract class Slider : GElement
    {

    }

    abstract class DropDown : GElement
    {

    }

    abstract class TextInput : GElement
    {

    }
}
