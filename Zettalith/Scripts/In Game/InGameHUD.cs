using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class InGameHUD
    {
        public GUI.Collection Collection { get; private set; }

        public BattleHUD BattleHUD { get; private set; }
        public LogisticsHUD LogisticsHUD { get; private set; }
        public EndHUD EndHUD { get; private set; }
        public PerkHUD PerkHUD { get; private set; }
        public BuffHUD BuffHUD { get; private set; }
        public BonusHUD BonusHUD { get; private set; }

        GUI.Collection collection;

        public InGameHUD(GUI.Collection inGameCollection, GUI.Collection perkCollection, GUI.Collection buffCollection, GUI.Collection bonusCollection, GUI.Collection battleCollection, GUI.Collection logisticsCollection, GUI.Collection endCollection, InGameController igc, ClientSideController csc, PlayerLocal player)
        {
            Collection = inGameCollection;

            BattleHUD = new BattleHUD(battleCollection, igc, player, csc);
            LogisticsHUD = new LogisticsHUD(logisticsCollection, igc, player, csc);
            EndHUD = new EndHUD(endCollection, igc, player, csc);
            PerkHUD = new PerkHUD(perkCollection, igc, player, csc);
            BuffHUD = new BuffHUD(buffCollection, igc, player, csc);
            BonusHUD = new BonusHUD(bonusCollection, igc, player, csc);
        }

        public void Update(float deltaTime)
        {
            BattleHUD.Update(deltaTime);
            LogisticsHUD.Update(deltaTime);
            EndHUD.Update(deltaTime);
            PerkHUD.Update(deltaTime);
            BuffHUD.Update(deltaTime);
            BonusHUD.Update(deltaTime);
        }
    }
}
