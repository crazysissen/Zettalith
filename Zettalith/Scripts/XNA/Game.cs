using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class XNAController : Game
    {
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static MainController MainController { get; private set; }

        string test = ContentController.Get<string>("Hello");

        public XNAController()
        {
            MainController = new MainController();

            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            MainController.Initialize(game: this);

            //System.Diagnostics.Debug.WriteLine();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ContentController.Initialize(Content, true);

            MainController.LateInitialize(game: this);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MainController.Update(game: this, gameTime: gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20, 20, 60));

            base.Draw(gameTime);

            MainController.Draw(game: this, gameTime: gameTime, graphics: Graphics, spriteBatch: SpriteBatch);
        }
    }
}
