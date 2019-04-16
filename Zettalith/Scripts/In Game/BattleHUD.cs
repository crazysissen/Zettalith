using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class BattleHUD : HUD
    {
        GUI.Collection collection;

        int handPieceHeight;
        List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)> handPieces;

        Renderer.SpriteScreen bottomPanel;
        GUI.Button bEndTurn;

        Point handStart, handEnd;

        public BattleHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            handPieces = new List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)>();
            handPieceHeight = Settings.GetResolution.Y / 5;

            int width = (int)(Settings.GetResolution.X * (401f / 480f)), height = (int)(Settings.GetResolution.Y * (24f / 270f)),
                buttonWidth = (int)(Settings.GetResolution.X * (51f / 480f)), buttonHeight = (int)(Settings.GetResolution.Y * (25f / 270f));

            bottomPanel = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("BottomPanel"), new Rectangle(Settings.GetHalfResolution.X - width / 2, Settings.GetResolution.Y - height, width, height));

            bEndTurn = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle(Settings.GetHalfResolution.X - buttonWidth / 2, (int)(Settings.GetResolution.Y * 0.885f), buttonWidth, buttonHeight), Load.Get<Texture2D>("EndTurnButton"), Load.Get<Texture2D>("EndTurnButtonHover"), Load.Get<Texture2D>("EndTurnButtonClick")) { ScaleEffect = true };
            bEndTurn.OnClick += InGameController.Local.EndTurn;

            handStart = new Point((int)(Settings.GetResolution.X * 0.15f), Settings.GetResolution.Y - height * 2);
            handEnd = new Point((int)(Settings.GetResolution.X * 0.30f), Settings.GetResolution.Y - height * 2);

            GUI.Button button = new GUI.Button(Layer.GUI, new Rectangle(10, 10, 160, 60));
            button.AddText("Draw Piece", 3, true, Color.Black, Font.Bold);
            button.OnClick += clientSideController.DrawPieceFromDeck;

            collection.Add(bottomPanel, bEndTurn, button);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {
            
        }

        public void UpdateHand(InGamePiece removePiece, ref InGamePiece dragOutPiece)
        {
            for (int i = handPieces.Count - 1; i >= 0; --i)
            {
                (Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus) item = handPieces[i];

                if (item.piece == removePiece)
                {
                    handPieces.RemoveAt(i);
                    item.renderer.Destroy();
                    item.focus.Remove();
                    collection.Remove(item.renderer);
                    continue;
                }

                Rectangle rectangle = GetTargetRectangle(i, handPieces.Count);

                bool color = false;

                if (dragOutPiece == null && rectangle.Contains(In.MousePosition) && !(i < handPieces.Count - 1 && GetTargetRectangle(i + 1, handPieces.Count).Contains(In.MousePosition)))
                {
                    color = true;
                    if (In.LeftMouseDown)
                    {
                        dragOutPiece = item.piece;
                    }
                }

                item.renderer.Color = color ? ClientSideController.pieceHighlightColor : Color.White;
                item.renderer.Layer = new Layer(MainLayer.GUI, 1 + i);
                item.renderer.Transform = rectangle;
                item.focus.Rectangle = rectangle;
            }
        }

        public void AddPiece(InGamePiece newPiece)
        {
            Renderer.SpriteScreen sc = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 1), newPiece.Texture, new Rectangle());
            collection.Add(sc);

            handPieces.Add((sc, newPiece, new RendererFocus(new Layer(MainLayer.GUI, 1), new Rectangle(), false)));
        }

        private Rectangle GetTargetRectangle(int index, int count)
            => new Rectangle(handStart.ToVector2().Lerp(handEnd.ToVector2(), count == 1 ? 0 : index / (count - 1.0f)).RoundToPoint(),
                new Point((int)(handPieceHeight * ((float)handPieces[index].piece.Texture.Width / handPieces[index].piece.Texture.Height)), handPieceHeight));
    }
}
