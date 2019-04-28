using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    class PlayerLocal : Player
    {
        public ClientSideController ClientController { get; private set; }

        public TilePiece ActionPiece { get; private set; }

        public override void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent, Deck deck, Set set)
        {
            base.Start(inGameController, mainController, xnaController, opponent, deck, set);

            ClientController = new ClientSideController(this, inGameController, InGameController.IsHost, InGameController.StartPlayer == InGameController.PlayerIndex);
        }

        public void SwitchTurns(InGameState newState)
        {
            if (newState == InGameState.Battle)
            {
                ClientController.CloseLogistics();
                ClientController.OpenBattle();

                return;
            }

            ClientController.OpenLogistics();
        }

        public override void TurnStart()
        {
            base.TurnStart();
        }

        public override void Update(float deltaTime, InGameState gameState)
        {
            ClientController.Update(deltaTime, gameState);

            UpdateAbility();

            if (ClientController.SetupComplete)
            {

            }
        }

        public void RequestAction(TilePiece piece)
        {
            ActionPiece = piece;

            piece.Piece.Top.InitializeAbility();
        }

        void UpdateAbility()
        {
            if (ActionPiece != null)
            {
                // Cancel
                if (Input.RightMouseDown)
                {
                    ActionPiece = null;
                    return;
                }

                object[] returnArray = ActionPiece.Piece.Top.UpdateAbility(ActionPiece, ClientSideController.MousePoint.ToRender(), RendererFocus.OnArea(new Rectangle(Input.MousePosition, new Point(1, 1)), Layer.Default) && Input.LeftMouseDown, out bool cancel);

                // Cancel
                if (cancel)
                {
                    ActionPiece = null;
                    return;
                }

                // Activate ability
                if (returnArray != null)
                {
                    if (InGameController.LocalMana >= (ActionPiece.Piece.ModifiedStats.AbilityCost - Ztuff.abilityCostDecrease))
                    {
                        ExecuteAction(ActionPiece, returnArray);
                        ActionPiece = null;
                    }
                    else
                    {
                        Mana requiredMana = ActionPiece.Piece.ModifiedStats.AbilityCost - Ztuff.abilityCostDecrease - InGameController.LocalMana;
                        // TODO: Not enough mana for ability pop-up
                        ActionPiece = null;
                        return;
                    }
                }
            }
        }
    }
}
