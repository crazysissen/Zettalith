using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zettalith
{
    class CameraMovement
    {
        Vector2 previousWorldPosition;
        Vector2 cameraSpeed = new Vector2();

        public void Update(Camera camera, Point mousePosition, bool mouseOnEmpty)
        {
            Vector2 initialWorldPosition = camera.ScreenToWorldPosition(mousePosition.ToVector2());

            if (In.MouseState.MiddleButton == ButtonState.Pressed)
            {
                Vector2 tempCameraMove = new Vector2(previousWorldPosition.X - initialWorldPosition.X, previousWorldPosition.Y - initialWorldPosition.Y);
                camera.Position += tempCameraMove;
                cameraSpeed = tempCameraMove;
            }
            else
            {
                camera.Position += cameraSpeed * 0.6f;
            }

            previousWorldPosition = camera.ScreenToWorldPosition(mousePosition.ToVector2());
        }
    }
}
