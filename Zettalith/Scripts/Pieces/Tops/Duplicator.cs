using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Duplicator : Top
    {
        public Duplicator()
        {
            Name = "Duplicator";
            Health = 1;
            AttackDamage = 2;
            ManaCost = new Mana(0, 0, 2);
            AbilityCost = new Mana(0, 0, 2);
            Description = "Duplicates itself.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
            AbilityRange = 1;
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<SPoint> spoints = Abilities.SquareAoE(piece.Position, AbilityRange).Cast<SPoint>().ToList();

            for (int i = 0; i < spoints.Count; ++i)
                if (InGameController.Grid.GetObject(spoints[i].X, spoints[i].Y) != null)
                    spoints.Remove(spoints[i]);

            ClientSideController.AddHighlight(spoints.Cast<Point>().ToArray());

            if (mouseDown)
            {
                for (int i = 0; i < spoints.Count; ++i)
                {
                    if (mousePos == spoints[i])
                    {
                        object[] temp = { mousePos, piece };
                        cancel = false;
                        return temp;
                    }
                }
            }

            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            Point point = data[0] as SPoint;
            InGameController.Grid.Place(point.X, point.Y, data[1] as TilePiece);
        }
    }
}
