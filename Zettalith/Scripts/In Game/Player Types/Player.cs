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
        protected InGameController inGameController;
        protected MainController mainController;
        protected XNAController xnaController;
        protected Player opponent;

        public List<TilePiece> TilePieces { get; private set; }
        public Set Set { get; private set; }

        public virtual void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent)
        {
            this.inGameController = inGameController;
            this.mainController = mainController;
            this.xnaController = xnaController;
            this.opponent = opponent;
        }

        public void EndTurn()
        {
            inGameController.EndTurn();
        }

        public void PlacePiece(InGamePiece piece, int x, int y)
        {
            inGameController.Execute(GameAction.Placement, true, piece, x, y, InGameController.PlayerIndex);
        }

        public virtual void TurnStart()
        {

        }

        public virtual void Update(float deltaTime)
        {

        }
    }
}
