using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class InfoBox
    {
        const float
            DISTANCE = 0.02f,
            SCREENSIZE = 0.45f,
            CONTENTSIZE = 0.35f,
            IMAGESIZE = 0.16f;

        public bool IsOpen { get; private set; }

        GUI.Collection collection;

        Renderer.SpriteScreen background;
        Renderer.SpriteScreen[] subPieces;
        Renderer.Text abilityCost, movementCost;
        Renderer.Text[] titles, descriptions, hps, dmgs, hps2, dmgs2;
        Renderer.SpriteScreenFloating[] hpCs, dmgCs;

        TilePiece selectedPiece;

        Point
            total, halfTotal, size, halfSize;

        public InfoBox(GUI.Collection rootCollection)
        {
            total = (new Vector2(Settings.GetResolution.ToVector2().Y * SCREENSIZE * 1.4f, Settings.GetResolution.ToVector2().Y * SCREENSIZE)).RoundToPoint();
            halfTotal = new Point(total.X / 2, total.Y / 2);
            size = (new Vector2(Settings.GetResolution.ToVector2().Y * CONTENTSIZE * 1.4f, Settings.GetResolution.ToVector2().Y * CONTENTSIZE)).RoundToPoint();
            halfSize = new Point(size.X / 2, size.Y / 2);

            collection = new GUI.Collection() { Origin = Settings.GetHalfResolution - halfSize };
            rootCollection.Add(collection);

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -10), Load.Get<Texture2D>("InfoOverlay"), new Rectangle(halfSize - halfTotal, total), new Color(255, 255, 255, 180));

            titles = new Renderer.Text[3];
            descriptions = new Renderer.Text[3];
            hps = new Renderer.Text[3];
            dmgs = new Renderer.Text[3];
            hps2 = new Renderer.Text[3];
            dmgs2 = new Renderer.Text[3];

            subPieces = new Renderer.SpriteScreen[3];

            hpCs = new Renderer.SpriteScreenFloating[3];
            dmgCs = new Renderer.SpriteScreenFloating[3];

            float textInset = 0.34f;
            float totalYOffset = 0;

            Texture2D damageTexture = Load.Get<Texture2D>("DamageCrystal"), healthTexture = Load.Get<Texture2D>("HealthCrystal");

            for (int i = 0; i < 3; ++i)
            {
                float currentY = totalYOffset + i * (size.Y / 3);

                titles[i] = new Renderer.Text(new Layer(MainLayer.GUI, -9), Font.Bold, "Title: " + i, 1.6f, 0, new Vector2(textInset * size.X, currentY));
                descriptions[i] = new Renderer.Text(new Layer(MainLayer.GUI, -9), Font.Default, "Description: " + i, 1.6f, 0, new Vector2(textInset * size.X, currentY + size.Y * 0.07f));

                int textureSize = (int)(size.X * IMAGESIZE);
                subPieces[i] = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -9), Load.Get<Texture2D>("Square"), new Rectangle(size.X / 7, (int)currentY, textureSize, (int)(textureSize * ClientSideController.HEIGHTDISTANCE * 2)));

                float crystalInset = size.X * 0.05f, crystalOffset = size.Y * 0.05f;

                hpCs[i] = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -9), healthTexture, new Vector2(crystalInset, crystalOffset + currentY), Vector2.One * 1.6f * (Settings.GetResolution.Y / 720f), Color.White, 0, new Vector2(healthTexture.Width, healthTexture.Height) * 0.5f, SpriteEffects.None);
                dmgCs[i] = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, -9), damageTexture, new Vector2(crystalInset, crystalOffset + currentY + size.Y * 0.09f), Vector2.One * 1.6f * (Settings.GetResolution.Y / 720f), Color.White, 0, new Vector2(healthTexture.Width, healthTexture.Height) * 0.5f, SpriteEffects.None);

                float textSize = 1.2f * (Settings.GetResolution.Y / 720f), textOffset = -size.Y * 0.001f;

                hps[i] = new Renderer.Text(new Layer(MainLayer.GUI, -7), Font.Styled, i.ToString(), textSize, 0, new Vector2(crystalInset, textOffset + crystalOffset + currentY), Color.White);
                dmgs[i] = new Renderer.Text(new Layer(MainLayer.GUI, -7), Font.Styled, i.ToString(), textSize, 0, new Vector2(crystalInset, textOffset + crystalOffset + currentY + size.Y * 0.09f), Color.White);
                hps2[i] = new Renderer.Text(new Layer(MainLayer.GUI, -8), Font.Styled, i.ToString(), textSize, 0, new Vector2(crystalInset + size.X * 0.004f, textOffset + crystalOffset + currentY + size.Y * 0.004f), Color.Black);
                dmgs2[i] = new Renderer.Text(new Layer(MainLayer.GUI, -8), Font.Styled, i.ToString(), textSize, 0, new Vector2(crystalInset + size.X * 0.004f, textOffset + crystalOffset + currentY + size.Y * 0.09f + size.Y * 0.004f), Color.Black);

                if (i == 0)
                {
                    abilityCost = new Renderer.Text(new Layer(MainLayer.GUI, -9), Font.Bold, "Ability Cost: " + i, 1.6f, 0, new Vector2(textInset * size.X, currentY + size.Y * 0.14f));
                    collection.Add(abilityCost);
                }

                if (i == 2)
                {
                    movementCost = new Renderer.Text(new Layer(MainLayer.GUI, -9), Font.Bold, "Movement Cost: " + i, 1.6f, 0, new Vector2(textInset * size.X, currentY + size.Y * 0.14f));
                    collection.Add(movementCost);
                }

                collection.Add(titles[i], descriptions[i], subPieces[i], hpCs[i], dmgCs[i], hps[i], dmgs[i], hps2[i], dmgs2[i]);
            }

            SetTextOrigins();

            collection.Add(background);

            Close();
        }

        public void Update(float deltaTime)
        {
            if (selectedPiece != null)
            {
                collection.Origin = (RendererController.Camera.WorldToScreenPosition(selectedPiece.SupposedPosition - new Vector2(-0.5f, 1)) - new Vector2(total.X + Settings.GetResolution.Y * DISTANCE, halfSize.Y)).RoundToPoint();
            }
        }

        public void Set(TilePiece tilePiece)
        {
            selectedPiece = tilePiece;
            InGamePiece piece = tilePiece.Piece;

            SubPiece[] subPieces = { piece.Top, piece.Middle, piece.Bottom };

            Mana tempCost = piece.Top.AbilityCost - (tilePiece.Player == InGameController.PlayerIndex ? Ztuff.abilityCostDecrease : new Mana());

            int red = tempCost.Red, green = tempCost.Green, blue = tempCost.Blue;

            if (tempCost.Red < 0)
            {
                red = 0;
            }
            if (tempCost.Green < 0)
            {
                green = 0;
            }
            if (tempCost.Blue < 0)
            {
                blue = 0;
            }

            tempCost = new Mana(red, green, blue);

            abilityCost.String = new StringBuilder(tempCost.ToString());

            movementCost.String = new StringBuilder(piece.Bottom.MoveCost.ToString());

            for (int i = 0; i < 3; i++)
            {
                hps[i].String = new StringBuilder(subPieces[i].Health.ToString());
                dmgs[i].String = new StringBuilder(subPieces[i].AttackDamage.ToString());
                hps2[i].String = new StringBuilder(subPieces[i].Health.ToString());
                dmgs2[i].String = new StringBuilder(subPieces[i].AttackDamage.ToString());

                titles[i].String = new StringBuilder(subPieces[i].Name);
                descriptions[i].String = new StringBuilder(subPieces[i].Description);

                this.subPieces[i].Texture = subPieces[i].Texture;
            }

            SetTextOrigins();
        }

        public void Open()
        {
            IsOpen = false;

            collection.Active = true;
        }

        public void Close()
        {
            IsOpen = true;

            collection.Active = false;
        }

        void SetTextOrigins()
        {
            for (int i = 0; i < 3; i++)
            {
                hps[i].Origin = hps[i].Font.MeasureString(hps[i].String) * 0.5f;
                hps2[i].Origin = hps2[i].Font.MeasureString(hps2[i].String) * 0.5f;
                dmgs[i].Origin = dmgs[i].Font.MeasureString(dmgs[i].String) * 0.5f;
                dmgs2[i].Origin = dmgs2[i].Font.MeasureString(dmgs2[i].String) * 0.5f;
            }
        }
    }
}
