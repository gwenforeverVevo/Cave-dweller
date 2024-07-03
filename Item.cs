// File path: Cave_dweller/Item.cs
using System;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        private readonly Action<Player?> _effect;
        public Bitmap Image { get; }
        public Vector2D Position { get; set; }

        public Item(string name, string description, string imagePath, Action<Player?> effect)
        {
            Name = name;
            Description = description;
            Image = SplashKit.LoadBitmap(name, imagePath);
            _effect = effect;
            Position = new Vector2D(); // Initialize position
        }

        public void Use(Player? player)
        {
            _effect(player);
        }
    }
}
