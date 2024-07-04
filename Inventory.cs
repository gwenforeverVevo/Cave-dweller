// File path: Cave_dweller/Inventory.cs
using System.Collections.Generic;
using System.Linq;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Inventory
    {
        private List<Item> _items;

        public Inventory()
        {
            _items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public bool ContainsItem(Item item)
        {
            return _items.Contains(item);
        }

        public void UseItem(Item item, Player? player)
        {
            if (ContainsItem(item) && player != null)
            {
                item.Use(player);
                RemoveItem(item); // Optionally remove the item after use
            }
        }

        public Item? Fetch(string name)
        {
            return _items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Item> GetItems()
        {
            return _items;
        }

        public void DropItem(Item item, Vector2D position)
        {
            // Here you can define what happens when an item is dropped, for now we just remove it from the inventory
            RemoveItem(item);
            Console.WriteLine($"Dropped item '{item.Name}' at location ({position.X}, {position.Y}).");
        }
    }
}
