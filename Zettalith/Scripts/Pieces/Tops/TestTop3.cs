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
            if (!mouseDown)
            {
                cancel = false;
                return null;
            }

            List<SPoint> spoints = Abilities.Beam(piece.Position, mousePos).Cast<SPoint>().ToList();

            if (spoints.Count == 0)
            {
                cancel = true;
                return null;
            }

            // TODO: Highlight spoints list

            object[] temp = new object[3];

            temp[0] = spoints;
            temp[1] = Modifier;

            cancel = false;
            return temp;
        }

        public override void ActivateAbility(TilePiece piece, object[] data)
        {
            for (int i = 0; i < (data[0] as List<SPoint>).Count; ++i)
            {
                (InGameController.Grid.GetObject((data[0] as List<SPoint>)[i].X, (data[0] as List<SPoint>)[i].Y) as TilePiece).Piece.ModThis(data[1] as Modifier);
            }
        }
    }
}