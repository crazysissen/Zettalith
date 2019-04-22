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
        GUI.Collection collection, descriptionCollection;

        Renderer.SpriteScreen background, statsBackground;
        Renderer.Text statsName, statsDescription;
        Renderer.Text[] costs;

        GUI.Button bClose;

        Perk[] allPerks;

        public PerkHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 5), Load.Get<Texture2D>("Shop Background"), new Rectangle( 0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));
            statsBackground = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 8), Load.Get<Texture2D>("Description"), new Rectangle());
            statsName = new Renderer.Text(new Layer(MainLayer.GUI, 9), Font.Default, "", 2, 0, new Vector2());
            statsDescription = new Renderer.Text(new Layer(MainLayer.GUI, 9), Font.Default, "", 2, 0, new Vector2());
            descriptionCollection = new GUI.Collection();
            descriptionCollection.Add(statsBackground, statsName, statsDescription);
            costs = new Renderer.Text[8];
            for (int i = 0; i < costs.Length; ++i)
            {
                costs[i] = new Renderer.Text(new Layer(MainLayer.GUI, 9), Font.Default, "", 2, 0, new Vector2());
                descriptionCollection.Add(costs[i]);
            }
            descriptionCollection.Active = false;

            int tempCloseButtonSize = 6;
            Texture2D bCloseTexture = Load.Get<Texture2D>("DeleteButton");
            bClose = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.1f), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Width), tempCloseButtonSize * (int)(Ztuff.SizeResFactor * bCloseTexture.Bounds.Height)), bCloseTexture) { ScaleEffect = true };
            bClose.OnClick += csc.ClosePerks;

            Texture2D White = Load.Get<Texture2D>("White");





            // Denna delen är allt som är intressant för dig som ska göra perks.
            // Effekten är indexet i en lista av metoder som du finner i PerkBuffBonusEffects klassen.
            // Targeten är piecens index i listan av pieces. -1 är jag och -2 är motståndaren.

            allPerks = new Perk[]
            {
                new Perk("aPerk", "Sample text", 0.5f, 0.5f, 1, Load.Get<Texture2D>("Perk Tree Button"), 0, 0, new Mana(0, 0, 0), 0, csc, descriptionCollection) {Achieved = true , On = false, Achievable = true},
                new Perk("Buff cost decrease", "The cost of all buffs \nis decreased by 30%", 0.3f, 0.3f, 1, Load.Get<Texture2D>("Buff Shop Button"), 4, -1, new Mana(), 0, csc, descriptionCollection),
                new Perk("Ability cost decrease", "The cost of all \nabilities is \ndecreased by " + 100 * (1 - Ztuff.abilityCostFactor) + "%", 0.7f, 0.3f, 1, Load.Get<Texture2D>("Ability Perk"), 9, -1, new Mana(), 0, csc, descriptionCollection),
                new Perk("Health increased", "The health of all \nZettaliths in your \nhand and deck is \nincreased by 50%", 0.3f, 0.7f, 1, Load.Get<Texture2D>("Health Perk"), 10, -1, new Mana(), 0, csc, descriptionCollection),
                new Perk("Movement increased", "The movement speed \nof all Zettaliths in \nyour hand and deck is \nincreased by 30%", 0.7f, 0.7f, 1, Load.Get<Texture2D>("Move Perk"), 11, -1, new Mana(), 0, csc, descriptionCollection),
                new Perk("Essence income 1", "Essence income is \nincreased by 30%", 0.5f, 0.35f, 1, Load.Get<Texture2D>("Bonus Shop Button"), 12, -1, new Mana(), 0, csc, descriptionCollection),
                new Perk("Essence income 2", "Essence income is \nincreased by another \n30%", 0.5f, 0.2f, 1, Load.Get<Texture2D>("Bonus Shop Button"), 12, -1, new Mana(), 0, csc, descriptionCollection)
            };

            // Lägg till vilka perks din perk kan gå till och vilka perks som kan gå till den perk.

            allPerks[0].CreateConnections(new Perk[] { allPerks[1], allPerks[2], allPerks[3], allPerks[4], allPerks[5] });
            allPerks[5].CreateConnections(new Perk[] { allPerks[6] });








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

                        allPerks[i].Connections[j].MyLine = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 6), White, new Rectangle((int)(allPerks[i].AccessButton.Transform.X + allPerks[i].AccessButton.Transform.Width * 0.5 + difX * 0.5), (int)(allPerks[i].AccessButton.Transform.Y + allPerks[i].AccessButton.Transform.Height * 0.5 + difY * 0.5), width, height), Color.White, (float)(Math.Atan2(difY, difX) + Math.PI * 0.5f), new Vector2(0.5f, 0.5f), SpriteEffects.None);
                        collection.Add(allPerks[i].Connections[j].MyLine);
                    }
                }
                collection.Add(allPerks[i].AccessButton);
            }

            collection.Add(background, bClose, descriptionCollection);

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
                                allPerks[i].Connections[j].MyPerk.Achievable = true;

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
        public bool On { get; set; } = true;
        public bool Achievable { get; set; } = false;
        public bool Achieved { get; set; }
        public Connection[] Connections { get; set; }
        public GUI.Button AccessButton { get; set; }
        public string PerkName { get; set; }
        public string PerkDescription { get; set; }
        public int Effect { get; set; }
        public int Target { get; set; }
        public Mana Cost { get; set; }
        public int EssenceCost { get; set; }
        ClientSideController theCSC;
        GUI.Collection theDEC;

        public Perk(string name, string description, float x, float y, float scale, Texture2D texture, int anEffect, int aTarget, Mana aCost, int essenceCost, ClientSideController csc, GUI.Collection dec)
        {
            theCSC = csc;
            theDEC = dec;
            PerkName = name;
            PerkDescription = description;
            Achieved = false;
            AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * x), (int)(Settings.GetResolution.Y * y), (int)(Ztuff.SizeResFactor * texture.Bounds.Width * scale), (int)(Ztuff.SizeResFactor * texture.Bounds.Height * scale)), texture) { ScaleEffect = true };
            Effect = anEffect;
            Target = aTarget;
            Cost = aCost;
            EssenceCost = essenceCost;
            AccessButton.OnClick += UsePerk;
            AccessButton.OnClick += HideStats;
            AccessButton.OnEnter += ShowStats;
            AccessButton.OnExit += HideStats;
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
            if ( Cost <= InGameController.LocalMana && EssenceCost <= InGameController.LocalEssence && Achieved == false && On == true && Achievable == true)
            {
                InGameController.LocalMana -= Cost;
                InGameController.LocalEssence -= EssenceCost;
                theCSC.MyEffectCache.AListOfSints.Add(new Sints(Effect, Target));
                Achieved = true;
            }
        }

        void ShowStats()
        {
            if (On == true)
            {
                theDEC.Active = true;

                (theDEC.Members[0] as Renderer.SpriteScreen).Transform = new Rectangle(Input.MousePosition.X, Input.MousePosition.Y, (int)((int)(Ztuff.SizeResFactor * (theDEC.Members[0] as Renderer.SpriteScreen).Texture.Bounds.Width) * 7f), (int)((int)(Ztuff.SizeResFactor * (theDEC.Members[0] as Renderer.SpriteScreen).Texture.Bounds.Height) * 7f));

                (theDEC.Members[1] as Renderer.Text).String = new StringBuilder(PerkName);
                (theDEC.Members[1] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.5f - ((theDEC.Members[1] as Renderer.Text).Font.MeasureString((theDEC.Members[1] as Renderer.Text).String).X * (theDEC.Members[1] as Renderer.Text).Scale.X) * 0.5f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.06f);

                (theDEC.Members[2] as Renderer.Text).String = new StringBuilder(PerkDescription);
                (theDEC.Members[2] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.1f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.4f);

                (theDEC.Members[3] as Renderer.Text).String = new StringBuilder(Cost.Red.ToString());
                (theDEC.Members[4] as Renderer.Text).String = new StringBuilder("R");
                (theDEC.Members[5] as Renderer.Text).String = new StringBuilder(Cost.Green.ToString());
                (theDEC.Members[6] as Renderer.Text).String = new StringBuilder("G");
                (theDEC.Members[7] as Renderer.Text).String = new StringBuilder(Cost.Blue.ToString());
                (theDEC.Members[8] as Renderer.Text).String = new StringBuilder("B");
                (theDEC.Members[9] as Renderer.Text).String = new StringBuilder(EssenceCost.ToString());
                (theDEC.Members[10] as Renderer.Text).String = new StringBuilder("e");

                (theDEC.Members[4] as Renderer.Text).Color = Color.Red;
                (theDEC.Members[6] as Renderer.Text).Color = Color.Green;
                (theDEC.Members[8] as Renderer.Text).Color = Color.Blue;
                (theDEC.Members[10] as Renderer.Text).Color = Color.LightBlue;

                (theDEC.Members[3] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.2f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.2f);
                (theDEC.Members[4] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.3f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.2f);
                (theDEC.Members[5] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.6f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.2f);
                (theDEC.Members[6] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.7f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.2f);
                (theDEC.Members[7] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.2f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.3f);
                (theDEC.Members[8] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.3f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.3f);
                (theDEC.Members[9] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.6f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.3f);
                (theDEC.Members[10] as Renderer.Text).Position = new Vector2((theDEC.Members[0] as Renderer.SpriteScreen).Transform.X + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Width * 0.7f, (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Y + (theDEC.Members[0] as Renderer.SpriteScreen).Transform.Height * 0.3f);
            }
        }

        void HideStats()
        {
            theDEC.Active = false;
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
