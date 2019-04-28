using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Zettalith
{
    class Tutorial
    {
        MainController controller;

        GUI.Collection collection;

        GUI.Button bBack;

        Texture2D buttonTexture;

        Renderer.SpriteScreen textPicture;

        Renderer.Text tutorialText;

        string theText;

        Action GoBack;

        RendererFocus focusTutorial;

        Layer tutorialLayer;

        public void Initialize(MainController controller, Action goBack, Layer useThisLayer)
        {
            tutorialLayer = useThisLayer;

            GoBack = goBack;

            this.controller = controller;

            focusTutorial = new RendererFocus(tutorialLayer);

            collection = new GUI.Collection();

            RendererController.GUI.Add(collection);

            buttonTexture = Load.Get<Texture2D>("Button1");

            bBack = new GUI.Button(tutorialLayer, new Rectangle((int)(Settings.GetResolution.X * 0.86), (int)(Settings.GetResolution.Y * 0.89f), (int)(Ztuff.SizeResFactor * buttonTexture.Width * 2.5f), (int)(Ztuff.SizeResFactor * buttonTexture.Height * 2.5f)), buttonTexture) { ScaleEffect = true };
            bBack.AddText("Back", 3, true, Color.White, Font.Default);
            bBack.OnClick += BGoBack;

            theText = "To play ZETTALITH, the following is good to know: There is no multiplayer, two people must face in a duel!\n\n" +

            "Games can be created between two computers on the same or different networks.\n" +
            "The local och global ip-addresses are shown in the lobby (host and join menues), \n" +
            "which is where the appropriate address is used to create a game between computers, local IP for the same network, global otherwise.\n" +
            "(NOTE! If the global IP, for any reason, is not shown you can simply type -What's my IP- into a search engine!)\n\n" +

            "In ZETTALITH one does not simply create their own personal collections of pieces but also the pieces themselves!\n" +
            "All your sets may be accessed in the -Collection-. While there you may modify the set's pieces which entails changing their heads, bodies, and feet. \n" +
            "The order of the pieces is then shuffled when the game starts. \n" +
            "The head has a special ability, the body has most of the health and attack damage and perhaps armor, and the foot has the movement ability. \n" +
            "The piece's total mana cost is equal to the sum of the three parts' mana costs.\n\n" +


            "Once you have begun a game you may use one of the following functions:\n\n" +

            "Inspect piece: Right click a piece to inspect it. The piece's placement cost, ability cost, and movement cost will be displayed. Health, armor, and attack damage is displayed through icons above the pieces.\n\n" +

            "Play piece: Hold down left click on a piece in your hand and drag it to an available spot to play it. For this you pay the piece's total mana cost.\n\n" +

            "Move piece: Hold down left click on one of your pieces on the board and drag it to an available spot to move it. \n" +
            "A piece may not be moved on the same round as it is played, or multiple times during the same round. For this you pay the piece's movement cost.\n\n" +

            "Attacking with a piece: Hold down left click on one of your pieces on the board and drag it to an enemy piece it is standing next to attack the enemy piece. \n" +
            "A piece may not attack on the same round as it is played. This costs no mana. \n\n" +

            "Using a piece's special ability: Left click your piece and then left click a target to use your piece's ability. For this you pay the piece's ability cost.\n\n" +

            "Moving the camera: Hold down mouse3 and move the mouse to move the camera and scroll to zoom in or out. Alternatively WASD can be used to move the camera.\n\n" +


            "The players alternate between two different kind of turns: Battle and Logistics. While one player plays the battle turn the other player plays the logistics turn.\n\n" +

            "During the battle turn you may play pieces, move pieces, attack with pieces, and use the pieces' special abilities. \n" +
            "It is mainly during this round you can win the game as the goal is to eliminate the opponent's king, which is a piece on the board. The battle turn player decides when the current turn ends.\n\n" +

            "During the Logistics turn the goal is to manipulate the numbers. You may strengthen and weaken pieces, and collect passive bonuses. \n" +
            "Remember that you will be abruptly interrupted when the other player ends their turn!\n\n" +


            "The game uses four different kinds of currencies: red, green, and blue Mana, as well as Essence. \n" +
            "Your mana is restored at the start of every Battle turn, and Essence is earned in real time for every second spent in the Logistics turn. Don't waste time in the Battle turn!\n" +
            "Your max mana is increased by one at the start of every Logistics turn. You choose which kind of mana increases.";

            tutorialText = new Renderer.Text(tutorialLayer, Font.Default, theText, 1.5f * Settings.GetResolution.Y / 720, 0, new Vector2(Settings.GetResolution.X * 0.01f, Settings.GetResolution.Y * 0.015f));

            /*Texture2D textTexture = Load.Get<Texture2D>("TutorialText");
            textPicture = new Renderer.SpriteScreen(tutorialLayer, textTexture, new Rectangle((int)(Settings.GetResolution.X * 0.01f), (int)(Settings.GetResolution.Y * 0.018), (int)(1.4f * Ztuff.SizeResFactor * textTexture.Bounds.Width), (int)(1.4f * Ztuff.SizeResFactor * textTexture.Bounds.Height)));*/

            collection.Add(bBack, textPicture, tutorialText);
        }

        public void Update()
        {

        }

        public void BGoBack()
        {
            focusTutorial.Remove();
            collection.Active = false;
            GoBack.Invoke();
        }
    }
}
