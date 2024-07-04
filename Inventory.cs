
using System.Collections.Generic;
using System.Linq;
using SplashKitSDK;

namespace Cave_dweller
{
    public class Inventory
    {

        private List<Item> _items;
        // Constructor initializing the items list
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

        // Uses an item from the inventory and optionally removes it after use
        public void UseItem(Item item, Player? player)
        {
            if (ContainsItem(item) && player != null)
            {
                item.Use(player);
                RemoveItem(item); // Optionally remove the item after use
            }
        }

        // Fetches an item from the inventory by name
        public Item? Fetch(string name)
        {
            return _items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Gets the list of items in the inventory
        public List<Item> GetItems()
        {
            return _items;
        }

        // Gets the list of items in the inventory
        public void DropItem(Item item, Vector2D position)
        {
            // Removes the item from the inventory and prints a message
            RemoveItem(item);
            Console.WriteLine($"Dropped item '{item.Name}' at location ({position.X}, {position.Y}).");
        }
    }
}
