using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    abstract class Player
    {
        InGameController inGameController;
        MainController mainController;
        XNAController xnaController;
        Player opponent;

        public List<TilePiece> TilePieces { get; private set; }
        public Set Set { get; private set; }

        public virtual void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent)
        {

        }

        public void EndTurn()
        {

        }

        public virtual void TurnStart()
        {

        }

        public virtual void Update(float deltaTime)
        {

        }
    }
}
