using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class HUD
    {
        public HUD(InGameController igc, PlayerLocal p, ClientSideController csc)
        {
            inGameController = igc;
            player = p;
            clientSideController = csc;
        }

        public InGameController inGameController;
        public PlayerLocal player;
        public ClientSideController clientSideController;
    }
}
