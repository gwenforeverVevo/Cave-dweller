
using SplashKitSDK;

namespace Cave_dweller
{
    public interface IInventory
    {
        void AddItem(Item item);
        void RemoveItem(Item item);
        bool ContainsItem(Item item);
        void UseItem(Item item);
        void DropItem(Item item, Vector2D position);
        List<Item> GetItems();
    }
}
