﻿using System;
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

        List<Renderer.Text> costs;

        public BuffHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, Buff[] someBuffs, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 5), Load.Get<Texture2D>("Shop Background"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            int tempCloseButtonSize = 6;
            Texture2D bCloseTexture = Load.Get<Texture2D>("DeleteButton");
            bClose = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.1f), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Width), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Height)), bCloseTexture) { ScaleEffect = true };
            bClose.OnClick += csc.CloseBuffsAndBonus;

            costs = new List<Renderer.Text>();

            allBuffs = someBuffs;

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

                Renderer.Text name = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BuffName, 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.1f), Color.White);
                Renderer.Text description = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BuffDescription, 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.4f), Color.White);

                Renderer.Text redCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].RedCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.1f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.White);
                Renderer.Text greenCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].GreenCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.3f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.White);
                Renderer.Text blueCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].BlueCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.5f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.White);
                Renderer.Text essenceCost = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, allBuffs[i].EssenceCost.ToString(), 2, 0, new Vector2(allBuffs[i].AccessButton.Transform.X + allBuffs[i].AccessButton.Transform.Width * 0.7f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.White);

                if (allBuffs[i].RedCost == 0)
                {
                    redCost.Position = new Vector2(-100000, -100000);
                    greenCost.Position = new Vector2(greenCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, greenCost.Position.Y);
                    blueCost.Position = new Vector2(blueCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, blueCost.Position.Y);
                    essenceCost.Position = new Vector2(essenceCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, essenceCost.Position.Y);
                }
                if (allBuffs[i].GreenCost == 0)
                {
                    greenCost.Position = new Vector2(-100000, -100000);
                    blueCost.Position = new Vector2(blueCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, blueCost.Position.Y);
                    essenceCost.Position = new Vector2(essenceCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, essenceCost.Position.Y);
                }
                if (allBuffs[i].BlueCost == 0)
                {
                    blueCost.Position = new Vector2(-100000, -100000);
                    essenceCost.Position = new Vector2(essenceCost.Position.X - allBuffs[i].AccessButton.Transform.Width * 0.2f, essenceCost.Position.Y);
                }
                if (allBuffs[i].EssenceCost == 0)
                {
                    essenceCost.Position = new Vector2(-100000, -100000);
                }

                costs.Add(redCost);
                costs.Add(greenCost);
                costs.Add(blueCost);
                costs.Add(essenceCost);

                Renderer.Text R = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "R", 2, 0, new Vector2(redCost.Position.X + allBuffs[i].AccessButton.Transform.Width * 0.11f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.Red);
                Renderer.Text G = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "G", 2, 0, new Vector2(greenCost.Position.X + allBuffs[i].AccessButton.Transform.Width * 0.11f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.Green);
                Renderer.Text B = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "B", 2, 0, new Vector2(blueCost.Position.X + allBuffs[i].AccessButton.Transform.Width * 0.11f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.Blue);
                Renderer.Text e = new Renderer.Text(new Layer(MainLayer.GUI, 8), Font.Default, "e", 2, 0, new Vector2(essenceCost.Position.X + allBuffs[i].AccessButton.Transform.Width * 0.11f, allBuffs[i].AccessButton.Transform.Y + allBuffs[i].AccessButton.Transform.Height * 0.25f), Color.LightBlue);

                collection.Add(allBuffs[i].AccessButton, name, description, redCost, greenCost, blueCost, essenceCost, R, G, B, e);
            }

            collection.Add(background, bClose);
            collection.Active = false;
        }

        public void Update(float deltaTime)
        {
            if(Ztuff.changeBuffCost)
            {
                for (int i = 0; i < costs.Count; ++i)
                {
                    string tempString = ((int)(int.Parse(costs[i].String.ToString()) * Ztuff.buffCostFactor + 0.5f)).ToString();
                    costs[i].String = new StringBuilder(tempString);
                }
                Ztuff.changeBuffCost = false;
            }
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
        GUI.Collection theCOL;

        public Buff(string name, string description, int effect, int costRedMana, int costGreenMana, int costBlueMana, int costEssence, GUI.Collection col)
        {
            BuffName = name;
            BuffDescription = description;
            Effect = effect;
            RedCost = costRedMana;
            GreenCost = costGreenMana;
            BlueCost = costBlueMana;
            EssenceCost = costEssence;
            theCOL = col;
            Texture2D buttonTexture = Load.Get<Texture2D>("BuffButton");
            AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle(0, 0, (int)((int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Width) * 5f), (int)((int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Height) * 5f)), buttonTexture);
            AccessButton.OnClick += UseBuff;
        }

        void UseBuff()
        {
            if ((int)(RedCost * Ztuff.buffCostFactor + 0.5f) <= InGameController.LocalMana.Red && (int)(GreenCost * Ztuff.buffCostFactor + 0.5f) <= InGameController.LocalMana.Green && (int)(BlueCost * Ztuff.buffCostFactor + 0.5f) <= InGameController.LocalMana.Blue && (int)(EssenceCost * Ztuff.buffCostFactor + 0.5f) <= InGameController.LocalEssence)
            {
                theCOL.Active = false;
                Ztuff.RecieveGUI(theCOL);
                Ztuff.incomingEffect = Effect;
                Ztuff.buffCost = new Mana(RedCost, GreenCost, BlueCost);
                Ztuff.essenceCost = EssenceCost;
                Ztuff.pickingPiece = true;
            }
            else
            {
                if (EssenceCost > 0)
                {
                    InGameController.Local.ClientController.Alert("Not enough essence");
                }
                else
                {
                    InGameController.Local.ClientController.Alert("Not enough mana");
                }
            }
        }
    }
}
