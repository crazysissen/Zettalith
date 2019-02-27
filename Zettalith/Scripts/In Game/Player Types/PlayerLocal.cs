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
        public ClientSideController Renderer { get; private set; }

        public TilePiece ActionPiece { get; private set; }

        public override void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent, Deck deck, Set set)
        {
            base.Start(inGameController, mainController, xnaController, opponent, deck, set);

            Renderer = new ClientSideController(this, InGameController.IsHost, InGameController.StartPlayer == InGameController.PlayerIndex);
        }

        public void SwitchTurns(InGameState newState)
        {
            if (newState == InGameState.Battle)
            {
                Renderer.OpenBattle();

                return;
            }

            Renderer.OpenLogistics();
        }

        public override void TurnStart()
        {
            base.TurnStart();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Renderer.Update(deltaTime);

            UpdateAbility();

            if (Renderer.SetupComplete)
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
                if (In.RightMouseDown)
                {
                    ActionPiece = null;
                    return;
                }

                object[] returnArray = ActionPiece.Piece.Top.UpdateAbility(ActionPiece, ClientSideController.MousePoint.ToRender(), RendererFocus.OnArea(new Rectangle(In.MousePosition, new Point(1, 1)), Layer.Default) && In.LeftMouseDown, out bool cancel);

                // Cancel
                if (cancel)
                {
                    ActionPiece = null;
                    return;
                }

                // Activate ability
                if (returnArray != null)
                {
                    ExecuteAction(ActionPiece, returnArray);
                    ActionPiece = null;
                }
            }
        }
    }
}
