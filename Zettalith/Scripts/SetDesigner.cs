using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SetDesigner
    {
        MainController controller;

        GUI.Collection collections, collectionInspector, setDesigner;

        GUI.Button bCreate, bBack, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2;

        Renderer.SpriteScreen collectionInspectorLines, setDesignerLines;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collections = new GUI.Collection();
            collectionInspector = new GUI.Collection();
            setDesigner = new GUI.Collection();

            setDesigner.Active = false;

            RendererController.GUI.Add(collections);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            #region //CollectionInspector
            bCreate = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.508f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bCreate.AddText("Create Deck", 4, true, textColor, Font.Default);
            bCreate.OnClick += BCreate;

            bBack = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.008f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bBack.AddText("Back", 4, true, textColor, Font.Default);
            bBack.OnClick += BBackToMain;

            collectionInspectorLines = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("CollectionInspectorLines"), new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            #endregion

            #region //SetDesigner
            bCancelSet = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.008f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bCancelSet.AddText("Cancel", 4, true, textColor, Font.Default);
            bCancelSet.OnClick += BCancelSet;

            #region //ArrowButtons
            bArrowHead1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.008f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bArrowHead1.OnClick += BArrow;

            bArrowHead2 = new GUI.Button(new Rectangle((int)(Settings.Resolution.X * 0.5f), (int)(Settings.Resolution.Y * 0.5f), (int)(Settings.Resolution.X * 1f), (int)(Settings.Resolution.Y * 1f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), new GUI.Button.Transition(), 0);
            #endregion

            setDesignerLines = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("SetDesignerLines"), new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            #endregion

            collectionInspector.Add(bCreate, bBack, collectionInspectorLines);
            setDesigner.Add(setDesignerLines, bCancelSet);
            collections.Add(collectionInspector, setDesigner);
        }

        public void Update(float deltaTime)
        {

        }

        private void BCreate()
        {
            collectionInspector.Active = false;
            setDesigner.Active = true;
        }

        private void BBackToMain()
        {
            collections.Active = false;
            controller.ToMenu();
        }

        private void BCancelSet()
        {
            setDesigner.Active = false;
            collectionInspector.Active = true;
        }

        private void BArrow()
        {

        }
    }
}
