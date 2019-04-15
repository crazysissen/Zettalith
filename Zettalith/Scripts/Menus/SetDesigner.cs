using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zettalith.Pieces;

namespace Zettalith
{
    class SetDesigner
    {
        MainController controller;

        JankPiece[] newSet;

        GUI.Collection collections, collectionInspector, setDesigner, topFullDesc, middleFullDesc, bottomFullDesc;

        GUI.Button bCreate, bBack, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2, bNext, bDone, bCopy;

        Renderer.SpriteScreen collectionInspectorLines, setDesignerLines, topSubPiece, middleSubPiece, bottomSubPiece, highlight;

        Renderer.Text topName, topHealth, topAttack, topMana, topDesc, middleName, middleHealth, middleAttack, middleMana, middleDesc, bottomName, bottomHealth, bottomAttack, bottomMana, bottomDesc;

        GUI.Button[] miniliths;
        List<GUI.Button> setsButtons, deleteButtons;

        List<Top> unlockedTopList;
        List<Middle> unlockedMiddleList;
        List<Bottom> unlockedBottomList;

        Texture2D miniLith2D, arrow2D, arrowHover2D, arrowPressed2D, highlight2D, bDelete2D, setNamePlate2D;

        int currentlyShowingTop = 0, currentlyShowingMiddle = 0, currentlyShowingBottom = 0, selectedPiece = 0, setBeingModifiedIndex;

        bool moddingASet = false;

        Layer collectionLayer, designerLayer;

        RendererFocus focusCollection, focusDesigner;

