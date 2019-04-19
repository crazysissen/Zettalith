using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class PerkHUD : HUD
    {
        GUI.Collection collection;

        Renderer.SpriteScreen Background;

        GUI.Button bClose;

        Perk[] allPerks;

        public PerkHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            Background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 5), Load.Get<Texture2D>("Shop Background"), new Rectangle( 0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            int tempCloseButtonSize = 6;
            Texture2D bCloseTexture = Load.Get<Texture2D>("DeleteButton");
            bClose = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * 0.1f), (int)(Settings.GetResolution.Y * 0.1f), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Width), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Height)), bCloseTexture) { ScaleEffect = true };
            bClose.OnClick += csc.ClosePerks;

            Texture2D White = Load.Get<Texture2D>("White");





            // Denna delen är allt som är intressant för dig som ska göra perks.
            // Effekten är indexet i en lista av metoder som du finner i PerkBuffBonusEffects klassen.
            // Targeten är piecens index i listan av pieces. -1 är jag och -2 är motståndaren.

            allPerks = new Perk[]
            {
                new Perk("aPerk", "Sample text", 0.5f, 0.5f, 1, Load.Get<Texture2D>("Perk Tree Button"), 0, 0, 0, 0, 0, 0, csc) {Achieved = true },
                new Perk("anotherPerk", "Sample text", 0.5f, 0.3f, 1, Load.Get<Texture2D>("Buff Shop Button"), 1, -1, 3, 0, 4, 0, csc)
            };

            // Lägg till vilka perks din perk kan gå till och vilka perks som kan gå till den perk.

            allPerks[0].CreateConnections(new Perk[] { allPerks[1]});








            float difX, difY;
            int width, height;
            for (int i = 0; i < allPerks.Length; ++i)
            {
                if (allPerks[i].Connections != null && allPerks[i].Connections.Length != 0)
                {
                    for (int j = 0; j < allPerks[i].Connections.Length; ++j)
                    {
                        difX = allPerks[i].Connections[j].MyPerk.AccessButton.Transform.X - allPerks[i].Connections[j].MyPerk.AccessButton.Transform.Width * 0.5f - allPerks[i].AccessButton.Transform.X + allPerks[i].AccessButton.Transform.Width * 0.5f;
                        difY = allPerks[i].Connections[j].MyPerk.AccessButton.Transform.Y - allPerks[i].Connections[j].MyPerk.AccessButton.Transform.Height * 0.5f - allPerks[i].AccessButton.Transform.Y + allPerks[i].AccessButton.Transform.Height * 0.5f;
                        width = (int)(Settings.GetResolution.Y * 0.01);
                        height = (int)Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));

                        allPerks[i].Connections[j].MyLine = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 6), White, new Rectangle((int)(allPerks[i].AccessButton.Transform.X + allPerks[i].AccessButton.Transform.Width * 0.5 + difX * 0.5), (int)(allPerks[i].AccessButton.Transform.Y + allPerks[i].AccessButton.Transform.Height * 0.5 + difY * 0.5), width, height), Color.White, ((difX != 0) ? ((float)Math.Atan(difY / difX)) : (0f)), new Vector2(0.5f, 0.5f), SpriteEffects.None);
                        collection.Add(allPerks[i].Connections[j].MyLine);
                    }
                }
                collection.Add(allPerks[i].AccessButton);
            }

            collection.Add(Background, bClose);

            collection.Active = false;
        }

        public void Update(float deltaTime)
        {
            if (collection.Active)
            {
                for (int i = 0; i < allPerks.Length; ++i)
                {
                    if (allPerks[i].Achieved)
                    {
                        allPerks[i].AccessButton.SetPseudoDefaultColors(Color.Gold);

                        if (allPerks[i].Connections != null && allPerks[i].Connections.Length != 0)
                        {
                            for (int j = 0; j < allPerks[i].Connections.Length; ++j)
                            {
                                if (allPerks[i].Connections[j].MyPerk.Achieved)
                                {
                                    allPerks[i].Connections[j].MyLine.Color = Color.Gold;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class Perk
    {
        public bool Achieved { get; set; }
        public Connection[] Connections { get; set; }
        public GUI.Button AccessButton { get; set; }
        public string PerkName { get; set; }
        public string PerkDescription { get; set; }
        public int Effect { get; set; }
        public int Target { get; set; }
        public int RedCost { get; set; }
        public int GreenCost { get; set; }
        public int BlueCost { get; set; }
        public int EssenceCost { get; set; }
        ClientSideController theCSC;

        public Perk(string name, string description, float x, float y, float scale, Texture2D texture, int anEffect, int aTarget, int redManaCost, int greenManaCost, int blueManaCost, int essenceCost, ClientSideController csc)
        {
            theCSC = csc;
            PerkName = name;
            PerkDescription = PerkDescription;
            Achieved = false;
            AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * x), (int)(Settings.GetResolution.Y * y), (int)(Ztuff.SizeResFactor * texture.Bounds.Width * scale), (int)(Ztuff.SizeResFactor * texture.Bounds.Height * scale)), texture) { ScaleEffect = true };
            Effect = anEffect;
            Target = aTarget;
            RedCost = redManaCost;
            GreenCost = greenManaCost;
            BlueCost = blueManaCost;
            EssenceCost = essenceCost;
            AccessButton.OnClick += UsePerk;
        }

        public Perk() { Achieved = false; AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 6), new Rectangle()); PerkName = "Sample text"; PerkDescription = "Sample text"; }

        public void CreateConnections(Perk[] somePerks)
        {
            Connections = new Connection[somePerks.Length];
            for (int i = 0; i < somePerks.Length; ++i)
            {
                Connections[i] = new Connection();
                Connections[i].MyPerk = somePerks[i];
            }
        }

        void UsePerk()
        {
            if ( RedCost <= InGameController.LocalMana.Red && GreenCost <= InGameController.LocalMana.Green && BlueCost <= InGameController.LocalMana.Blue && EssenceCost <= InGameController.LocalEssence && Achieved == false)
            {
                //InGameController.LocalMana.Red -= RedCost;
                theCSC.MyEffectCache.AListOfSints.Add(new Sints(Effect, Target));
                Achieved = true;
            }
        }
    }

    class Connection
    {
        public Perk MyPerk { get; set; }
        public Renderer.SpriteScreen MyLine { get; set; }

        public Connection()
        {
            MyPerk = new Perk();
        }
    }
}
