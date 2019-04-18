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

            allPerks = new Perk[]
            {
                new Perk("aPerk", "Sample text", 0.5f, 0.5f, 1, Load.Get<Texture2D>("Perk Tree Button"), null) {On = false, Achieved = true },
                new Perk("anotherPerk", "Sample text", 0.5f, 0.3f, 1, Load.Get<Texture2D>("Buff Shop Button"), null)
            };

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
        public bool On { get; set; } = true;
        public bool Achieved { get; set; }
        public Connection[] Connections { get; set; }
        public GUI.Button AccessButton { get; set; }
        public string PerkName { get; set; }
        public string PerkDescription { get; set; }

        public Perk(string name, string description, float x, float y, float scale, Texture2D texture, Action effect)
        {
            PerkName = name;
            PerkDescription = PerkDescription;
            Achieved = false;
            AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 7), new Rectangle((int)(Settings.GetResolution.X * x), (int)(Settings.GetResolution.Y * y), (int)(Ztuff.SizeResFactor * texture.Bounds.Width * scale), (int)(Ztuff.SizeResFactor * texture.Bounds.Height * scale)), texture) { ScaleEffect = true };
            AccessButton.OnClick += UsePerk;
            AccessButton.OnClick += effect;
        }

        public Perk() { On = true; Achieved = false; AccessButton = new GUI.Button(new Layer(MainLayer.GUI, 6), new Rectangle()); PerkName = "Sample text"; PerkDescription = "Sample text"; }

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
            Achieved = true;
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
