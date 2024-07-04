using Cave_dweller;
using SplashKitSDK;

public static class GameDrawer
{
    public static void DrawGame(Player player, List<Goblin> goblins, List<Wolf> wolfs, List<Spider> spiders, List<Bitmap> playerRunRightFrames, List<Bitmap> playerRunLeftFrames, Bitmap playerRestBitmap, int currentFrame, bool isMoving, bool showHitboxes, Bitmap floorBitmap)
    {
        DrawFloor(floorBitmap);

        Vector2D playerPosition = GetSpritePosition(player.GetLocation(), playerRestBitmap.Width, playerRestBitmap.Height);
        DrawCharacterWithFlash(player, playerRunRightFrames[currentFrame], playerPosition);

        if (isMoving)
        {
            if (player.IsFacingRight)
            {
                DrawCharacterWithFlash(player, playerRunRightFrames[currentFrame], playerPosition);
            }
            else
            {
                DrawCharacterWithFlash(player, playerRunLeftFrames[currentFrame], playerPosition);
            }
        }
        else
        {
            DrawCharacterWithFlash(player, playerRestBitmap, playerPosition);
        }

        DrawHealth(player.Health, new Vector2D() { X = 10, Y = 10 });
        DrawAmmunition(player.Ammunition, new Vector2D() { X = 10, Y = 40 });
        DrawPlayerStats(player, new Vector2D() { X = 10, Y = 70 });
        player.DrawReloadMessage();

        if (showHitboxes)
        {
            player.DrawHitbox(Color.Purple, playerRestBitmap.Width, playerRestBitmap.Height);
        }

        foreach (Goblin goblin in goblins)
        {
            Bitmap goblinBitmap = SplashKit.LoadBitmap("goblin_bitmap", "asset\\goblin.png");
            if (goblinBitmap == null)
            {
                Console.WriteLine("Error: Could not load goblin.png!");
                Environment.Exit(1);
            }
            Vector2D goblinPosition = GetSpritePosition(goblin.GetLocation(),goblinBitmap.Width, goblinBitmap.Height);
            DrawCharacterWithFlash(goblin, goblinBitmap, goblinPosition);

            Vector2D healthBarPosition = new Vector2D() { X = goblinPosition.X, Y = goblinPosition.Y - 10 };
            DrawHealth(goblin.Health, healthBarPosition);

            if (showHitboxes)
            {
                goblin.DrawHitbox(Color.Purple, goblinBitmap.Width, goblinBitmap.Height);
                DrawChaseRange(goblin);
            }


        }

        foreach (Wolf wolf in wolfs)
        {
            Bitmap wolfBitmap = SplashKit.LoadBitmap("wolf_bitmap", "asset\\wolf.png");
            if (wolfBitmap == null)
            {
                Console.WriteLine("Error: Could not load wolf.png!");
                Environment.Exit(1);
            }

            Vector2D wolfPosition = GetSpritePosition(wolf.GetLocation(), wolfBitmap.Width, wolfBitmap.Height);
            DrawCharacterWithFlash(wolf, wolfBitmap, wolfPosition);

            Vector2D healthBarPosition = new Vector2D() { X = wolfPosition.X, Y = wolfPosition.Y - 10 };
            DrawHealth(wolf.Health, healthBarPosition);

            if (showHitboxes)
            {
                wolf.DrawHitbox(Color.Purple, wolfBitmap.Width, wolfBitmap.Height);
                DrawChaseRange(wolf);
            }
        }

        foreach (Spider spider in spiders)
        {
            Bitmap spiderBitmap = SplashKit.LoadBitmap("spider_bitmap", "asset\\spider.png");
            if (spiderBitmap == null)
            {
                Console.WriteLine("Error: Could not load spider.png!");
                Environment.Exit(1);
            }

            Vector2D spiderPosition = GetSpritePosition(spider.GetLocation(), spiderBitmap.Width, spiderBitmap.Height);
            DrawCharacterWithFlash(spider, spiderBitmap, spiderPosition);

            Vector2D healthBarPosition = new Vector2D() { X = spiderPosition.X, Y = spiderPosition.Y - 10 };
            DrawHealth(spider.Health, healthBarPosition);

            if (showHitboxes)
            {
                spider.DrawHitbox(Color.Purple, spiderBitmap.Width, spiderBitmap.Height);
                DrawChaseRange(spider);
            }
        }

        foreach (Projectile projectile in player.Projectiles)
        {
            projectile.Draw();
            if (showHitboxes)
            {
                projectile.DrawHitbox();
            }
        }

        DrawDroppedItems(Goblin.DroppedItems);
        DrawDroppedItems(Wolf.DroppedItems);
        DrawDroppedItems(Spider.DroppedItems);
    }

    private static void DrawFloor(Bitmap floorBitmap)
    {
        int screenWidth = SplashKit.ScreenWidth();
        int screenHeight = SplashKit.ScreenHeight();
        int tileWidth = floorBitmap.Width;
        int tileHeight = floorBitmap.Height;
        for (int x = 0; x < screenWidth; x += tileWidth)
        {
            for (int y = 0; y < screenHeight; y += tileHeight)
            {
                SplashKit.DrawBitmap(floorBitmap, x, y);
            }
        }
    }

    private static void DrawCharacterWithFlash(Character character, Bitmap bitmap, Vector2D position)
    {
        if (IsColorRed(character.CurrentColor))
        {
            SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 128), position.X, position.Y, bitmap.Width, bitmap.Height);
        }

        SplashKit.DrawBitmap(bitmap, (float)position.X, (float)position.Y);
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
        DrawTextWithOutline($"Ammunition: {ammunition}", Color.Black, Color.White, position);
    }

    private static void DrawPlayerStats(Player player, Vector2D position)
    {
        DrawTextWithOutline($"Health: {player.Health}", Color.Black, Color.White, position);
        DrawTextWithOutline($"Damage: {player.Damage}", Color.Black, Color.White, new Vector2D() { X = position.X, Y = position.Y + 20 });
        DrawTextWithOutline($"Speed: {player.Speed}", Color.Black, Color.White, new Vector2D() { X = position.X, Y = position.Y + 40 });
        DrawTextWithOutline($"Ammo: {player.Ammunition}", Color.Black, Color.White, new Vector2D() { X = position.X, Y = position.Y + 60 });
    }

    private static void DrawTextWithOutline(string text, Color textColor, Color outlineColor, Vector2D position)
    {
        const int outlineThickness = 2;

        // Draw outline
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y);
        SplashKit.DrawText(text, outlineColor, (float)position.X, (float)position.Y - outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y - outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y - outlineThickness);

        // Draw text
        SplashKit.DrawText(text, textColor, (float)position.X, (float)position.Y);
    }

    private static void DrawChaseRange(Goblin goblin)
    {
        const double chaseThreshold = 250.0;
        SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 128), (float)goblin.GetLocation().X, (float)goblin.GetLocation().Y, (float)chaseThreshold);
    }

    private static void DrawChaseRange(Wolf wolf)
    {
        const double chaseThreshold = 250.0;
        SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 128), (float)wolf.GetLocation().X, (float)wolf.GetLocation().Y, (float)chaseThreshold);
    }

    private static void DrawChaseRange(Spider spider)
    {
        const double chaseThreshold = 250.0;
        SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 128), (float)spider.GetLocation().X, (float)spider.GetLocation().Y, (float)chaseThreshold);
    }

    public static void DrawDroppedItems(List<Item> items)
    {
        foreach (Item item in items)
        {

            SplashKit.DrawBitmap(item.Image, (float)item.Position.X,(float)item.Position.Y);

        }
    }
}