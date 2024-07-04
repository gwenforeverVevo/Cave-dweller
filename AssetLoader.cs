using SplashKitSDK;

public class AssetLoader
{
    // Loads a bitmap from the specified file path
    public Bitmap LoadBitmap(string name, string filePath)
    {
        Bitmap bitmap = SplashKit.LoadBitmap(name, filePath);
        if (bitmap == null)
        {
            // Display error message and exit if bitmap cannot be loaded
            Console.WriteLine($"Error: Could not load {filePath}!");
            Environment.Exit(1); // Exit the program if the bitmap cannot be loaded
        }
        return bitmap;
    }
    // Loads a list of bitmap frames from a directory path
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

    // Loads a sound effect from the specified file path
    public SoundEffect LoadSound(string name, string filePath)
    {
        SoundEffect sound = SplashKit.LoadSoundEffect(name, filePath);
        if (sound == null)
        {
            // Display error message and exit if sound cannot be loaded
            Console.WriteLine($"Error: Could not load {filePath}!");
            Environment.Exit(1); // Exit the program if the sound cannot be loaded
        }
        return sound;
    }
} 