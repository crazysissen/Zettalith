using System;
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
        Movement, Ability, Attack, Placement, EndTurn
    }

    enum InGameState
    {
        Setup, Logistics, Battle, End
    }

    class InGameController
    {
        public static InGameController Main { get; private set; }

        public static Grid Grid { get; private set; }

        public static bool IsHost => Main.isHost;
        public static int PlayerIndex { get; private set; }
        public static int StartPlayer { get; private set; }
        public static Player Host => Main.players?[0];
        public static Player Client => Main.players?[1];
        public static PlayerLocal Local => Main.players?[PlayerIndex] as PlayerLocal;
        public static PlayerRemote Remote => Main.players?[(PlayerIndex + 1) % 2] as PlayerRemote;

        XNAController xnaController;
        MainController mainController;

        bool isHost;
        bool loading;
        LoadGame loadGame;

        InGameState gameState;

        Player[] players;
        Deck[] decks;

        LoadedConfig loadedConfig;

        InGamePiece piece;

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

        public InGameController(bool isHost, MainController mainController, XNAController xnaController)
        {
            Main = this;
            Test.Log("InGameController created.");

            this.isHost = isHost;
            this.mainController = mainController;
            this.xnaController = xnaController;
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
            this.loadedConfig = loadedConfig;

            PlayerIndex = isHost ? 0 : 1;
            StartPlayer = loadedConfig.startPlayer;

            NetworkManager.Listen("GAMEACTION", RecieveAction);

            decks = loadedConfig.decks;

            players = new Player[]
            {
                isHost ? CreateLocalPlayer() : CreateRemotePlayer(),
                isHost ? CreateRemotePlayer() : CreateLocalPlayer()
            };

            players[0].Start(this, mainController, xnaController, players[1]);
            players[1].Start(this, mainController, xnaController, players[0]);

            Grid = loadedConfig.map.grid;

            loading = false;
            gameState = InGameState.Setup;

            if (IsHost)
            {
                //Local.PlacePiece(decks[0].Draw(), 3, 3);
            }
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

            Local.Update(deltaTime);
            Remote.Update(deltaTime);

            switch (gameState)
            {
                case InGameState.Setup:
                    UpdateSetup();
                    break;

                case InGameState.Logistics:
                    UpdateLogistics();
                    break;

                case InGameState.Battle:
                    UpdateBattle();
                    break;

                case InGameState.End:
                    UpdateEnd();
                    break;
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

                case GameAction.EndTurn:
                    break;

                default:
                    Test.Log("Unimplemented GameAction request: " + actionType.ToString());
                    break;
            }
        }

        public void Execute(GameAction actionType, bool host, params object[] arg)
        {
            Test.Log("GameAction: " + actionType + ". Requested locally: " + host + ".");

            if (host)
                NetworkManager.Send("GAMEACTION", Bytestreamer.ToBytes(new GameActionType("EXECUTE", actionType, arg)));

            switch (actionType)
            {
                case GameAction.Movement:
                    break;

                case GameAction.Ability:
                    break;

                case GameAction.Attack:
                    break;

                case GameAction.Placement:
                    PlacePiece((InGamePiece)arg[0], (int)arg[1], (int)arg[2], (int)arg[3]);
                    break;

                case GameAction.EndTurn:
                    TurnSwitch();
                    break;

                default:
                    Test.Log("Unimplemented GameAction request: " + actionType.ToString());
                    break;
            }
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

        public void PlacePiece(InGamePiece piece, int x, int y, int player)
        {
            TileObject obj = Grid.Place(x, y, new TilePiece(piece, player));

            obj.Renderer = new Renderer.Sprite(Layer.Default, piece.Texture, new Vector2(x, y * GameRendering.HEIGHTDISTANCE), Vector2.One, Color.White, 0, new Vector2(13, piece.Texture.Height - 9), SpriteEffects.None);
            obj.UpdateRenderer();
        }

        public void SetupEnd()
        {
            gameState = StartPlayer == PlayerIndex ? InGameState.Battle : InGameState.Logistics;
        }

        public void EndTurn()
        {
            if (gameState == InGameState.Battle)
            {
                Execute(GameAction.EndTurn, true);
            }
        }

        void TurnSwitch()
        {
            InGameState newState = gameState == InGameState.Battle ? InGameState.Logistics : InGameState.Battle;

            gameState = newState;

            Local.SwitchTurns(newState);
        }

        private Player CreateLocalPlayer()
        {
            return new PlayerLocal();
        }

        private Player CreateRemotePlayer()
        {
            return new PlayerRemote();
        }

        private void UpdateSetup()
        {

        }

        private void UpdateBattle()
        {

        }

        private void UpdateLogistics()
        {

        }

        private void UpdateEnd()
        {

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
