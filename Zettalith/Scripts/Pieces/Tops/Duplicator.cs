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
            AttackDamage = 1;
            AbilityRange = 1;
            ManaCost = new Mana(0, 1, 4);
            AbilityCost = new Mana(0, 1, 3);
            // Modifier = new Addition();
            Texture = Load.Get<Texture2D>("DuplicateHead");

            Description = "Duplicates itself.";
        }

        public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        {
            List<Point> points = Abilities.SquareAoE(piece.Position, AbilityRange, true);
            List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

            for (int i = 0; i < sPoints.Count; ++i)
                if (InGameController.Grid.GetObject(sPoints[i].X, sPoints[i].Y) != null)
                    sPoints.Remove(sPoints[i]);

            ClientSideController.AddHighlight(sPoints.ToArray().ToPointArray());

            if (mouseDown)
            {
                for (int i = 0; i < sPoints.Count; ++i)
                {
                    if (mousePos == sPoints[i])
                    {
                        object[] temp = { (SPoint)mousePos, piece.GridIndex };
                        cancel = false;
                        return temp;
                    }
                }

                cancel = true;
                return null;
            }

            cancel = false;
            return null;
        }

        public override void ActivateAbility(object[] data)
        {
            Point pos = (SPoint)data[0];
            TilePiece duplicator = (TilePiece)InGameController.Grid[(int)data[1]];
            InGamePiece piece = new InGamePiece(new Piece((byte)duplicator.Piece.Top.ToIndex(), (byte)duplicator.Piece.Middle.ToIndex(), (byte)duplicator.Piece.Bottom.ToIndex()));
            InGameController.Main.PlacePiece(piece.Index, pos.X, pos.Y, InGameController.PlayerIndex);

            //TilePiece temp = new TilePiece(new InGamePiece(piece.Piece.Piece), piece.Player);
            //InGameController.Main.PlacePiece((int)data[1], pos.X, pos.Y, piece.Player);
        }
    }
}
