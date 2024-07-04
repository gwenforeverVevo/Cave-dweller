﻿
using SplashKitSDK;

namespace Cave_dweller
{
    public static class InputHandler
    {
        public static void HandleInput(Player player, ref bool isMoving, int spriteWidth, int spriteHeight)
        {
            Vector2D direction = new Vector2D() { X = 0, Y = 0 };
            isMoving = false;

            if (SplashKit.KeyDown(KeyCode.WKey)) { direction.Y = -1; isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.SKey)) { direction.Y = 1; isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.AKey)) { direction.X = -1; isMoving = true; }
            if (SplashKit.KeyDown(KeyCode.DKey)) { direction.X = 1; isMoving = true; }

            if (isMoving)
            {
                player.Move(direction);
            }

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Vector2D mousePosition = SplashKit.MousePosition().ToVector2D();
                player.ShootProjectile(mousePosition, spriteWidth, spriteHeight);
            }

            if (SplashKit.KeyTyped(KeyCode.RKey))
            {
                player.Reload();
            }

            // Adjust player speed with key inputs (example: '+' to increase, '-' to decrease)
            //if (SplashKit.KeyTyped(KeyCode.PlusKey))
            //{
            //    player.SetSpeed(player.GetSpeed() + 0.5);
            //}
            //if (SplashKit.KeyTyped(KeyCode.MinusKey))
            //{
            //    player.SetSpeed(player.GetSpeed() - 0.5);
            //}
        }

        private static Vector2D ToVector2D(this Point2D point)
        {
            return new Vector2D() { X = point.X, Y = point.Y };
        }
    }
}
