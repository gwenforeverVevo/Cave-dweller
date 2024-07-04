
using Cave_dweller;
using SplashKitSDK;

public class Item
{
    public string Name { get; }
    public string Description { get; }
    private readonly Action<Player> _effect;
    public Vector2D Position { get; set; }
    public Bitmap Image { get; }
    // Constructor initializing the item properties
    public Item(string name, string description, string imagePath, Action<Player> effect)
    {
        Name = name;
        Description = description;
        _effect = effect;
        Image = SplashKit.LoadBitmap(name, imagePath);
        if (Image == null)
        {
            // Display error message and exit if image cannot be loaded
            Console.WriteLine($"Error: Could not load {imagePath}!");
            Environment.Exit(1);
        }
    }
    // Uses the item and applies its effect to the player
    public void Use(Player player)
    {
        _effect(player);
    }
    // Applies the item's effect to the player
    public void ApplyEffect(Player player)
    {
        Use(player);
    }
    // Gets the hitbox of the item based on its position and image size
    public Rectangle Hitbox => SplashKit.RectangleFrom(Position.X, Position.Y, Image.Width, Image.Height);
}
