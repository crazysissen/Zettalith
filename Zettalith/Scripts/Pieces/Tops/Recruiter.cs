using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class Recruiter : Top
    {
        public Recruiter()
        {
            Name = "Recruiter";
            Health = 5;
            AttackDamage = 2;
            AbilityRange = 0;
            ManaCost = new Mana(0, 5, 0);
            AbilityCost = new Mana(0, 4, 0);
            // Modifier = 
            Texture = Load.Get<Texture2D>("RecruiterTop");

            Description = "Draws a Zettalith from your deck";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            SPoint point = new SPoint(piece.Position.X, piece.Position.Y);

            ClientSideController.AddHighlight(point);

            if (mouseDown)
            {
                if (mousePos == point)
                {
                    object[] temp = { point };
                    cancel = false;
                    return temp;
                }

                cancel = true;
                return null;
            }

            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            InGameController.Local.ClientController.DrawPieceFromDeck();
        }
    }
}
