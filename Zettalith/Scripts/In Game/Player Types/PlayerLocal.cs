using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class PlayerLocal : Player
    {
        public ClientSideController Renderer { get; private set; }

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

            if (Renderer.SetupComplete)
            {

            }
        }
    }
}
