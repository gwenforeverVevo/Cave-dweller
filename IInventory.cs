
using SplashKitSDK;

namespace Cave_dweller
{
    public interface IInventory
    {
        // Adds an item to the inventory
        void AddItem(Item item);
        // Removes an item from the inventory
        void RemoveItem(Item item);
        // Checks if the inventory contains a specific item
        bool ContainsItem(Item item);
        // Uses an item from the inventory
        void UseItem(Item item);
        // Drops an item from the inventory at a specific position
        void DropItem(Item item, Vector2D position);
        // Gets the list of items in the inventory
        List<Item> GetItems();
    }
}