        public void Initialize(MainController controller)
        {
            SaveLoad.Load();

            this.controller = controller;

            collections = new GUI.Collection();
            collectionInspector = new GUI.Collection();
            topFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * -0.375f)) };
            middleFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * -0.1f)) };
            bottomFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * 0.15f)) };
            setDesigner = new GUI.Collection() { Active = false };

            miniLith2D = Load.Get<Texture2D>("Minilith");
            arrow2D = Load.Get<Texture2D>("Arrow");
            arrowHover2D = Load.Get<Texture2D>("ArrowHover");
            arrowPressed2D = Load.Get<Texture2D>("ArrowPressed");
            highlight2D = Load.Get<Texture2D>("Highlighted");
            bDelete2D = Load.Get<Texture2D>("DeleteButton");
            setNamePlate2D = Load.Get<Texture2D>("SetNamePlate");

            RendererController.GUI.Add(collections);

            focusCollection = new RendererFocus(collectionLayer);
            focusDesigner = new RendererFocus(designerLayer) { Active = false };

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255), black = new Color(0, 0, 0, 255);

            unlockedTopList = Subpieces.GetSubpieces<Top>();
            unlockedMiddleList = Subpieces.GetSubpieces<Middle>();
            unlockedBottomList = Subpieces.GetSubpieces<Bottom>();

            collectionLayer = new Layer(MainLayer.GUI, 5);
            designerLayer = new Layer(MainLayer.GUI, 10);

            miniliths = new GUI.Button[Set.MaxSize];

            setsButtons = new List<GUI.Button>();
            deleteButtons = new List<GUI.Button>();

            CustomArrowCall[] arrowCalls = new CustomArrowCall[7];
            for (int i = 0; i < arrowCalls.Length; i++)
            {
                arrowCalls[i] = new CustomArrowCall() { TargetMethod = BArrow, SubPiece = (int)(i * 0.5 + 0.5), B1 = (i % 2 == 0) };
            }

            CustomSelectPieceCall[] selectPieceCalls = new CustomSelectPieceCall[Set.MaxSize];
            for (int i = 0; i < selectPieceCalls.Length; i++)
            {
                selectPieceCalls[i] = new CustomSelectPieceCall() { TargetMethod = BSelectPiece, PieceToSelect = i };
            }

            CustomModSetCall[] modSetCalls = new CustomModSetCall[PersonalData.UserData.SavedSets.Count()];
            for (int i = 0; i < modSetCalls.Length; i++)
            {
                modSetCalls[i] = new CustomModSetCall() { TargetMethod = BModSet, SetIndex = i, TheSet = PersonalData.UserData.SavedSets[i] };
            }

            CustomDeleteSetCall[] deleteCalls = new CustomDeleteSetCall[PersonalData.UserData.SavedSets.Count()];
            for (int i = 0; i < deleteCalls.Length; i++)
            {
                deleteCalls[i] = new CustomDeleteSetCall() { TargetMethod = BDeleteSet, Set = PersonalData.UserData.SavedSets[i] };
            }

            #region //CollectionInspector
            bCreate = new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.523f), (int)(Settings.GetResolution.Y * 0.9f), (int)(Settings.GetResolution.X * 0.486f), (int)(Settings.GetResolution.Y * 0.09f)));
            bCreate.AddText("Create Deck", 4, true, textColor, Font.Default);
            bCreate.OnClick += BCreateSet;

            bBack = new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.023f), (int)(Settings.GetResolution.Y * 0.9f), (int)(Settings.GetResolution.X * 0.486f), (int)(Settings.GetResolution.Y * 0.09f)));
            bBack.AddText("Back", 4, true, textColor, Font.Default);
            bBack.OnClick += BBackToMain;

            collectionInspectorLines = new Renderer.SpriteScreen(collectionLayer, Load.Get<Texture2D>("CollectionInspectorLines"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));

            int numOfSavedSets = PersonalData.UserData.SavedSets.Count(), curPosInRow = 0, curPosVert = 0;

            for (int i = 0; i < numOfSavedSets; i++)
            {
                if (curPosInRow == 6) { curPosInRow = 0; curPosVert++; }

                setsButtons.Add(new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.16 * (0.17 + curPosInRow)), (int)(Settings.GetResolution.Y * 0.1 * (0.15 + curPosVert)), (int)(Settings.GetResolution.X * 0.06 * 9 / 4), (int)(Settings.GetResolution.Y * 0.06)), setNamePlate2D));
                setsButtons[i].AddText(PersonalData.UserData.SavedSets[i].Name, 3, true, black, Font.Default);
                setsButtons[i].OnClick += modSetCalls[i].Activate;
                deleteButtons.Add(new GUI.Button(new Layer(MainLayer.GUI, collectionLayer.layer + 1), new Rectangle(setsButtons[i].Transform.Location.X + setsButtons[i].Transform.Width, setsButtons[i].Transform.Location.Y, (int)(Settings.GetResolution.X * 0.02 * 9 / 16), (int)(Settings.GetResolution.Y * 0.02)), bDelete2D));
                deleteButtons[i].OnClick += deleteCalls[i].Activate;
                collectionInspector.Add(setsButtons[i], deleteButtons[i]);

                curPosInRow++;
            }

            #endregion

            #region //SetDesigner
            bCancelSet = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.023f), (int)(Settings.GetResolution.Y * 0.9f), (int)(Settings.GetResolution.X * 0.486f), (int)(Settings.GetResolution.Y * 0.09f)));
            bCancelSet.AddText("Cancel", 4, true, textColor, Font.Default);
            bCancelSet.OnClick += BBackToMain;

            Texture2D tempMini = ImageProcessing.CombinePiece(unlockedTopList[0].Texture, unlockedMiddleList[0].Texture, unlockedBottomList[0].Texture);

            for (int i = 0; i < miniliths.Length; ++i)
            {
                miniliths[i] = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 1/(Set.MaxSize + 1) * (i + 1) - Settings.GetResolution.X * 0.015), (int)(Settings.GetResolution.Y * 0.02f), (int)(Settings.GetResolution.X * 0.00055 * tempMini.Width), (int)(Settings.GetResolution.Y * tempMini.Height * 0.00055 * 16f / 9f)), miniLith2D);
                miniliths[i].ScaleEffect = true;
                miniliths[i].OnClick += selectPieceCalls[i].Activate;
            }

            highlight = new Renderer.SpriteScreen(designerLayer, highlight2D, new Rectangle());
            UpdateHighlight();

            #region //ArrowButtons
            bArrowHead1 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.21f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f);
            bArrowHead1.OnClick += arrowCalls[1].Activate;

            bArrowHead2 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4f), (int)(Settings.GetResolution.Y * 0.21f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowHead2.OnClick += arrowCalls[2].Activate;

            bArrowMiddle1 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.47f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f);
            bArrowMiddle1.OnClick += arrowCalls[3].Activate;

            bArrowMiddle2 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4f), (int)(Settings.GetResolution.Y * 0.47f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowMiddle2.OnClick += arrowCalls[4].Activate;

            bArrowBottom1 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.05f), (int)(Settings.GetResolution.Y * 0.73f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f);
            bArrowBottom1.OnClick += arrowCalls[5].Activate;

            bArrowBottom2 = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4f), (int)(Settings.GetResolution.Y * 0.73f), (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D, GUI.Button.Transition.Switch, 0f)
            { SpriteEffects = SpriteEffects.FlipHorizontally };
            bArrowBottom2.OnClick += arrowCalls[6].Activate;
            #endregion

            topSubPiece = new Renderer.SpriteScreen(designerLayer, unlockedTopList[currentlyShowingTop].Texture, new Rectangle((int)(Settings.GetResolution.X * 0.20f), (int)(Settings.GetResolution.Y * 0.14f), (int)(Settings.GetResolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.GetResolution.Y * 0.177777777)));
            middleSubPiece = new Renderer.SpriteScreen(designerLayer, unlockedMiddleList[currentlyShowingMiddle].Texture, new Rectangle((int)(Settings.GetResolution.X * 0.20f), (int)(Settings.GetResolution.Y * 0.40f), (int)(Settings.GetResolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.GetResolution.Y * 0.177777777)));
            bottomSubPiece = new Renderer.SpriteScreen(designerLayer, unlockedBottomList[currentlyShowingBottom].Texture, new Rectangle((int)(Settings.GetResolution.X * 0.20f), (int)(Settings.GetResolution.Y * 0.66f), (int)(Settings.GetResolution.X * 0.1 / Math.Sqrt(2)), (int)(Settings.GetResolution.Y * 0.177777777)));

            bNext = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4), (int)(Settings.GetResolution.Y * 0.3), (int)(Settings.GetResolution.X * 0.05), (int)(Settings.GetResolution.Y * 0.05)), buttonColor);
            bNext.AddText("Next", 4, true, textColor, Font.Default);
            bNext.OnClick += BNextPiece;

            bCopy = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4), (int)(Settings.GetResolution.Y * 0.4), (int)(Settings.GetResolution.X * 0.05), (int)(Settings.GetResolution.Y * 0.05)), buttonColor);
            bCopy.AddText("Copy", 4, true, textColor, Font.Default);
            bCopy.OnClick += BCopyPiece;

            bDone = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.523f), (int)(Settings.GetResolution.Y * 0.9f), (int)(Settings.GetResolution.X * 0.486f), (int)(Settings.GetResolution.Y * 0.09f)), buttonColor);
            bDone.AddText("Done", 4, true, textColor, Font.Default);
            bDone.OnClick += BDone;

            #region //Descriptions
            topName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedTopList[currentlyShowingTop].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedTopList[currentlyShowingTop].AttackDamage.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topMana = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].ManaCost.Red + " red, " + unlockedTopList[currentlyShowingTop].ManaCost.Green + " green, " + unlockedTopList[currentlyShowingTop].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            topDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);

            middleName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedMiddleList[currentlyShowingMiddle].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedMiddleList[currentlyShowingMiddle].AttackDamage.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleMana = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].ManaCost.Red + " red, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Green + " green, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            middleDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);

            bottomName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Name), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedBottomList[currentlyShowingBottom].Health.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.546f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedBottomList[currentlyShowingBottom].AttackDamage.ToString()), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.582f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomMana = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].ManaCost.Red + " red, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Green + " green, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Blue + " blue"), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.618f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            bottomDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Description), new Vector2(1f, 1f), 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.654f)), new Vector2(0, 0), textColor, SpriteEffects.None);
            #endregion

            setDesignerLines = new Renderer.SpriteScreen(designerLayer, Load.Get<Texture2D>("SetDesignerLines"), new Rectangle(0, 0, Settings.GetResolution.X, Settings.GetResolution.Y));
            #endregion

            topFullDesc.Add(topName, topHealth, topAttack, topMana, topDesc);
            middleFullDesc.Add(middleName, middleHealth, middleAttack, middleMana, middleDesc);
            bottomFullDesc.Add(bottomName, bottomHealth, bottomAttack, bottomMana, bottomDesc);
            collectionInspector.Add(bCreate, bBack, collectionInspectorLines);
            setDesigner.Add(setDesignerLines, bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2, topSubPiece, middleSubPiece, bottomSubPiece, topFullDesc, middleFullDesc, bottomFullDesc, bNext, bDone, highlight, bCopy);
            for (int i = 0; i < miniliths.Length; ++i)
            {
                setDesigner.Add(miniliths[i]);
            }
            collections.Add(collectionInspector, setDesigner);
        }

        public void Update(float deltaTime)
        {

        }

        private void BDone()
        {
            Piece[] tempSet = new Piece[newSet.Length];

            for (int i = 0; i < newSet.Count(); i++)
            {
                tempSet[i] = new Piece((byte)(Subpieces.SubPieces.IndexOf(unlockedTopList[newSet[i].TopIndex].GetType())), (byte)(Subpieces.SubPieces.IndexOf(unlockedMiddleList[newSet[i].MiddleIndex].GetType())), (byte)(Subpieces.SubPieces.IndexOf(unlockedBottomList[newSet[i].BottomIndex].GetType())));
            }

            if (moddingASet)
            {
                PersonalData.UserData.SavedSets[setBeingModifiedIndex].Pieces = tempSet.ToList();
            }
            else
            {
                PersonalData.UserData.SavedSets.Add(new Set() { Pieces = tempSet.ToList(), Name = "Set " + (PersonalData.UserData.SavedSets.Count + 1) });
            }

            SaveLoad.Save();

            BBackToMain();
        }

        private void BCreateSet()
        {
            if (PersonalData.UserData.SavedSets.Count() < 54)
            {
                collectionInspector.Active = false;
                setDesigner.Active = true;
                focusCollection.Active = false;
                focusDesigner.Active = true;
                newSet = new JankPiece[Set.MaxSize];
                for (int i = 0; i < newSet.Length; ++i)
                {
                    newSet[i] = new JankPiece(0, 0, 0);
                }
                selectedPiece = 0;
                currentlyShowingTop = newSet[selectedPiece].TopIndex;
                currentlyShowingMiddle = newSet[selectedPiece].MiddleIndex;
                currentlyShowingBottom = newSet[selectedPiece].BottomIndex;

                for (int i = 0; i < Set.MaxSize; i++)
                {
                    miniliths[i].Texture = ImageProcessing.CombinePiece(unlockedTopList[newSet[i].TopIndex].Texture, unlockedMiddleList[newSet[i].MiddleIndex].Texture, unlockedBottomList[newSet[i].BottomIndex].Texture);
                }

                UpdateShownTop();
                UpdateShownMiddle();
                UpdateShownBottom();
            }
        }

        private void BModSet(Set set, int setIndex)
        {
            moddingASet = true;
            setBeingModifiedIndex = setIndex;

            collectionInspector.Active = false;
            setDesigner.Active = true;
            focusCollection.Active = false;
            focusDesigner.Active = true;

            newSet = new JankPiece[Set.MaxSize];

            for (int i = 0; i < newSet.Length; ++i)
            {
                for (int j = 0; j < unlockedTopList.Count(); j++)
                {
                    if (unlockedTopList[j].GetType() == Subpieces.SubPieces[set.Pieces[i].TopIndex])
                    {
                        newSet[i].TopIndex = j;
                    }
                }

                for (int j = 0; j < unlockedMiddleList.Count(); j++)
                {
                    if (unlockedMiddleList[j].GetType() == Subpieces.SubPieces[set.Pieces[i].MiddleIndex])
                    {
                        newSet[i].MiddleIndex = j;
                    }
                }

                for (int j = 0; j < unlockedBottomList.Count(); j++)
                {
                    if (unlockedBottomList[j].GetType() == Subpieces.SubPieces[set.Pieces[i].BottomIndex])
                    {
                        newSet[i].BottomIndex = j;
                    }
                }
            }

            selectedPiece = 0;
            currentlyShowingTop = newSet[selectedPiece].TopIndex;
            currentlyShowingMiddle = newSet[selectedPiece].MiddleIndex;
            currentlyShowingBottom = newSet[selectedPiece].BottomIndex;

            for (int i = 0; i < Set.MaxSize; i++)
            {
                miniliths[i].Texture = ImageProcessing.CombinePiece(unlockedTopList[newSet[i].TopIndex].Texture, unlockedMiddleList[newSet[i].MiddleIndex].Texture, unlockedBottomList[newSet[i].BottomIndex].Texture);
            }

            UpdateShownTop();
            UpdateShownMiddle();
            UpdateShownBottom();
        }

        private void BDeleteSet(Set set)
        {
            PersonalData.UserData.SavedSets.Remove(set);
            SaveLoad.Save();
            BBackToMain();
        }

        private void BNextPiece()
        {
            if (selectedPiece == Set.MaxSize - 1)
                selectedPiece = 0;
            else
                selectedPiece++;

            ChangeShownPiece();
        }

        private void BCopyPiece()
        {
            if (selectedPiece == Set.MaxSize - 1)
            {
                newSet[0].TopIndex = newSet[selectedPiece].TopIndex;
                newSet[0].MiddleIndex = newSet[selectedPiece].MiddleIndex;
                newSet[0].BottomIndex = newSet[selectedPiece].BottomIndex;
                selectedPiece = 0;
            }
            else
            {
                newSet[selectedPiece + 1].TopIndex = newSet[selectedPiece].TopIndex;
                newSet[selectedPiece + 1].MiddleIndex = newSet[selectedPiece].MiddleIndex;
                newSet[selectedPiece + 1].BottomIndex = newSet[selectedPiece].BottomIndex;
                selectedPiece++;
            }

            ChangeShownPiece();
        }

        private void BSelectPiece(int pieceToSelect)
        {
            selectedPiece = pieceToSelect;

            ChangeShownPiece();
        }

        private void ChangeShownPiece()
        {
            currentlyShowingTop = newSet[selectedPiece].TopIndex;
            currentlyShowingMiddle = newSet[selectedPiece].MiddleIndex;
            currentlyShowingBottom = newSet[selectedPiece].BottomIndex;

            UpdateHighlight();

            UpdateShownTop();
            UpdateShownMiddle();
            UpdateShownBottom();
        }

        private void BBackToMain()
        {
            collections.Active = false;
            controller.ToMenu();
        }

        public void BArrow(int subPiece, bool b1)
        {
            if(subPiece == 1)
            {
                if (!b1)
                {
                    if(currentlyShowingTop == 0)
                        currentlyShowingTop = unlockedTopList.Count - 1;
                    else
                        currentlyShowingTop--;
                }
                if (b1)
                {
                    if (currentlyShowingTop == unlockedTopList.Count - 1)
                        currentlyShowingTop = 0;
                    else
                        currentlyShowingTop++;
                }
                UpdateShownTop();
            }
            if (subPiece == 2)
            {
                if (!b1)
                {
                    if (currentlyShowingMiddle == 0)
                        currentlyShowingMiddle = unlockedMiddleList.Count - 1;
                    else
                        currentlyShowingMiddle--;
                }
                if (b1)
                {
                    if (currentlyShowingMiddle == unlockedMiddleList.Count - 1)
                        currentlyShowingMiddle = 0;
                    else
                        currentlyShowingMiddle++;
                }
                UpdateShownMiddle();
            }
            if (subPiece == 3)
            {
                if (!b1)
                {
                    if (currentlyShowingBottom == 0)
                        currentlyShowingBottom = unlockedBottomList.Count - 1;
                    else
                        currentlyShowingBottom--;
                }
                if (b1)
                {
                    if (currentlyShowingBottom == unlockedBottomList.Count - 1)
                        currentlyShowingBottom = 0;
                    else
                        currentlyShowingBottom++;
                }
                UpdateShownBottom();
            }
        }

        private void UpdateShownTop()
        {
            topSubPiece.Texture = unlockedTopList[currentlyShowingTop].Texture;
            topName.String = new StringBuilder(unlockedTopList[currentlyShowingTop].Name);
            topHealth.String = new StringBuilder("Health: " + unlockedTopList[currentlyShowingTop].Health.ToString());
            topAttack.String = new StringBuilder("Attack: " + unlockedTopList[currentlyShowingTop].AttackDamage.ToString());
            topMana.String = new StringBuilder(unlockedTopList[currentlyShowingTop].ManaCost.Red + " red, " + unlockedTopList[currentlyShowingTop].ManaCost.Blue + " blue, " + unlockedTopList[currentlyShowingTop].ManaCost.Green + " green");
            topDesc.String = new StringBuilder(unlockedTopList[currentlyShowingTop].Description);

            newSet[selectedPiece].TopIndex = (byte)currentlyShowingTop;

            UpdateMinilith();
        }

        private void UpdateShownMiddle()
        {
            middleSubPiece.Texture = unlockedMiddleList[currentlyShowingMiddle].Texture;
            middleName.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Name);
            middleHealth.String = new StringBuilder("Health: " + unlockedMiddleList[currentlyShowingMiddle].Health.ToString());
            middleAttack.String = new StringBuilder("Attack: " + unlockedMiddleList[currentlyShowingMiddle].AttackDamage.ToString());
            middleMana.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].ManaCost.Red + " red, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Blue + " blue, " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.Green + " green");
            middleDesc.String = new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Description);
            newSet[selectedPiece].MiddleIndex = (byte)(currentlyShowingMiddle);

            UpdateMinilith();
        }

        private void UpdateShownBottom()
        {
            bottomSubPiece.Texture = unlockedBottomList[currentlyShowingBottom].Texture;
            bottomName.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].Name);
            bottomHealth.String = new StringBuilder("Health: " + unlockedBottomList[currentlyShowingBottom].Health.ToString());
            bottomAttack.String = new StringBuilder("Attack: " + unlockedBottomList[currentlyShowingBottom].AttackDamage.ToString());
            bottomMana.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].ManaCost.Red + " red, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Blue + " blue, " + unlockedBottomList[currentlyShowingBottom].ManaCost.Green + " green");
            bottomDesc.String = new StringBuilder(unlockedBottomList[currentlyShowingBottom].Description);
            newSet[selectedPiece].BottomIndex = (byte)(currentlyShowingBottom);

            UpdateMinilith();
        }

        private void UpdateMinilith()
        {
            miniliths[selectedPiece].Texture = ImageProcessing.CombinePiece(unlockedTopList[newSet[selectedPiece].TopIndex].Texture, unlockedMiddleList[newSet[selectedPiece].MiddleIndex].Texture, unlockedBottomList[newSet[selectedPiece].BottomIndex].Texture);
        }

        private void UpdateHighlight()
        {
            highlight.Transform = new Rectangle((int)(miniliths[selectedPiece].Transform.X - Settings.GetResolution.X * 0.022), (int)(miniliths[selectedPiece].Transform.Y - Settings.GetResolution.Y * 0.01), (int)(Settings.GetResolution.X * 0.06), (int)(Settings.GetResolution.Y * 0.09));
        }
    }

    struct CustomArrowCall
    {
        public Action<int, bool> TargetMethod;
        public int SubPiece;
        public bool B1;

        public void Activate()
        {
            TargetMethod.Invoke(SubPiece, B1);
        }
    }

    struct CustomSelectPieceCall
    {
        public Action<int> TargetMethod;
        public int PieceToSelect;

        public void Activate()
        {
            TargetMethod.Invoke(PieceToSelect);
        }
    }

    struct CustomModSetCall
    {
        public Action<Set, int> TargetMethod;
        public Set TheSet;
        public int SetIndex;

        public void Activate()
        {
            TargetMethod.Invoke(TheSet, SetIndex);
        }
    }

    struct CustomDeleteSetCall
    {
        public Action<Set> TargetMethod;
        public Set Set;

        public void Activate()
        {
            TargetMethod.Invoke(Set);
        }
    }

    struct JankPiece
    {
        public int TopIndex;
        public int MiddleIndex;
        public int BottomIndex;

        public JankPiece (int topIndex, int middleIndex, int bottomIndex)
        {
            TopIndex = topIndex;
            MiddleIndex = middleIndex;
            BottomIndex = bottomIndex;
        }
    }
}
