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

        Renderer.SpriteScreen topSubPiece, middleSubPiece, bottomSubPiece, highlight;

        Renderer.Text topName, topHealth, topAttack, topAbilityRange, topCost, topAbilityCost, topDesc, middleName, middleHealth, middleAttack, middleArmor, middleCost, middleDesc, bottomName, bottomHealth, bottomAttack, bottomMovementRange, bottomCost, bottomMovementCost, bottomDesc;

        GUI.TextField nameField;

        GUI.Button[] miniliths;
        List<GUI.Button> setsButtons, deleteButtons;

        List<Top> unlockedTopList;
        List<Middle> unlockedMiddleList;
        List<Bottom> unlockedBottomList;

        Texture2D miniLith2D, arrow2D, arrowHover2D, arrowPressed2D, highlight2D, bDelete2D, setNamePlate2D, button2D;

        int currentlyShowingTop = 0, currentlyShowingMiddle = 0, currentlyShowingBottom = 0, selectedPiece = 0, setBeingModifiedIndex;

        float descriptionSpace = 0.033f * Settings.GetResolution.Y;

        bool moddingASet = false;

        Layer collectionLayer, designerLayer;

        RendererFocus focusCollection, focusDesigner;

        public void Initialize(MainController controller)
        {
            SaveLoad.Load();

            this.controller = controller;

            collections = new GUI.Collection();
            collectionInspector = new GUI.Collection();
            topFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * -0.405f)) };
            middleFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * -0.13f)) };
            bottomFullDesc = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * 0.115f)) };
            setDesigner = new GUI.Collection() { Active = false };

            miniLith2D = Load.Get<Texture2D>("Minilith");
            arrow2D = Load.Get<Texture2D>("Arrow");
            arrowHover2D = Load.Get<Texture2D>("ArrowHover");
            arrowPressed2D = Load.Get<Texture2D>("ArrowPressed");
            highlight2D = Load.Get<Texture2D>("Highlighted");
            bDelete2D = Load.Get<Texture2D>("DeleteButton");
            setNamePlate2D = Load.Get<Texture2D>("SetNamePlate");
            button2D = Load.Get<Texture2D>("Button1");

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
            bCreate = new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.795f), (int)(Settings.GetResolution.Y * 0.877f), (int)(Ztuff.SizeResFactor * button2D.Width) * 5, (int)(Ztuff.SizeResFactor * button2D.Height) * 5), button2D);
            bCreate.AddText("Create Deck", 4, true, Color.White, Font.Default);
            bCreate.OnClick += BCreateSet;

            bBack = new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.01f), (int)(Settings.GetResolution.Y * 0.877f), (int)(Ztuff.SizeResFactor * button2D.Width) * 5, (int)(Ztuff.SizeResFactor * button2D.Height) * 5), button2D);
            bBack.AddText("Back", 4, true, Color.White, Font.Default);
            bBack.OnClick += BBackToMain;
            

            int numOfSavedSets = PersonalData.UserData.SavedSets.Count(), curPosInRow = 0, curPosVert = 0;

            for (int i = 0; i < numOfSavedSets; i++)
            {
                if (curPosInRow == 6) { curPosInRow = 0; curPosVert++; }

                setsButtons.Add(new GUI.Button(collectionLayer, new Rectangle((int)(Settings.GetResolution.X * 0.16 * (0.17 + curPosInRow)), (int)(Settings.GetResolution.Y * 0.03 + Settings.GetResolution.Y * 0.1 * (0.15 + curPosVert)), (int)(Ztuff.SizeResFactor * button2D.Width) * 3, (int)(Ztuff.SizeResFactor * button2D.Height) * 3), button2D));
                setsButtons[i].AddText(PersonalData.UserData.SavedSets[i].Name, 3, true, Color.White, Font.Default);
                setsButtons[i].OnClick += modSetCalls[i].Activate;
                deleteButtons.Add(new GUI.Button(new Layer(MainLayer.GUI, collectionLayer.layer + 1), new Rectangle(setsButtons[i].Transform.Location.X + setsButtons[i].Transform.Width, setsButtons[i].Transform.Location.Y, (int)(Settings.GetResolution.X * 0.02 * 9 / 16), (int)(Settings.GetResolution.Y * 0.02)), bDelete2D));
                deleteButtons[i].OnClick += deleteCalls[i].Activate;
                collectionInspector.Add(setsButtons[i], deleteButtons[i]);

                curPosInRow++;
            }

            #endregion

            #region //SetDesigner
            bCancelSet = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.01f), (int)(Settings.GetResolution.Y * 0.877f), (int)(Ztuff.SizeResFactor * button2D.Width) * 5, (int)(Ztuff.SizeResFactor * button2D.Height) * 5), button2D);
            bCancelSet.AddText("Cancel", 4, true, Color.White, Font.Default);
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

            bNext = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4), (int)(Settings.GetResolution.Y * 0.3), (int)(Ztuff.SizeResFactor * button2D.Width) * 2, (int)(Ztuff.SizeResFactor * button2D.Height) * 2), button2D);
            bNext.AddText("Next", 3, true, Color.White, Font.Default);
            bNext.OnClick += BNextPiece;

            bCopy = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.4), (int)(Settings.GetResolution.Y * 0.37), (int)(Ztuff.SizeResFactor * button2D.Width) * 2, (int)(Ztuff.SizeResFactor * button2D.Height) * 2), button2D);
            bCopy.AddText("Copy", 3, true, Color.White, Font.Default);
            bCopy.OnClick += BCopyPiece;

            bDone = new GUI.Button(designerLayer, new Rectangle((int)(Settings.GetResolution.X * 0.795f), (int)(Settings.GetResolution.Y * 0.877f), (int)(Ztuff.SizeResFactor * button2D.Width) * 5, (int)(Ztuff.SizeResFactor * button2D.Height) * 5), button2D);
            bDone.AddText("Done", 4, true, Color.White, Font.Default);
            bDone.OnClick += BDone;

            Texture2D TextFieldTexture = Load.Get<Texture2D>("Button2");
            nameField = new GUI.TextField(new Layer(MainLayer.GUI, 10), new Layer(MainLayer.GUI, 11), Font.Default, 4, new Rectangle((int)(Settings.GetResolution.X * 0.55f), (int)(Settings.GetResolution.Y * 0.9f), (int)(Ztuff.SizeResFactor * TextFieldTexture.Width) * 5, (int)(Ztuff.SizeResFactor * TextFieldTexture.Width)), new Vector2((int)(Settings.GetResolution.X * 0.565f), (int)(Settings.GetResolution.Y * 0.915)), Vector2.Zero, "", Color.Black, Color.DarkGray, TextFieldTexture);
            nameField.MaxLetters = 14;

            #region //Descriptions
            topName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Name), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)));
            topHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedTopList[currentlyShowingTop].Health.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 1)));
            topAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedTopList[currentlyShowingTop].AttackDamage.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 2)));
            topAbilityRange = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Ability Range: " + unlockedTopList[currentlyShowingTop].AbilityRange.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 3)));
            topCost = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Cost: " + unlockedTopList[currentlyShowingTop].ManaCost.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 4)));
            topAbilityCost = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Ability Cost: " + unlockedTopList[currentlyShowingTop].AbilityCost.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 5)));
            topDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedTopList[currentlyShowingTop].Description), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 6)));

            middleName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Name), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)));
            middleHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedMiddleList[currentlyShowingMiddle].Health.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 1)));
            middleAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedMiddleList[currentlyShowingMiddle].AttackDamage.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 2)));
            middleArmor = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Armor: " + unlockedMiddleList[currentlyShowingMiddle].Armor.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 3)));
            middleCost = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Cost: " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 4)));
            middleDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedMiddleList[currentlyShowingMiddle].Description), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 5)));

            bottomName = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Name), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(Settings.GetResolution.Y * 0.51f)));
            bottomHealth = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Health: " + unlockedBottomList[currentlyShowingBottom].Health.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 1)));
            bottomAttack = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Attack: " + unlockedBottomList[currentlyShowingBottom].AttackDamage.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 2)));
            bottomMovementRange = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Movement Range: " + unlockedBottomList[currentlyShowingBottom].MoveRange.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 3)));
            bottomCost = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Cost: " + unlockedBottomList[currentlyShowingBottom].ManaCost.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 4)));
            bottomMovementCost = new Renderer.Text(designerLayer, Font.Default, new StringBuilder("Movement Cost: " + unlockedBottomList[currentlyShowingBottom].MoveCost.ToString()), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 5)));
            bottomDesc = new Renderer.Text(designerLayer, Font.Default, new StringBuilder(unlockedBottomList[currentlyShowingBottom].Description), 3, 0, new Vector2((int)(Settings.GetResolution.X * 0.51f), (int)(topName.Position.Y + descriptionSpace * 6)));
            #endregion

            #endregion

            topFullDesc.Add(topName, topHealth, topAttack, topAbilityRange, topCost, topAbilityCost, topDesc);
            middleFullDesc.Add(middleName, middleHealth, middleAttack, middleArmor, middleCost, middleDesc);
            bottomFullDesc.Add(bottomName, bottomHealth, bottomAttack, bottomMovementRange, bottomCost, bottomMovementCost, bottomDesc);
            collectionInspector.Add(bCreate, bBack);
            setDesigner.Add(bCancelSet, bArrowHead1, bArrowHead2, bArrowMiddle1, bArrowMiddle2, bArrowBottom1, bArrowBottom2, topSubPiece, middleSubPiece, bottomSubPiece, topFullDesc, middleFullDesc, bottomFullDesc, bNext, bDone, highlight, bCopy, nameField);
            for (int i = 0; i < miniliths.Length; ++i)
            {
                setDesigner.Add(miniliths[i]);
            }
            collections.Add(collectionInspector, setDesigner);
        }

        public void Update(float deltaTime)
        {

        }

        public void Close()
        {
            if (setDesigner.Active == true)
            {
                BDone();
                return;
            }

            BBackToMain();
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
                PersonalData.UserData.SavedSets[setBeingModifiedIndex].Name = nameField.Content;
            }
            else
            {
                PersonalData.UserData.SavedSets.Add(new Set() { Pieces = tempSet.ToList(), Name = nameField.Content });
            }

            SaveLoad.Save();

            BBackToMain();
        }

        private void BCreateSet()
        {
            if (PersonalData.UserData.SavedSets.Count() < 48)
            {
                nameField.Content = "Input name";
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
            nameField.Content = set.Name;

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

            if (PersonalData.UserData.SavedSets.Count == 0)
            {
                PersonalData.CreateDefaultSet();
            }

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
            topAbilityRange.String = new StringBuilder("Ability Range: " + unlockedTopList[currentlyShowingTop].AbilityRange.ToString());
            topCost.String = new StringBuilder("Cost: " + unlockedTopList[currentlyShowingTop].ManaCost.ToString());
            topAbilityCost.String = new StringBuilder("Ability Cost: " + unlockedTopList[currentlyShowingTop].AbilityCost.ToString());
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
            middleArmor.String = new StringBuilder("Armor: " + unlockedMiddleList[currentlyShowingMiddle].Armor.ToString());
            middleCost.String = new StringBuilder("Cost: " + unlockedMiddleList[currentlyShowingMiddle].ManaCost.ToString());
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

            string moveRange = unlockedBottomList[currentlyShowingBottom].MoveRange == 0 ? "Not specified" : unlockedBottomList[currentlyShowingBottom].MoveRange.ToString();
            bottomMovementRange.String = new StringBuilder("Movement Range: " + moveRange);

            bottomCost.String = new StringBuilder("Cost: " + unlockedBottomList[currentlyShowingBottom].ManaCost.ToString());
            bottomMovementCost.String = new StringBuilder("Movement Cost: " + unlockedBottomList[currentlyShowingBottom].MoveCost.ToString());
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
