using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    public class MainController
    {
        public MainController()
        {
            
        }

        public void Initialize(XNAController systemController)
        {

        }

        public void Update(XNAController systemController, GameTime gameTime)
        {


            DirectInput.UpdateMethods();
        }

        public void Draw(XNAController systemController, GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            
        }
    }
}
