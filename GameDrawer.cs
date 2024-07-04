
using Cave_dweller;
using SplashKitSDK;
using System;
using System.Collections.Generic;

public static class GameDrawer
{
    public static void DrawGame(Player player, List<Monster> monsters, List<Bitmap> playerRunRightFrames,
                                List<Bitmap> playerRunLeftFrames, Bitmap playerRestBitmap, int currentFrame,
                                bool isMoving, bool showHitboxes, Bitmap floorBitmap, int highScore)
    {
        DrawFloor(floorBitmap);

        Vector2D playerPosition = GetSpritePosition(player.GetLocation(), playerRestBitmap.Width, playerRestBitmap.Height);

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
        //DrawPlayerScore(player.Score, new Vector2D() { X = SplashKit.ScreenWidth() / 2, Y = 10 });
        DrawPlayerScore(player.Score, highScore, new Vector2D() { X = SplashKit.ScreenWidth() / 2, Y = 10 });

        if (showHitboxes)
        {
            player.DrawHitbox(Color.Purple, playerRestBitmap.Width, playerRestBitmap.Height);
        }

        foreach (Monster monster in monsters)
        {
            Bitmap monsterBitmap = LoadMonsterBitmap(monster.Type);
            Vector2D monsterPosition = GetSpritePosition(monster.GetLocation(), monsterBitmap.Width, monsterBitmap.Height);
            DrawCharacterWithFlash(monster, monsterBitmap, monsterPosition);

            Vector2D healthBarPosition = new Vector2D() { X = monsterPosition.X, Y = monsterPosition.Y - 10 };
            DrawHealth(monster.Health, healthBarPosition);

            if (showHitboxes)
            {
                monster.DrawHitbox(Color.Purple, monsterBitmap.Width, monsterBitmap.Height);
                DrawChaseRange(monster);
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

    private static Bitmap LoadMonsterBitmap(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.Goblin:
                return SplashKit.LoadBitmap("goblin_bitmap", "asset\\goblin.png");
            case MonsterType.Wolf:
                return SplashKit.LoadBitmap("wolf_bitmap", "asset\\wolf.png");
            case MonsterType.Spider:
                return SplashKit.LoadBitmap("spider_bitmap", "asset\\spider.png");
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    private static void DrawPlayerScore(int score, int highScore, Vector2D position)
    {
        string scoreText = $"Score: {score}";
        string highScoreText = $"High Score: {highScore}";
 
        DrawTextWithOutline(scoreText, Color.Black, Color.White, position);
        DrawTextWithOutline(highScoreText, Color.Black, Color.White, new Vector2D() { X = position.X + 100, Y = position.Y });

    }

    private static void DrawTextWithOutline(string text, Color textColor, Color outlineColor, Vector2D position)
    {
        const int outlineThickness = 2;
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y);
        SplashKit.DrawText(text, outlineColor, (float)position.X, (float)position.Y - outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y - outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X - outlineThickness, (float)position.Y + outlineThickness);
        SplashKit.DrawText(text, outlineColor, (float)position.X + outlineThickness, (float)position.Y - outlineThickness);
        SplashKit.DrawText(text, textColor, (float)position.X, (float)position.Y);
    }

    private static void DrawChaseRange(Monster monster)
    {
        const double chaseThreshold = 250.0;
        SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 128), (float)monster.GetLocation().X, (float)monster.GetLocation().Y, (float)chaseThreshold);
    }

    public static void DrawDroppedItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            SplashKit.DrawBitmap(item.Image, (float)item.Position.X, (float)item.Position.Y);
        }
    }
}
