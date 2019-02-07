﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Zettalith.Pieces;
using Lidgren.Network;

namespace Zettalith
{
    // Jag har en idé, vi skapar en enum -Sixten
    enum GameAction
    {
        Movement, Ability, Attack, Placement, MiscPlacement, Upgrade, Turn, Timer, Death
    }

    enum InGameState
    {
        Config, Setup, Logistics, Battle, End
    }

    class InGameController
    {
        public static InGameController Main { get; private set; }

        public static Grid Grid { get; private set; }

        Player Local => players?[0];
        Player Remote => players?[1];

        bool isHost;

        bool loading;
        LoadGame loadGame;

        InGameState gameState;

        /// <summary>
        /// Can move, piece, origin, target
        /// </summary>
        public event Func<bool, int, Point, Point> MovementAttempt; 

        /// <summary>
        /// Piece, origin, target
        /// </summary>
        public event Action<int, Point, Point> MovementStart, MovementEnd;

        /// <summary>
        /// Can cast, caster, target,
        /// </summary>
        public event Func<bool, int, int> Ability, Attack;

        /// <summary>
        /// Can place, piece, Point
        /// </summary>
        public event Func<bool, Piece, Point> Placement;

        /// <summary>
        /// Can place, OBS INTE FÄRDIGT, Points
        /// </summary>
        public event Func<bool, TileObject, Point> MiscPlacement;

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

        public InGameController(bool isHost)
        {
            Main = this;
            Test.Log("InGameController created.");

            this.isHost = isHost;
            gameState = InGameState.Config;
        }

        public void Setup(StartupConfig config)
        {
            //players = new Player[2] { local, remote };

            loading = true;
            loadGame = new LoadGame();
            loadGame.Initialize(config, this, isHost);
        }

        public void Initialize(LoadedConfig loadedConfig)
        {
            NetworkManager.Listen("GAMEACTION", RecieveAction);


        }

        public void NewTurnStart()
        {

        }

        public void Update(float deltaTime, MainController mainController, XNAController xnaController)
        {
            if (loading)
            {
                loadGame.Update(deltaTime);

                return;
            }
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
                    Test.Log("Unimplemented GameAction request: " + actionType.ToString());
                    break;
            }
        }

        public void Execute(GameAction actionType, bool host, params object[] arg)
        {
            Test.Log("GameAction: " + actionType + ". Requested locally: " + host + ".");

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
                    Test.Log("Unimplemented GameAction request: " + actionType.ToString());
                    break;
            }

            NetworkManager.Send("GAMEACTION", Bytestreamer.ToBytes(new GameActionType("EXECUTE", actionType, arg)));
        }

        public void RecieveAction(byte[] data)
        {
            GameActionType type = Bytestreamer.ToObject<GameActionType>(data);

            Test.Log(type.subHeader);

            switch (type.subHeader)
            {
                case "EXECUTE":
                    Execute((GameAction)type.arg[0], false, (object[])type.arg[1]);
                    break;

                default:
                    break;
            }
        }
    }

    [System.Serializable]
    struct GameActionType
    {
        public string subHeader;
        public object[] arg;

        public GameActionType(string subHeader, params object[] arg)
        {
            this.subHeader = subHeader;
            this.arg = arg;
        }
    }
}
