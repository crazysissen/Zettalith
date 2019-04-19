using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class InGameHUD
    {
        public GUI.Collection Collection { get; private set; }

        public BattleHUD BattleHUD { get; private set; }
        public LogisticsHUD LogisticsHUD { get; private set; }
        public EndHUD EndHUD { get; private set; }
        public PerkHUD PerkHUD { get; private set; }
        public BuffHUD BuffHUD { get; private set; }
        public BonusHUD BonusHUD { get; private set; }

        GUI.Collection collection;

        public InGameHUD(GUI.Collection inGameCollection, GUI.Collection perkCollection, GUI.Collection buffCollection, GUI.Collection bonusCollection, GUI.Collection battleCollection, GUI.Collection logisticsCollection, GUI.Collection endCollection, InGameController igc, ClientSideController csc, PlayerLocal player)
        {
            Collection = inGameCollection;

            BattleHUD = new BattleHUD(battleCollection, igc, player, csc);
            LogisticsHUD = new LogisticsHUD(logisticsCollection, igc, player, csc);
            EndHUD = new EndHUD(endCollection, igc, player, csc);
            PerkHUD = new PerkHUD(perkCollection, igc, player, csc);
            BuffHUD = new BuffHUD(buffCollection, igc, player, csc);
            BonusHUD = new BonusHUD(bonusCollection, igc, player, csc);
        }

        public void Update(float deltaTime)
        {
            BattleHUD.Update(deltaTime);
            LogisticsHUD.Update(deltaTime);
            EndHUD.Update(deltaTime);
            PerkHUD.Update(deltaTime);
            BuffHUD.Update(deltaTime);
            BonusHUD.Update(deltaTime);
        }
    }

    class ManaBar
    {
        public Vector2 Position { get; set; }
        public Renderer.SpriteScreen Bottom { get; set; }
        public Renderer.SpriteScreen Top { get; set; }
        public Renderer.SpriteScreen ManaBlock { get; set; }
        public Renderer.Text ManaText { get; set; }
        Texture2D ManaBlockTexture;

        public ManaBar(Vector2 aPosition, int Colour)
        {
            Position = aPosition;

            Texture2D tempBottomTexture = Load.Get<Texture2D>("ManaBottom");
            Bottom = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 2), tempBottomTexture, new Rectangle((int)Position.X, (int)Position.Y - (int)(Ztuff.SizeResFactor * tempBottomTexture.Bounds.Height), (int)(Ztuff.SizeResFactor * tempBottomTexture.Bounds.Width), (int)(Ztuff.SizeResFactor * tempBottomTexture.Bounds.Height)));

            Texture2D tempTopTexture = Load.Get<Texture2D>("ManaTop");
            Top = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 2), tempTopTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)(Ztuff.SizeResFactor * tempTopTexture.Bounds.Width), (int)(Ztuff.SizeResFactor * tempTopTexture.Bounds.Height)));

            ManaBlockTexture = (Colour == 1 ? Load.Get<Texture2D>("Red mana") : (Colour == 2 ? Load.Get<Texture2D>("Green mana") : Load.Get<Texture2D>("Blue mana")));
            ManaBlock = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 2), ManaBlockTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)(Ztuff.SizeResFactor * ManaBlockTexture.Bounds.Width), (int)(Ztuff.SizeResFactor * ManaBlockTexture.Bounds.Height)));

            ManaText = new Renderer.Text(new Layer(MainLayer.GUI, 3), Font.Default, "0", 3, 0, new Vector2(), Color.Black);
            ManaText.Position = new Vector2(ManaBlock.Transform.X + ManaBlock.Transform.Width * 0.5f - ManaText.Font.MeasureString(ManaText.String).X * ManaText.Scale.X * 0.5f, Position.Y - Settings.GetResolution.Y * 0.02f);
        }

        public void Update(int Mana)
        {
            ManaBlock.Transform = new Rectangle((int)Position.X, Bottom.Transform.Y - (int)((int)(Ztuff.SizeResFactor * ManaBlockTexture.Bounds.Height) * Mana * 0.5f), (int)(Ztuff.SizeResFactor * ManaBlockTexture.Bounds.Width), (int)((int)(Ztuff.SizeResFactor * ManaBlockTexture.Bounds.Height) * Mana * 0.5f));

            Top.Transform = new Rectangle((int)Position.X, ManaBlock.Transform.Y - Top.Transform.Height, (int)(Ztuff.SizeResFactor * Top.Texture.Bounds.Width), (int)(Ztuff.SizeResFactor * Top.Texture.Bounds.Height));

            ManaText.String = new StringBuilder(Mana.ToString());
            ManaText.Position = new Vector2(ManaBlock.Transform.X + ManaBlock.Transform.Width * 0.5f - ManaText.Font.MeasureString(ManaText.String).X * ManaText.Scale.X * 0.5f, Position.Y - Settings.GetResolution.Y * 0.05f);
        }
    }
}
