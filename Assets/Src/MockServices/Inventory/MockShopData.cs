using System.Collections.Generic;
using Game.DataManagement;

/* A mock of shop items that, if game saved, will appear here. Usually
 * you'd see these in a JSON file or something. They only get updated when you
 * actually save the game though. You'll find there's no schema right now
 * for the different items but there 'are' objects pre-built that the system
 * will look for. So basically you can't just throw any old thing in here
 * and expect it to work. It has to be an item that exists in the game. */
namespace Game.MockServices
{
    public static class MockShopData
    {
        public static List<ShopMeta> shops = new List<ShopMeta>()
        {
            new ShopMeta()
            {
                Id = "defaultShopId",
                Description = "Default shop of legend.",
                ItemsSold = new List<ShopItemMeta>()
                {
                    new ShopItemMeta()
                    {
                        Id = "chickenId",
                        Qty = 50
                    }
                }
            }
        };
    }
}