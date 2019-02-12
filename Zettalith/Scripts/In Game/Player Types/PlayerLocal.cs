using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class PlayerLocal : Player
    {
        PlayerRendering renderer;

        public override void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent)
        {
            base.Start(inGameController, mainController, xnaController, opponent);

            renderer = new PlayerRendering(this);
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
