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

            bBack = new GUI.Button(tutorialLayer, new Rectangle((int)(Settings.GetResolution.X * 0.1), (int)(Settings.GetResolution.Y * 0.9f), (int)(Ztuff.SizeResFactor * buttonTexture.Width * 2), (int)(Ztuff.SizeResFactor * buttonTexture.Height * 2)), buttonTexture) { ScaleEffect = true };
            bBack.AddText("Back", 3 * Font.Multiplier, true, Color.White, Font.Small);
            bBack.OnClick += BGoBack;

            theText = "För att spela ZETTALITH™ är följande bra att veta: Det finns ingen singleplayer, två personer behöver möta varandra i en duell!\n" +

            "Spel kan slås upp mellan datorer på samma eller olika nätverk!\n" +
            "Lokal och global IP-adress visas i lobbyn(host - eller join menyerna), \n" +
            "där man använder respektive adress för att starta spelet mellan olika datorer,\n" +
            "lokal IP för samma nätverk, global annars.\n" +
            "(OBS! Om global IP inte visas i lobbyn pga serverfel eller underhåll funkar det ofta bäst att helt enkelt googla “What’s my IP ?”!)\n" +

            "I ZETTALITH skapar man inte bara sina personliga samlingar med pjäser, utan även pjäserna i sig!\n" +
            "Genom att klicka in på “Collection” i huvudmenyn och välja/ skapa en pjässamling kommer man få möjlighet att skapa de individuella pjäserna(ett huvud, en mitt, och en fot).\n" +
            "Ordningen är sedan omkastad efter att spelet börjat. \n" +
            "Huvudet håller pjäsens specialförmåga, mitten håller ofta större delen av en pjäs hälsa, skada eller eventuella sköld, och foten håller pjäsens rörelseförmåga.\n" +
            "Pjäsens totala kostnad(Mana) summeras ihop från delarnas individuella kostnader.\n" +


            "Efter att man tagit sig in i ett spel kan man använda följande funktioner:\n" +

            "Inspektera pjäs: \n" +
            "Högerklicka på en pjäs när som helst för att inspektera den.\n" +
            "Du kan ta reda på dess placeringskostnad, förmågokostnad och förflyttningskostnad.\n" +
            "Hälsa, sköld och skada visas med ikoner ovanför pjäsen. \n" +

            "Spela pjäs: \n" +
            "Håll inne vänsterklick på en pjäs i handen och förflytta den till en tillgänglig ruta för att spela ut den.\n" +
            "För detta betalar du pjäsens totala kostnad." +

            "Flytta pjäs: \n" +
            "Håll inne vänsterklick på en pjäs på planen och dra den till en tillgänglig ruta för att förflytta den.\n" +
            "Kan inte flyttas direkt efter den spelats ut, eller flera gånger under samma runda.\n" +
            "För detta betalar du pjäsens förflyttningskostnad.\n" +

            "Attackera med pjäs: \n" +
            "            Håll inne vänsterklick på en pjäs på planen och släpp på en närliggande fientlig pjäs för att attackera den.\n" +
            "Kan inte attackera direkt efter den spelats ut eller mer än en gång under samma runda. Detta kostar ingen Mana. \n" +

            "Använda pjäsens speciella förmåga: \n" +
            "Tryck på en pjäs på planen och välj sedan målet för attacken för att använda pjäsens förmåga.\n" +
            "För detta betalar du pjäsens förmågokostnad.\n" +

            "Förflytta kameran:\n" +
            "Håll inne mushjulet och flytta musen för att förflytta kameran och rulla hjulet för att zooma in eller ut.  \n" +
            "Alternativt kan WASD användas.\n" +


            "Spelare alternerar mellan olika turer: Battle och Logistics, \n" +
            "där de spelar vars en och sedan byter efter varje runda är över.\n" +
            "I sin Battle Turn kan man placera pjäser och använda dem, man kan röra de omkring, använda dess speciella förmågor, samt attackera.\n" +
            "Det är i huvudsak här man vinner spelet då målet är att eliminera motståndarens kung som är placerad på planen. \n" +
            "Det är spelaren i Battle Turn som har privilegiet att byta turn.\n" +

            "I ens Logistics Turn ligger fokus på att förvränga siffrorna runt spelplanen. \n" +
            "Man kan stärka och försvaga pjäser, samt samla på passiva bonusar. Kom ihåg att man blir abrupt avbruten när den andra spelaren avslutar sin runda!\n" +


            "Spelet använder fyra olika valutor: röd, grön och blå Mana, samt Essence.\n" +
            "Ens totala mana fylls upp vid början på varje Battle Turn, och Essence ökar i realtid för varje sekund i Logistics Turn, slösa inte tid när du spelar så ökar motståndarens Essence mindre!\n" +
            "Taket maximal mana ökas efter eget val vid början på varje Logistics Turn där det kommer upp tre färgade symboler som motsvarar den färg man vill öka. \n";

            tutorialText = new Renderer.Text(tutorialLayer, Font.Default, theText, 3, 0, new Vector2(Settings.GetResolution.X * 0.1f, Settings.GetResolution.Y * 0.1f));

            collection.Add(bBack, tutorialText);
        }

        public void Update()
        {

        }

        private void BGoBack()
        {
            focusTutorial.Remove();
            collection.Active = false;
            GoBack.Invoke();
        }
    }
}
