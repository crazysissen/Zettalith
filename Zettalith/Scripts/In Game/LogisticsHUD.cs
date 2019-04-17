using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class LogisticsHUD : HUD
    {
        GUI.Collection collection;

        int handPieceHeight;
        List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)> handPieces;

        Renderer.SpriteScreen panels;
        GUI.Button bPerks, bBuffs, bBonuses;

        Point handStart, handEnd;

        public LogisticsHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            handPieces = new List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)>();
            handPieceHeight = Settings.GetResolution.Y / 5;

            Vector2 ButtonSizeResFactor = new Vector2(Settings.GetResolution.X / 1920f, Settings.GetResolution.Y / 1080f);

            panels = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("HUD Logistics"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            Texture2D PerkButtonTexture = Load.Get<Texture2D>("Perk Tree Button");
            bPerks = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.4f), (int)(Settings.GetResolution.Y * 0.008f), (int)(ButtonSizeResFactor.X * PerkButtonTexture.Bounds.Width), (int)(ButtonSizeResFactor.Y * PerkButtonTexture.Bounds.Height)), PerkButtonTexture) { ScaleEffect = true };

            bBuffs = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.5f), (int)(Settings.GetResolution.Y * 0.008f), (int)(ButtonSizeResFactor.X * PerkButtonTexture.Bounds.Width), (int)(ButtonSizeResFactor.Y * PerkButtonTexture.Bounds.Height)), Load.Get<Texture2D>("Buff Shop Button")) { ScaleEffect = true };

            bBonuses = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.6f), (int)(Settings.GetResolution.Y * 0.008f), (int)(ButtonSizeResFactor.X * PerkButtonTexture.Bounds.Width), (int)(ButtonSizeResFactor.Y * PerkButtonTexture.Bounds.Height)), Load.Get<Texture2D>("Bonus Shop Button")) { ScaleEffect = true };



            handStart = new Point((int)(Settings.GetResolution.X * 0.13f), (int)(Settings.GetResolution.Y * 0.77f));
            handEnd = new Point((int)(Settings.GetResolution.X * 0.6f), (int)(Settings.GetResolution.Y * 0.77f));

            GUI.Button button = new GUI.Button(Layer.GUI, new Rectangle(10, 10, 160, 60));
            button.AddText("Draw Piece", 3, true, Color.Black, Font.Bold);
            button.OnClick += clientSideController.DrawPieceFromDeck;

            collection.Add(panels, bPerks, bBuffs, bBonuses, button);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {

        }
    }
}