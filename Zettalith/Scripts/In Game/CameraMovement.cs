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
        const float cameraScaleZoom = 0.15f, cameraKeyboardMoveDivider = 10f;
        int previousScrollWheelValue;

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
                cameraSpeed *= 0.8f * (1f - deltaTime);
                camera.Position += cameraSpeed * 2;
            }

            if (In.MouseState.ScrollWheelValue != previousScrollWheelValue)
            {
                float tempCameraScaleModifier = 1;
                Vector2 tempCameraPositionAddend = new Vector2();

                // Zoomat in
                if (0 < In.MouseState.ScrollWheelValue - previousScrollWheelValue)
                {
                    tempCameraScaleModifier = 1f + cameraScaleZoom;
                    tempCameraPositionAddend = 0.13f * (camera.ScreenToWorldPosition(mousePosition.ToVector2()) - camera.ScreenToWorldPosition(camera.CenterCoordinate));
                }

                // Zoomat ut
                if (0 > In.MouseState.ScrollWheelValue - previousScrollWheelValue)
                {
                    tempCameraScaleModifier = 1 - (cameraScaleZoom / (1f + cameraScaleZoom));
                    tempCameraPositionAddend = -1 * 0.175f * (camera.ScreenToWorldPosition(mousePosition.ToVector2()) - camera.ScreenToWorldPosition(camera.CenterCoordinate));
                }

                for (int i = 0; i < Math.Abs((In.MouseState.ScrollWheelValue - previousScrollWheelValue) / 120); ++i)
                {
                    camera.Scale *= tempCameraScaleModifier;
                    if (camera.Scale > 0.05) { camera.Position += tempCameraPositionAddend; }
                }

                camera.Scale = camera.Scale.Clamp(0.05f, 0.8f);
            }

            if (In.Key(Keys.W))
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - deltaTime * (camera.ScreenToWorldSize(new Vector2(0, camera.ScreenHeight)).Y / cameraKeyboardMoveDivider));
            if (In.Key(Keys.A))
                camera.Position = new Vector2(camera.Position.X - deltaTime * (camera.ScreenToWorldSize(new Vector2(0, camera.ScreenHeight)).Y / cameraKeyboardMoveDivider), camera.Position.Y);
            if (In.Key(Keys.S))
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + deltaTime * (camera.ScreenToWorldSize(new Vector2(0, camera.ScreenHeight)).Y / cameraKeyboardMoveDivider));
            if (In.Key(Keys.D))
                camera.Position = new Vector2(camera.Position.X + deltaTime * (camera.ScreenToWorldSize(new Vector2(0, camera.ScreenHeight)).Y / cameraKeyboardMoveDivider), camera.Position.Y);

            if (camera.Position.Y < ClientSideController.TopLeftCorner.Y) { camera.Position = new Vector2(camera.Position.X, ClientSideController.TopLeftCorner.Y); }
            if (camera.Position.X < ClientSideController.TopLeftCorner.X) { camera.Position = new Vector2(ClientSideController.TopLeftCorner.X, camera.Position.Y); }
            if (camera.Position.Y > ClientSideController.BottomRightCorner.Y) { camera.Position = new Vector2(camera.Position.X, ClientSideController.BottomRightCorner.Y); }
            if (camera.Position.X > ClientSideController.BottomRightCorner.X) { camera.Position = new Vector2(ClientSideController.BottomRightCorner.X, camera.Position.Y); }

            previousWorldPosition = camera.ScreenToWorldPosition(mousePosition.ToVector2());
            previousScrollWheelValue = In.MouseState.ScrollWheelValue;
        }
    }
}
