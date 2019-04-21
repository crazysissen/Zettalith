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
        Renderer.Text essence;
        GUI.Button bPerks, bBuffs, bBonuses;

        ManaBar[] Bars;

        public LogisticsHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            handPieces = new List<(Renderer.SpriteScreen renderer, InGamePiece piece, RendererFocus focus)>();
            handPieceHeight = Settings.GetResolution.Y / 5;

            panels = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("HUD Logistics"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            Texture2D PerkButtonTexture = Load.Get<Texture2D>("Perk Tree Button");
            bPerks = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.45f), (int)(Settings.GetResolution.Y * 0.005f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Width)) * 1.5f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Height)) * 1.5f)), PerkButtonTexture) { ScaleEffect = true };
            bPerks.OnClick += csc.OpenPerks;

            bBuffs = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.55f), (int)(Settings.GetResolution.Y * 0.005f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Width)) * 1.5f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Height)) * 1.5f)), Load.Get<Texture2D>("Buff Shop Button")) { ScaleEffect = true };
            bBuffs.OnClick += csc.OpenBuff;

            bBonuses = new GUI.Button(new Layer(MainLayer.GUI, 2), new Rectangle((int)(Settings.GetResolution.X * 0.65f), (int)(Settings.GetResolution.Y * 0.005f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Width)) * 1.5f), (int)(((int)(Ztuff.SizeResFactor * PerkButtonTexture.Bounds.Height)) * 1.5f)), Load.Get<Texture2D>("Bonus Shop Button")) { ScaleEffect = true };
            bBonuses.OnClick += csc.OpenBonus;

            essence = new Renderer.Text(new Layer(MainLayer.GUI, 2), Font.Italic, InGameController.LocalEssence + "e", 5, 0, new Vector2(Settings.GetResolution.X * 0.83f, Settings.GetResolution.Y * 0.006f), new Vector2(), Color.Blue);
            essence.Position = new Vector2(essence.Position.X - essence.Font.MeasureString(essence.String).X * essence.Scale.X, essence.Position.Y);

            Bars = new ManaBar[3];
            for (int i = 0; i < Bars.Length; ++i)
            {
                Bars[i] = new ManaBar(new Vector2(Settings.GetResolution.X * (0.71f + 0.05f * i), Settings.GetResolution.Y), i + 1);
                collection.Add(Bars[i].Top, Bars[i].Bottom, Bars[i].ManaText, Bars[i].ManaBlock);
            }

            //GUI.Button button = new GUI.Button(Layer.GUI, new Rectangle(10, 10, 160, 60));
            //button.AddText("Draw Piece", 3, true, Color.Black, Font.Bold);
            //button.OnClick += clientSideController.DrawPieceFromDeck;

            collection.Add(panels, bPerks, bBuffs, bBonuses, /*button,*/ essence);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {
            essence.String = new StringBuilder(InGameController.LocalEssence + "e");
            essence.Position = new Vector2(Settings.GetResolution.X * 0.83f - essence.Font.MeasureString(essence.String).X * essence.Scale.X, essence.Position.Y);

            Bars[0].Update(InGameController.LocalMana.Red);
            Bars[1].Update(InGameController.LocalMana.Green);
            Bars[2].Update(InGameController.LocalMana.Blue);
        }
    }
}