// File path: Cave_dweller/AssetLoader.cs
using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Cave_dweller
{
    public class AssetLoader
    {
        public List<Bitmap> LoadFrames(string baseName, string filePath, int frameCount)
        {
            Bitmap spriteSheet = LoadBitmap(baseName, filePath);
            List<Bitmap> frames = new List<Bitmap>();

            int frameWidth = spriteSheet.Width / 4; // 4 columns
            int frameHeight = spriteSheet.Height / 5; // 5 rows

            for (int i = 0; i < frameCount; i++)
            {
                int srcX = (i % 4) * frameWidth;
                int srcY = (i / 4) * frameHeight;
                Bitmap frame = SplashKit.CreateBitmap($"{baseName}_{i}", frameWidth, frameHeight);
                SplashKit.ClearBitmap(frame, Color.Transparent);
                SplashKit.DrawBitmapOnBitmap(frame, spriteSheet, -srcX, -srcY);
                frames.Add(frame);
            }

            return frames;
        }

        public Bitmap LoadBitmap(string name, string filePath)
        {
            Bitmap bitmap = SplashKit.LoadBitmap(name, filePath);
            if (bitmap == null)
            {
                Console.WriteLine($"Error: Could not load {filePath}!");
                Environment.Exit(1); // Exit the program if the bitmap cannot be loaded
            }
            return bitmap;
        }
    }
}
