using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    abstract class Modifier
    {
        public abstract void Activate();
    }

    class HealthModifier : Modifier
    {

    }

    class ManaModifier : Modifier
    {

    }

    class CustomModifier : Modifier
    {
        public delegate void Manipulate(Piece piece, int turnsElapsed);

        public CustomModifier(Manipulate manipulationCommand)
        {

        }
    }
}