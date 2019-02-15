using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    abstract class Top : SubPiece
    {
        public Modifier Modifier { get; protected set; }
        public Mana AbilityCost { get; protected set; } = new Mana(0, 0, 0);

        public bool HasAbility { get; protected set; }

        public int AbilityRange { get; protected set; }

        // TODO: Make abstract
        public virtual void InitializeAbility()
        {

        }

        public virtual object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            cancel = true;
            return null;
        }

        public virtual void ActivateAbility(object[] data)
        {

        }

        //public int ToIndex()
        //{
        //    return Tops.tops.IndexOf(GetType());
        //}

        //public Top FromIndex(int index)
        //{
        //    return (Top)Activator.CreateInstance(Tops.tops[index]);
        //}
    }
}