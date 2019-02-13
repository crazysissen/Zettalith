using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class TestTop3 : Top
    {
        public TestTop3()
        {
            Name = "Woop";
            Health = 10;
            AttackDamage = 2;
            ManaCost = new Mana(4, 2, 0);
            Texture = Load.Get<Texture2D>("TestSubpiece");
            Description = "Deals 3 damage to all units in a straight line.";
            Modifier = new Addition(new Stats(-3), false);
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<SPoint> spoints = Abilities.Beam(piece.Position, mousePos).Cast<SPoint>().ToList();

            if (spoints.Count == 0)
            {
                if (mouseDown)
                {
                    cancel = true;
                    return null;
                }

                cancel = false;
                return null;
            }

            // TODO: Highlight spoints list

            if (mouseDown)
            {
                object[] temp = { spoints, Modifier };
                cancel = false;
                return temp;
            }

            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            for (int i = 0; i < (data[0] as List<SPoint>).Count; ++i)
            {
                (InGameController.Grid.GetObject((data[0] as List<SPoint>)[i].X, (data[0] as List<SPoint>)[i].Y) as TilePiece).Piece.ModThis(data[1] as Modifier);
            }
        }
    }
}