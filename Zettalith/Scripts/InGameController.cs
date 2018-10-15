using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Zettalith.Pieces;

namespace Zettalith
{
    // Jag har en idé, vi skapar en enum -Sixten
    enum GameAction
    {
        Movement, Ability, Attack, Placement, MiscPlacement, Upgrade, Turn, Timer, Death
    }

    // Det här är en extremt dålig idé -Sixten. No -Benjamin
    enum Type
    {
        Start, End
    }

    class InGameController
    {
        Player Local => players?[0];
        Player Remote => players?[1];

        /// <summary>
        /// Can move, piece, origin, target
        /// </summary>
        public event Func<bool, TilePiece, Coordinate, Coordinate> MovementAttempt; 

        /// <summary>
        /// Piece, origin, target
        /// </summary>
        public event Action<TilePiece, Coordinate, Coordinate> MovementStart, MovementEnd;

        /// <summary>
        /// Can cast, caster, target,
        /// </summary>
        public event Func<bool, TilePiece, TilePiece> Ability, Attack;

        /// <summary>
        /// Can place, piece, coordinate
        /// </summary>
        public event Func<bool, Piece, Coordinate> Placement;

        /// <summary>
        /// Can place, OBS INTE FÄRDIGT, coordinates
        /// </summary>
        public event Func<bool, TileObject, Coordinate> MiscPlacement;

        /// <summary>
        /// TODO: Implementera upgrade event
        /// </summary>
        public event Func<bool, object> Upgrade;

        /// <summary>
        /// Round number
        /// </summary>
        public event Action<int> TurnEnd, TurnStart;

        /// <summary>
        /// Elapsed time
        /// </summary>
        public event Action<float> Timer;

        public event Action<Piece> Death;

        Player[] players;

        public void Setup(Player local, Player remote)
        {
            players = new Player[2] { local, remote };

            NetworkManager.Listen("GAMEACTION", RemoteAction);
        }

        public void Request(GameAction actionType, params object[] arg)
        {
            switch (actionType)
            {
                case GameAction.Movement:
                    break;
                case GameAction.Ability:
                    break;
                case GameAction.Attack:
                    break;
                case GameAction.Placement:
                    break;
                case GameAction.MiscPlacement:
                    break;
                case GameAction.Upgrade:
                    break;
                case GameAction.Turn:
                    break;
                case GameAction.Timer:
                    break;
                case GameAction.Death:
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Unimplemented GameAction request: " + actionType.ToString());
                    break;
            }
        }

        public void RemoteAction(byte[] data)
        {
            
        }
    }
}
