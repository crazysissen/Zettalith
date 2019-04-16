using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class LogisticsHUD : HUD
    {


        GUI.Collection collection;

        public LogisticsHUD(GUI.Collection collection, InGameController igc, PlayerLocal p, ClientSideController csc) : base(igc, p, csc)
        {
            this.collection = collection;


        }

        public void Update(float deltaTime)
        {

        }
    }
}
