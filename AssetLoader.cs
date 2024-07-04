using SplashKitSDK;

public class AssetLoader
{
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

    public List<Bitmap> LoadFrames(string baseName, string directoryPath, int frameCount)
    {
        List<Bitmap> frames = new List<Bitmap>();

        for (int i = 0; i < frameCount; i++)
        {
            string filePath = $"{directoryPath}/{baseName}_{i}.png";
            Bitmap frame = LoadBitmap($"{baseName}_{i}", filePath);
            frames.Add(frame);
        }

        return frames;
    }

    public SoundEffect LoadSound(string name, string filePath)
    {
        SoundEffect sound = SplashKit.LoadSoundEffect(name, filePath);
        if (sound == null)
        {
            Console.WriteLine($"Error: Could not load {filePath}!");
            Environment.Exit(1); // Exit the program if the sound cannot be loaded
        }
        return sound;
    }
} 