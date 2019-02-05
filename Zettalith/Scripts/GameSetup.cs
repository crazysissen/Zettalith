using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class GameSetup
    {
        private MainController controller;
        private InGameController inGameController;

        public void Initialize(MainController controller, InGameController inGameController)
        {
            this.controller = controller;
            this.inGameController = inGameController;
        }

        public void Update(float deltaTime)
        {

        }
    }
}
