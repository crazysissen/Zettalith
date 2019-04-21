using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    //class Imitator : Top
    //{
    //    public Imitator()
    //    {
    //        Name = "Imitator";
    //        Health = 1;
    //        AttackDamage = 0;
    //        AbilityRange = 1;
    //        ManaCost = new Mana(0, 3, 0);
    //        AbilityCost = new Mana(0, 5, 0);
    //        // Modifier = new Addition(new Stats(-9), true);
    //        Texture = Load.Get<Texture2D>("Top");

    //        Description = "Become a copy of another Zettalith";
    //    }

    //    public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
    //    {
    //        List<Point> points = Abilities.TargetAll(piece.Position, AbilityRange);
    //        List<SPoint> sPoints = new List<SPoint>(points.ToArray().ToSPointArray());

    //        ClientSideController.AddHighlight(points.ToArray());

    //        if (mouseDown)
    //        {
    //            for (int i = 0; i < sPoints.Count; ++i)
    //            {
    //                if (mousePos == sPoints[i])
    //                {
    //                    object[] temp = { sPoints[i], piece.GridIndex };
    //                    cancel = false;
    //                    return temp;
    //                }
    //            }

    //            cancel = true;
    //            return null;
    //        }

    //        cancel = false;
    //        return null;
    //    }

    //    public override void ActivateAbility(object[] data)
    //    {
    //        Point pos = InGameController.Grid.PositionOf((int)data[1]);
    //        TilePiece toCopy = (TilePiece)InGameController.Grid.GetObject(((SPoint)data[0]).X, ((SPoint)data[0]).Y);

    //        InGamePiece copy = new InGamePiece(new Piece((byte)toCopy.Piece.Top.ToIndex(), (byte)toCopy.Piece.Middle.ToIndex(), (byte)toCopy.Piece.Bottom.ToIndex()));
    //        InGameController.Grid.Remove((int)data[1]);
    //        InGameController.Main.PlacePiece()
    //    }
    //}
}
