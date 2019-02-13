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
            Name = "TestTop2";
            Health = 20;
            AttackDamage = 3;
            ManaCost = new Mana(0, 2, 0);
            Description = "Deals 5 damage to target enemy unit";
            Texture = Load.Get<Texture2D>("TestSubpiece2");
            Modifier = new Addition(new Stats(-5), true);
        }

        //public override object[] UpdateAbility(TilePiece piece, Point mousePos, bool mouseDown, out bool cancel)
        //{
        //    List<SPoint> spoints = Abilities.Target(true).Cast<SPoint>().ToList();

        //    if (spoints.Count == 0)
        //    {
        //        if (mouseDown)
        //        {
        //            cancel = true;
        //            return null;
        //        }

        //        cancel = false;
        //        return null;
        //    }

        //    GameRendering.AddHighlight(spoints.Cast<Point>().ToArray());

        //    if (mouseDown)
        //    {
        //        object[] temp = { spoints, Modifier };
        //        cancel = false;
        //        return temp;
        //    }

        //    cancel = false;
        //    return null;
        //}

        //public override void ActivateAbility(object[] data)
        //{
        //    List<Point> temp = (data[0] as List<SPoint>).Cast<Point>().ToList();

        //    for (int i = 0; i < temp.Count; ++i)
        //    {
        //        TileObject piece = InGameController.Grid.GetObject(temp[i].X, temp[i].Y);

        //        if (piece == null || !(piece is TilePiece))
        //            continue;

        //        (piece as TilePiece).Piece.ModThis(data[1] as Modifier);
        //        //(InGameController.Grid.GetObject((data[0] as List<SPoint>)[i].X, (data[0] as List<SPoint>)[i].Y) as TilePiece).Piece.ModThis(data[1] as Modifier);
        //    }
        //}
    }
}
