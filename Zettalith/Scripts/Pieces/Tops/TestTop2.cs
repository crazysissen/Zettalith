using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class TestTop2 : Top
    {
        public TestTop2()
        {
            Name = "Pyro";
            Health = 20;
            AttackDamage = 3;
            ManaCost = new Mana(0, 2, 0);
            Description = "Deals 1 damage to all Zettaliths.";
            Texture = Load.Get<Texture2D>("TestSubpiece2");
            Modifier = new Addition(new Stats(-1), true);
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<SPoint> spoints = Abilities.TargetAll().Cast<SPoint>().ToList();

            ClientSideController.AddHighlight(spoints.Cast<Point>().ToArray());

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
            List<Point> temp = (data[0] as List<SPoint>).Cast<Point>().ToList();

            for (int i = 0; i < temp.Count; ++i)
            {
                TileObject piece = InGameController.Grid.GetObject(temp[i].X, temp[i].Y);

                if (piece == null || !(piece is TilePiece))
                    continue;

                (piece as TilePiece).Piece.ModThis(data[1] as Modifier);
            }
        }
    }
}
