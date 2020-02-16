using System.Collections.Generic;
using Game.DataManagement;
using Game.Inventory;

/* A mock collection of items that, if game saved, will appear here. Usually
 * you'd see these in a JSON file or something. They only get updated when you
 * actually save the game though. You'll find there's no schema right now
 * for the different items but there 'are' objects pre-built that the system
 * will look for. So basically you can't just throw any old thing in here
 * and expect it to work. It has to be an item that exists in the game. */
namespace Game.MockServices
{
    public static class MockItemData
    {
        public static List<ItemMeta> items = new List<ItemMeta>()
        {
            new ItemMeta()
            {
                Id = "chickenId",
                Name = "Chicken",
                Description = "A tasty piece of Chicken.",
                Qty = 1,
                Type = ITEM_TYPE.STANDARD_ITEM,
                HealthValue = 100,
                MpValue = 50
            },
            new ItemMeta()
            {
                Id = "goldenKey",
                Name = "Golden Key Of Legend",
                Description = "The generic golden key that will open a door somewhere. Probably.",
                Qty = 1,
                Type = ITEM_TYPE.KEY_ITEM,
                HealthValue = 0,
                MpValue = 0
            },
            new ItemMeta()
            {
                Id = "cashmereHat",
                Name = "Cashmere Hat",
                Description = "A Hat of purest Cashmere, nice.",
                Qty = 1,
                Type = ITEM_TYPE.KEY_ITEM,
                HealthValue = 0,
                MpValue = 0
            }
        };
    }
}