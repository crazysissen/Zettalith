using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class BuffHUD : HUD
    {
        GUI.Collection collection, mainCollection;

        GUI.Button bClose;

        Renderer.SpriteScreen background;

        Buff[] allBuffs;

        public BuffHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc, GUI.Collection aMainCollection) : base(igc, p, csc)
        {
            this.collection = collection;
            mainCollection = aMainCollection;

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 5), Load.Get<Texture2D>("Shop Background"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            int tempCloseButtonSize = 6;
            Texture2D bCloseTexture = Load.Get<Texture2D>("DeleteButton");
            bClose = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.1f), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Width), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Height)), bCloseTexture) { ScaleEffect = true };
            bClose.OnClick += csc.CloseBuffs;

            allBuffs = new Buff[]
            {
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection),
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection),
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection),
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection),
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection),
                new Buff("Buff name", "Sample text", 1, 1, 2, 3, 4, csc, mainCollection)
            };

            int row;
            int iThisRow;
            for (int i = 0; i < allBuffs.Length; ++i)
            {
                row = 0;
                iThisRow = i;
                while (iThisRow >= 4)
                {
                    row++;
                    iThisRow -= 4;
                }
                allBuffs[i].AccessButton.Transform = new Rectangle((int)(Settings.GetResolution.X * 0.1 + Settings.GetResolution.X * 0.2 * iThisRow), (int)(Settings.GetResolution.Y * 0.1 + Settings.GetResolution.Y * 0.2 * row), allBuffs[i].AccessButton.Transform.Width, allBuffs[i].AccessButton.Transform.Height);

                Renderer.Text name = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BuffName, 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.1f), Color.Black);
                Renderer.Text description = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BuffDescription, 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.3f), Color.Black);

                Renderer.Text redCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].RedCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Black);
                Renderer.Text greenCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].GreenCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.3f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Black);
                Renderer.Text blueCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BlueCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.5f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Black);
                Renderer.Text essenceCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].EssenceCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.7f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Black);

                Renderer.Text R = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "R", 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.15f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Red);
                Renderer.Text G = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "G", 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.35f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Green);
                Renderer.Text B = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "B", 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.55f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.Blue);
                Renderer.Text e = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "e", 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.75f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.2f), Color.LightBlue);


                collection.Add(allBuffs[i].AccessButton, name, description, redCost, greenCost, blueCost, essenceCost, R, G, B, e);
            }

            collection.Add(background, bClose);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {

        }
    }

    class Buff
    {
        public GUI.Button AccessButton { get; set; }
        public string BuffName { get; set; }
        public string BuffDescription { get; set; }
        public int Effect { get; set; }
        public int RedCost { get; set; }
        public int GreenCost { get; set; }
        public int BlueCost { get; set; }
        public int EssenceCost { get; set; }
        ClientSideController theCSC;
        GUI.Collection theCOL;

        public Buff(string name, string description, int effect, int costRedMana, int costGreenMana, int costBlueMana, int costEssence, ClientSideController csc, GUI.Collection col)
        {
            BuffName = name;
            BuffDescription = description;
            Effect = effect;
            RedCost = costRedMana;
            GreenCost = costGreenMana;
            BlueCost = costBlueMana;
            EssenceCost = costEssence;
            theCSC = csc;
            theCOL = col;
            Texture2D buttonTexture = Load.Get<Texture2D>("BuffButton");
            AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle(0, 0, (int)((int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Width) * 6f), (int)((int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Height) * 6f)), buttonTexture);
            AccessButton.OnClick += UseBuff;
        }

        void UseBuff()
        {
            if (RedCost <= InGameController.LocalMana.Red && GreenCost <= InGameController.LocalMana.Green && BlueCost <= InGameController.LocalMana.Blue && EssenceCost <= InGameController.LocalEssence)
            {
                InGameController.LocalMana -= new Mana(RedCost, BlueCost, GreenCost);
                InGameController.LocalEssence -= EssenceCost;

                theCOL.Active = false;


                //theCSC.MyEffectCache.AListOfSints.Add(new Sints(Effect, target));
            }
        }
    }
}
