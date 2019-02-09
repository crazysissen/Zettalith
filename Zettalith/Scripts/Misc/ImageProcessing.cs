using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class ImageProcessing
    {
        public const int
            PIECEWIDTH = 26,
            FACEHEIGHT = 18;

        public static Texture2D CombinePiece(Texture2D top, Texture2D middle, Texture2D bottom)
        {
            int bH = bottom.Height, mH = middle.Height, tH = top.Height,
                totalFaceHeight = bH + mH + tH - FACEHEIGHT * 3;

            Texture2D newTexture = new Texture2D(XNAController.Graphics.GraphicsDevice, PIECEWIDTH, bottom.Height + middle.Height + top.Height - FACEHEIGHT * 2);

            Color[]
                bData = new Color[bottom.Width * bottom.Height],
                mData = new Color[middle.Width * middle.Height],
                tData = new Color[top.Width * top.Height],
                newData = new Color[newTexture.Height * newTexture.Width];

            bottom.GetData(bData);
            middle.GetData(mData);
            top.GetData(tData);

            for (int y = 0; y < newTexture.Height; ++y)
            {
                int mY = y - tH + FACEHEIGHT,
                    bY = y - tH - mH + FACEHEIGHT * 2;

                for (int x = 0; x < PIECEWIDTH; ++x)
                {
                    int current = y * PIECEWIDTH + x;

                    if (y < tH)
                    {
                        if (tData[current] != null && tData[current].A != 0)
                        {
                            newData[current] = tData[current];
                            continue;
                        }
                    }

                    if (mY >= 0 && mY < mH)
                    {
                        int mCurrent = mY * PIECEWIDTH + x;
                        if (mData[mCurrent] != null && mData[mCurrent].A != 0)
                        {
                            newData[current] = mData[mCurrent];
                            continue;
                        }
                    }

                    if (bY >= 0 && bY < bH)
                    {
                        int bCurrent = bY * PIECEWIDTH + x;
                        if (bData[bCurrent] != null && bData[bCurrent].A != 0)
                        {
                            newData[current] = bData[bCurrent];
                            continue;
                        }
                    }

                    newData[current] = Color.Transparent;
                }
            }

            newTexture.Name = string.Format("Piece[B: {0}, M: {1}, T: {2}]", bottom.Name, middle.Name, top.Name);
            newTexture.SetData(newData);

            return newTexture;
        }
    }
}
