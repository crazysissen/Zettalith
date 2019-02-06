using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class GameSetup
    {
        const int
            WINDOWWIDTH = 400,
            WINDOWHEIGHT = 500,
            WINDOWMARGIN = 20;

        readonly int[]
            presetSizes = { 8, 12, 16, 20 };

        public GUI.Collection Collection { get; private set; }

        private MainController controller;
        private InGameController inGameController;

        private GUI.Collection content;
        private Renderer.SpriteScreen background;
        private Renderer.Text title;
        private GUI.Button[] bSizes;
        private GUI.Button bConfirm;

        public void Initialize(MainController controller, InGameController inGameController)
        {
            this.controller = controller;
            this.inGameController = inGameController;

            Collection = new GUI.Collection()
            {
                Origin = Settings.HalfResolution - new Point(WINDOWWIDTH / 2, WINDOWHEIGHT / 2)
            };

            background = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 9), Load.Get<Texture2D>("Square"), new Rectangle(0, 0, WINDOWHEIGHT, WINDOWHEIGHT), Color.DarkGray);

            content = new GUI.Collection()
            {
                Origin = new Point(WINDOWMARGIN, WINDOWMARGIN)
            };

            int buttonHeight = 60, buttonDistance = 20;

            title = new Renderer.Text(new Layer(MainLayer.GUI, 11), Font.Bold, "Host Game", 6, 0, Vector2.Zero);

            int sizeButtonWidth = 100;
            bSizes = new GUI.Button[presetSizes.Length];
            for (int i = 0; i < bSizes.Length; ++i)
            {
                bSizes[i] = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(i * (sizeButtonWidth + 10), buttonHeight + buttonDistance, sizeButtonWidth, buttonHeight));
                bSizes[i].AddText(presetSizes[i].ToString(), 3, true, Color.Black, Font.Default);
            }

            bConfirm = new GUI.Button(new Layer(MainLayer.GUI, 11), new Rectangle(0, WINDOWHEIGHT - WINDOWMARGIN * 2 - buttonHeight, 200, buttonHeight), Color.LightYellow);
            bConfirm.AddText("Confirm", 4, false, Color.Black, Font.Bold);
            bConfirm.OnClick += Confirm;

            Collection.Add(content, background);
            content.Add(bSizes);
            content.Add(title, bConfirm);
        }

        public void Update(float deltaTime)
        {

        }

        private void Confirm()
        {

        }
    }
}
