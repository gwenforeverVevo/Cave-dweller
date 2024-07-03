// File path: Cave_dweller/GameDrawer.cs
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class GameDrawer
    {
        public static void DrawGame(Player player, List<Goblin> goblins, List<Bitmap> playerRunFrames, Bitmap playerRestBitmap, int currentFrame, bool isMoving, bool showHitboxes)
        {
            // Draw player with flash effect
            Vector2D playerPosition = GetSpritePosition(player.GetLocation(), playerRestBitmap.Width, playerRestBitmap.Height);
            if (isMoving)
            {
                DrawCharacterWithFlash(player, playerRunFrames[currentFrame], playerPosition);
            }
            else
            {
                DrawCharacterWithFlash(player, playerRestBitmap, playerPosition);
            }

            // Draw player health
            DrawHealth(player.Health, new Vector2D() { X = 10, Y = 10 });
            // Draw player ammunition
            DrawAmmunition(player.Ammunition, new Vector2D() { X = 10, Y = 40 });
            // Draw player reloading message
            player.DrawReloadMessage();
            // Draw player hitbox if enabled
            if (showHitboxes)
            {
                player.DrawHitbox(Color.Purple, playerRestBitmap.Width, playerRestBitmap.Height);
            }

            // Draw goblins with flash effect
            foreach (Goblin goblin in goblins)
            {
                Bitmap goblinBitmap = SplashKit.LoadBitmap("goblin_bitmap", "asset\\goblin.png");
                if (goblinBitmap == null)
                {
                    Console.WriteLine("Error: Could not load goblin.png!");
                    Environment.Exit(1); // Exit if bitmap cannot be loaded
                }
                Vector2D goblinPosition = GetSpritePosition(goblin.GetLocation(), goblinBitmap.Width, goblinBitmap.Height);
                DrawCharacterWithFlash(goblin, goblinBitmap, goblinPosition);

                // Draw goblin health above the sprite
                Vector2D healthBarPosition = new Vector2D() { X = goblinPosition.X, Y = goblinPosition.Y - 10 };
                DrawHealth(goblin.Health, healthBarPosition);

                // Draw goblin hitbox if enabled
                if (showHitboxes)
                {
                    goblin.DrawHitbox(Color.Purple, goblinBitmap.Width, goblinBitmap.Height);
                    DrawChaseRange(goblin); // Draw chase range when hitboxes are shown
                }
            }

            // Draw player projectiles
            foreach (Projectile projectile in player.Projectiles)
            {
                projectile.Draw();
                // Draw projectile hitbox if enabled
                if (showHitboxes)
                {
                    projectile.DrawHitbox();
                }
            }

            // Draw dropped items
            DrawDroppedItems(Goblin.DroppedItems);
        }

        private static void DrawCharacterWithFlash(Character character, Bitmap bitmap, Vector2D position)
        {
            if (IsColorRed(character.CurrentColor))
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 128), position.X, position.Y, bitmap.Width, bitmap.Height);
            }
            SplashKit.DrawBitmap(bitmap, (float)position.X, (float)position.Y);
        }

        public static void DrawDroppedItems(List<Item> items)
        {
            foreach (Item item in items)
            {
                // Draw the item at its actual position
                SplashKit.DrawBitmap(item.Image, (float)item.Position.X, (float)item.Position.Y);
            }
        }

        private static bool IsColorRed(Color color)
        {
            return color.R == 255 && color.G == 0 && color.B == 0;
        }

        private static Vector2D GetSpritePosition(Vector2D hitboxLocation, int spriteWidth, int spriteHeight)
        {
            double x = hitboxLocation.X - (spriteWidth / 2);
            double y = hitboxLocation.Y - (spriteHeight / 2);
            return new Vector2D() { X = x, Y = y };
        }

        private static void DrawHealth(int health, Vector2D position)
        {
            for (int i = 0; i < health; i++)
            {
                SplashKit.FillRectangle(Color.Red, position.X + i * 20, position.Y, 15, 15);
            }
        }

        private static void DrawAmmunition(int ammunition, Vector2D position)
        {
            SplashKit.DrawText($"Ammunition: {ammunition}", Color.Black, position.X, position.Y);
        }

        private static void DrawChaseRange(Goblin goblin)
        {
            const double chaseThreshold = 250.0;
            SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 128), (float)goblin.GetLocation().X, (float)goblin.GetLocation().Y, (float)chaseThreshold);
        }
    }
}
