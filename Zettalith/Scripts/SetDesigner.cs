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

        GUI.Collection collections, collectionInspector, setDesigner, topFullDesc, middleFullDesc, bottomFullDesc;

        GUI.Button bCreate, bBack, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2;

        Renderer.SpriteScreen collectionInspectorLines, setDesignerLines, top, middle, bottom;

        Renderer.Text topName, topHealth, topAttack, topMana, topDesc, middleName, middleHealth, middleAttack, middleMana, middleDesc, bottomName, bottomHealth, bottomAttack, bottomMana, bottomDesc;

        GUI.Button[] miniliths;

        List<Top> unlockedTopList;
        List<Middle> unlockedMiddleList;
        List<Bottom> unlockedBottomList;

        int currentlyShowingTop = 0, currentlyShowingMiddle = 0, currentlyShowingBottom = 0;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collections = new GUI.Collection();
            collectionInspector = new GUI.Collection();
            topFullDesc = new GUI.Collection() { Origin = new Point((int)(Settings.Resolution.X * 0f), (int)(Settings.Resolution.Y * -0.375f)) };
            middleFullDesc = new GUI.Collection() { Origin = new Point((int)(Settings.Resolution.X * 0f), (int)(Settings.Resolution.Y * -0.1f)) };
            bottomFullDesc = new GUI.Collection() { Origin = new Point((int)(Settings.Resolution.X * 0f), (int)(Settings.Resolution.Y * 0.15f)) };
            setDesigner = new GUI.Collection() { Active = false };

            RendererController.GUI.Add(collections);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            unlockedTopList = Subpieces.GetSubpieces<Top>();
            unlockedMiddleList = Subpieces.GetSubpieces<Middle>();
            unlockedBottomList = Subpieces.GetSubpieces<Bottom>();

            miniliths = new GUI.Button[20];

            CustomParameterCall[] arrowCalls = new CustomParameterCall[7];
            for (int i = 0; i < arrowCalls.Length; i++)
            {
                arrowCalls[i] = new CustomParameterCall() { TargetMethod = BArrow, SubPiece = (int)(i * 0.5 + 0.5), B1 = (i % 2 == 0) };
            }

            #region //CollectionInspector
            bCreate = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.523f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bCreate.AddText("Create Deck", 4, true, textColor, Font.Default);
            bCreate.OnClick += BCreate;

            bBack = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.023f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bBack.AddText("Back", 4, true, textColor, Font.Default);
            bBack.OnClick += BBackToMain;

            collectionInspectorLines = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("CollectionInspectorLines"), new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            #endregion

            #region //SetDesigner
            bCancelSet = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.023f), (int)(Settings.Resolution.Y * 0.9f), (int)(Settings.Resolution.X * 0.486f), (int)(Settings.Resolution.Y * 0.09f)));
            bCancelSet.AddText("Cancel", 4, true, textColor, Font.Default);
            bCancelSet.OnClick += BCancelSet;

            for (int i = 0; i < miniliths.Length; ++i)
            {
                //TODO
                //miniliths[i] = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.21f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            }

            #region //ArrowButtons
            bArrowHead1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.21f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowHead1.OnClick += arrowCalls[1].Activate;

            bArrowHead2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.21f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowHead2.OnClick += arrowCalls[2].Activate;

            bArrowMiddle1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.47f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowMiddle1.OnClick += arrowCalls[3].Activate;

            bArrowMiddle2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.47f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowMiddle2.OnClick += arrowCalls[4].Activate;

            bArrowBottom1 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.05f), (int)(Settings.Resolution.Y * 0.73f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f);
            bArrowBottom1.OnClick += arrowCalls[5].Activate;

            bArrowBottom2 = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle((int)(Settings.Resolution.X * 0.4f), (int)(Settings.Resolution.Y * 0.73f), (int)(Settings.Resolution.X * 0.03f), (int)(Settings.Resolution.Y * 0.04f)), Load.Get<Texture2D>("Arrow"), Load.Get<Texture2D>("ArrowHover"), Load.Get<Texture2D>("ArrowPressed"), GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowBottom2.OnClick += arrowCalls[6].Activate;
            #endregion

            top = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedTopList[currentlyShowingTop].Texture, new Rectangle((int)(Settings.Resolution.X * 0.20f), (int)(Settings.Resolution.Y * 0.14f), (int)(Settings.Resolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.Resolution.X * 0.1)));
            middle = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedMiddleList[currentlyShowingMiddle].Texture, new Rectangle((int)(Settings.Resolution.X * 0.20f), (int)(Settings.Resolution.Y * 0.40f), (int)(Settings.Resolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.Resolution.X * 0.1)));
            bottom = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), unlockedBottomList[currentlyShowingBottom].Texture, new Rectangle((int)(Settings.Resolution.X * 0.20f), (int)(Settings.Resolution.Y * 0.66f), (int)(Settings.Resolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.Resolution.X * 0.1)));

            #region //Descriptions
            topName = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topHealth = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Health: " + unlockedTopList[currentlyShowingTop].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topAttack = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Attack: " + unlockedTopList[currentlyShowingTop].AttackDamage.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topMana = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].ManaCost.Red + " red, " + unlockedTopList[currentlyShowingTop].ManaCost.Green + " green, " + unlockedTopList[currentlyShowingTop].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topDesc = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);

            middleName = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleHealth = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Health: " + unlockedMiddleList[currentlyShowingMiddle].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleAttack = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Attack: " + unlockedMiddleList[currentlyShowingMiddle].AttackDamage.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleMana = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].ManaCost.Red + " red, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Green + " green, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleDesc = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);

            bottomName = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomHealth = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Health: " + unlockedBottomList[currentlyShowingBottom].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomAttack = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder("Attack: " + unlockedBottomList[currentlyShowingBottom].ManaCost.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomMana = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].ManaCost.Red + " red, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Green + " green, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomDesc = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.Resolution.X * 0.51f), (int)(Settings.Resolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            #endregion

            setDesignerLines = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("SetDesignerLines"), new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            #endregion

            topFullDesc.Add(topName, topHealth, topAttack, topMana, topDesc);
            middleFullDesc.Add(middleName, middleHealth, middleAttack, middleMana, middleDesc);
            bottomFullDesc.Add(bottomName, bottomHealth, bottomAttack, bottomMana, bottomDesc);
            collectionInspector.Add(bCreate, bBack, collectionInspectorLines);
            setDesigner.Add(setDesignerLines, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2, top, middle, bottom, topFullDesc, middleFullDesc, bottomFullDesc);
            collections.Add(collectionInspector, setDesigner);
        }

        public void Update(float deltaTime)
        {

        }

        private void CreateSet()
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
            if(subPiece == 1)
            {
                if (!b1)
                {
                    if(currentlyShowingTop == unlockedTopList.Count - 1)
                        currentlyShowingTop = 0;
                    else
                        currentlyShowingTop++;
                }
                if (b1)
                {
                    if (currentlyShowingTop == 0)
                        currentlyShowingTop = unlockedTopList.Count - 1;
                    else
                        currentlyShowingTop--;
                }
                top.Texture = unlockedTopList[currentlyShowingTop].Texture;
                topName.String = new StringBuilder(unlockedTopList[currentlyShowingTop].Name);
                topHealth.String = new StringBuilder("Health: " + unlockedTopList[currentlyShowingTop].Health.ToString());
                topAttack.String = new StringBuilder("Attack: " + unlockedTopList[currentlyShowingTop].AttackDamage.ToString());
                topMana.String = new StringBuilder(unlockedTopList[currentlyShowingTop].ManaCost.Red + " red, " + unlockedTopList[currentlyShowingTop].ManaCost.Green + " green, " + unlockedTopList[currentlyShowingTop].ManaCost.Blue + " blue");
                topDesc.String = new StringBuilder(unlockedTopList[currentlyShowingTop].Description);
            }
            if (subPiece == 2)
            {
                if (!b1)
                {
                    if (currentlyShowingMiddle == unlockedMiddleList.Count - 1)
                        currentlyShowingMiddle = 0;
                    else
                        currentlyShowingMiddle++;
                }
                if (b1)
                {
                    if (currentlyShowingMiddle == 0)
                        currentlyShowingMiddle = unlockedMiddleList.Count - 1;
                    else
                        currentlyShowingMiddle--;
                }
                middle.Texture = unlockedMiddleList[currentlyShowingMiddle].Texture;
                middleName.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Name);
                middleHealth.String = new StringBuilder("Health: " + unlockedMiddleList[currentlyShowingMiddle].Health.ToString());
                middleAttack.String = new StringBuilder("Attack: " + unlockedMiddleList[currentlyShowingMiddle].AttackDamage.ToString());
                middleMana.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].ManaCost.Red + " red, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Green + " green, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Blue + " blue");
                middleDesc.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Description);
            }
            if (subPiece == 3)
            {
                if (!b1)
                {
                    if (currentlyShowingBottom == unlockedBottomList.Count - 1)
                        currentlyShowingBottom = 0;
                    else
                        currentlyShowingBottom++;
                }
                if (b1)
                {
                    if (currentlyShowingBottom == 0)
                        currentlyShowingBottom = unlockedBottomList.Count - 1;
                    else
                        currentlyShowingBottom--;
                }
                bottom.Texture = unlockedBottomList[currentlyShowingBottom].Texture;
                bottomName.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].Name);
                bottomHealth.String = new StringBuilder("Health: " + unlockedBottomList[currentlyShowingBottom].Health.ToString());
                bottomAttack.String = new StringBuilder("Attack: " + unlockedBottomList[currentlyShowingBottom].AttackDamage.ToString());
                bottomMana.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].ManaCost.Red + " red, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Green + " green, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Blue + " blue");
                bottomDesc.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].Description);
            }
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
