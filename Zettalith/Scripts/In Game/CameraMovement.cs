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

        public void Update(Camera camera, Point mousePosition, bool mouseOnEmpty, float deltaTime)
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
                cameraSpeed *= 0.9f * (1f - deltaTime);
                camera.Position += cameraSpeed * 2;
            }


            previousWorldPosition = camera.ScreenToWorldPosition(mousePosition.ToVector2());
        }
    }
}
