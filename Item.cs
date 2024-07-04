
using Cave_dweller;
using SplashKitSDK;

public class Item
{
    public string Name { get; }
    public string Description { get; }
    private readonly Action<Player> _effect;
    public Vector2D Position { get; set; }
    public Bitmap Image { get; }

    public Item(string name, string description, string imagePath, Action<Player> effect)
    {
        Name = name;
        Description = description;
        _effect = effect;
        Image = SplashKit.LoadBitmap(name, imagePath);
        if (Image == null)
        {
            Console.WriteLine($"Error: Could not load {imagePath}!");
            Environment.Exit(1);
        }
    }

    public void Use(Player player)
    {
        _effect(player);
    }

    public void ApplyEffect(Player player)
    {
        Use(player);
    }

    public Rectangle Hitbox => SplashKit.RectangleFrom(Position.X, Position.Y, Image.Width, Image.Height);
}
