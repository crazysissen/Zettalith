using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zettalith.Pieces;

namespace Zettalith
{
    class SetDesigner
    {
        MainController controller;

        GUI.Collection collections, collectionInspector, setDesigner;

        GUI.Button bCreate, bBack, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2;

        Renderer.SpriteScreen collectionInspectorLines, setDesignerLines, top, middle, bottom;

        List<Top> unlockedTopList;
        List<Middle> unlockedMiddleList;
        List<Bottom> unlockedBottomList;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collections = new GUI.Collection();
            collectionInspector = new GUI.Collection();
            setDesigner = new GUI.Collection() { Active = false };

            RendererController.GUI.Add(collections);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            unlockedTopList = Subpieces.GetSubpieces<Top>();
            unlockedMiddleList = Subpieces.GetSubpieces<Middle>();
            unlockedBottomList = Subpieces.GetSubpieces<Bottom>();

            CustomParameterCall[] arrowCalls = new CustomParameterCall[6];
            for (int i = 0; i < arrowCalls.Length; i++)
            {
                arrowCalls[i] = new CustomParameterCall() { TargetMethod = BArrow, SubPiece = (int)(i * 0.5), B1 = (i % 2 == 0)};
            }

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
            bArrowHead1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.17f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowHead1.OnClick += arrowCalls[0].Activate;

            bArrowHead2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.17f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowHead2.OnClick += arrowCalls[1].Activate;

            bArrowMiddle1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.47f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowMiddle1.OnClick += arrowCalls[2].Activate;

            bArrowMiddle2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.47f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowMiddle2.OnClick += arrowCalls[3].Activate;

            bArrowBottom1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.77f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowBottom1.OnClick += arrowCalls[4].Activate;

            bArrowBottom2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.77f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowBottom2.OnClick += arrowCalls[5].Activate;
            #endregion

            top = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedTopList[0].Texture, new Rectangle((int)(Settings.Resolution.X * 0.175f), (int)(Settings.Resolution.X * 0.17f), Settings.Resolution.X, Settings.Resolution.Y));
            middle = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedMiddleList[0].Texture, new Rectangle((int)(Settings.Resolution.X * 0.175f), (int)(Settings.Resolution.X * 0.47f), Settings.Resolution.X, Settings.Resolution.Y));
            bottom = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedBottomList[0].Texture, new Rectangle((int)(Settings.Resolution.X * 0.175f), (int)(Settings.Resolution.X * 0.77f), Settings.Resolution.X, Settings.Resolution.Y));

            setDesignerLines = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("SetDesignerLines"), new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            #endregion

            collectionInspector.Add(bCreate, bBack, collectionInspectorLines);
            setDesigner.Add(setDesignerLines, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2, top, middle, bottom);
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

        public void BArrow(int subPiece, bool b1)
        {

        }
    }

    struct CustomParameterCall
    {
        public Action<int, bool> TargetMethod;
        public int SubPiece;
        public bool B1;

        public void Activate()
        {
            TargetMethod.Invoke(SubPiece, B1);
        }
    }
}
