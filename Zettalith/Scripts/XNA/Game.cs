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
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public MainController MainController { get; private set; }
        public ContentController ContentController { get; private set; }

        public XNAController()
        {
            Graphics = new GraphicsDeviceManager(this);
            MainController = new MainController();
            ContentController = new ContentController(true);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            MainController.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ContentController.Initialize(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MainController.Update(this, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20, 20, 60));

            base.Draw(gameTime);

            MainController.Draw(this, Graphics, SpriteBatch, gameTime);
        }
    }
}
