using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Zettalith
{
    class Tutorial
    {
        MainController controller;

        GUI.Collection collection;

        GUI.Button bBack;

        Texture2D buttonTexture;

        Renderer.Text tutorialText;

        string theText;

        Action GoBack;

        RendererFocus focusTutorial;

        Layer tutorialLayer;

        public void Initialize(MainController controller, Action goBack, Layer useThisLayer)
        {
            tutorialLayer = useThisLayer;

            GoBack = goBack;

            this.controller = controller;

            focusTutorial = new RendererFocus(tutorialLayer);

            collection = new GUI.Collection();

            RendererController.GUI.Add(collection);

            buttonTexture = Load.Get<Texture2D>("Button1");

            bBack = new GUI.Button(tutorialLayer, new Rectangle((int)(Settings.GetResolution.X * 0.1), (int)(Settings.GetResolution.Y * 0.9f), (int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Width * 2), (int)(Ztuff.SizeResFactor * buttonTexture.Bounds.Height * 2)), buttonTexture) { ScaleEffect = true };
            bBack.AddText("Back", 3, true, Color.White, Font.Default);
            bBack.OnClick += BGoBack;

            theText = "Haha you gay";

            tutorialText = new Renderer.Text(tutorialLayer, Font.Default, theText, 3, 0, new Vector2(Settings.GetResolution.X * 0.1f, Settings.GetResolution.Y * 0.1f));

            collection.Add(bBack, tutorialText);
        }

        public void Update()
        {

        }

        private void BGoBack()
        {
            focusTutorial.Remove();
            collection.Active = false;
            GoBack.Invoke();
        }
    }
}
