using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class PlayerLocal : Player
    {
        GameRendering renderer;

        public override void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent)
        {
            base.Start(inGameController, mainController, xnaController, opponent);

            //renderer = new GameRendering(this);
        }

        public override void TurnStart()
        {
            base.TurnStart();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            renderer.Render(deltaTime);
        }
    }
}
