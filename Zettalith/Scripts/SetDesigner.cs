using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SetDesigner
    {
        MainController controller;

        GUI.Collection collection;
        GUI.Button bBlah;

        public void Initialize(MainController controller)
        {
            this.controller = controller;

            collection = new GUI.Collection();
            collection.Origin = new Point(40, 240);

            RendererController.GUI.Add(collection);

            bBlah = new GUI.Button(new Layer(MainLayer.GUI, 10), new Rectangle(0, 0, 200, 40));
            bBlah.OnClick += BBlah;

            collection.Add(bBlah);
        }

        public void Update(float deltaTime)
        {

        }

        private void BBlah()
        {

        }
    }
}
