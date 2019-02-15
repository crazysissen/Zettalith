using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    abstract class Player
    {
        public const string
            MOVEMENTHEADER = "MOVEMENT",
            ABILITYHEADER = "ABILITY";

        protected InGameController inGameController;
        protected MainController mainController;
        protected XNAController xnaController;
        protected Player opponent;

        public List<TilePiece> TilePieces { get; private set; }
        public Set Set { get; private set; }
        public Deck Deck { get; private set; }
        public List<InGamePiece> Hand { get; private set; }

        public virtual void Start(InGameController inGameController, MainController mainController, XNAController xnaController, Player opponent, Deck deck, Set set)
        {
            this.inGameController = inGameController;
            this.mainController = mainController;
            this.xnaController = xnaController;
            this.opponent = opponent;

            Deck = deck;
            Set = set;
        }

        public void EndTurn()
        {
            inGameController.EndTurn();
        }

        public void PlacePiece(InGamePiece piece, int x, int y)
        {
            inGameController.Execute(GameAction.Placement, true, piece.Index, x, y, InGameController.PlayerIndex);
        }


        public Point[] RequestMovement(TilePiece piece)
        {
            return piece.Piece.Bottom.RequestMove(piece.Position).ToArray();

            //return new Point[0];
        }

        public void ExecuteMovement(TilePiece piece, Point point)
        {
            inGameController.Execute(GameAction.Movement, true, piece.GridIndex, point.X, point.Y);
        }

        public void ExecuteAction(TilePiece piece, object[] data)
        {
            inGameController.Execute(GameAction.Ability, true, piece.GridIndex, new OArray() { o = data });
        }

        public InGamePiece TryRemoveFromHand(InGamePiece piece)
        {
            if (Hand.Contains(piece))
            {
                Hand.Remove(piece);
                return piece;
            }

            return null;
        }

        public InGamePiece TryRemoveFromHand(int piece)
        {
            foreach (InGamePiece currentPiece in Hand)
            {
                if (currentPiece.Index == piece)
                {
                    Hand.Remove(currentPiece);
                    return currentPiece;
                }
            }

            return null;
        }

        public virtual void TurnStart()
        {

        }

        public virtual void Update(float deltaTime)
        {
           
        }


    }

    [Serializable]
    struct OArray
    {
        public object[] o;
    }
}
