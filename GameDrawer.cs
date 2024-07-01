// File path: Cave_dweller/GameDrawer.cs
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class GameDrawer
    {
        public static void DrawGame(Player player, List<Goblin> goblins, List<Bitmap> playerRunFrames, Bitmap playerRestBitmap, int currentFrame, bool isMoving, bool showHitboxes)
        {
            // Draw player hitbox if enabled
            int playerSpriteWidth = playerRestBitmap.Width;
            int playerSpriteHeight = playerRestBitmap.Height;
            if (showHitboxes)
            {
                player.DrawHitbox(Color.Purple, playerSpriteWidth, playerSpriteHeight);
            }

            // Draw player health
            DrawHealth(player.Health, new Vector2D() { X = 10, Y = 10 });

            // Draw player ammunition
            DrawAmmunition(player.Ammunition, new Vector2D() { X = 10, Y = 40 });

            // Draw reloading message
            player.DrawReloadMessage();

            if (isMoving)
            {
                SplashKit.DrawBitmap(playerRunFrames[currentFrame], (float)player.GetLocation().X, (float)player.GetLocation().Y);
            }
            else
            {
                SplashKit.DrawBitmap(playerRestBitmap, (float)player.GetLocation().X, (float)player.GetLocation().Y);
            }

            foreach (Goblin goblin in goblins)
            {
                // Draw goblin hitbox if enabled
                Bitmap goblinBitmap = SplashKit.LoadBitmap("goblin_bitmap", "asset\\goblin.png");
                if (goblinBitmap == null)
                {
                    Console.WriteLine("Error: Could not load goblin.png!");
                    Environment.Exit(1); // Exit if bitmap cannot be loaded
                }
                int goblinSpriteWidth = goblinBitmap.Width;
                int goblinSpriteHeight = goblinBitmap.Height;
                if (showHitboxes)
                {
                    goblin.DrawHitbox(Color.Purple, goblinSpriteWidth, goblinSpriteHeight);
                }

                // Draw goblin health
                DrawHealth(goblin.Health, goblin.GetLocation());

                SplashKit.DrawBitmap(goblinBitmap, (float)goblin.GetLocation().X, (float)goblin.GetLocation().Y);
            }

            foreach (Projectile projectile in player.Projectiles)
            {
                projectile.Draw();
                if (showHitboxes)
                {
                    DrawProjectileHitbox(projectile);
                }
            }
        }

        private static void DrawHealth(int health, Vector2D position)
        {
            for (int i = 0; i < health; i++)
            {
                SplashKit.FillRectangle(Color.Red, position.X + i * 20, position.Y, 15, 15); // Drawing small red rectangles to represent health
            }
        }

        private static void DrawAmmunition(int ammunition, Vector2D position)
        {
            SplashKit.DrawText($"Ammunition: {ammunition}", Color.Black, position.X, position.Y);
        }

        private static void DrawProjectileHitbox(Projectile projectile)
        {
            Rectangle hitbox = projectile.Hitbox;
            SplashKit.FillRectangle(Color.Purple, hitbox);
        }
    }
}
