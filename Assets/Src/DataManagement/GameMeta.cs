using System.Collections.Generic;
using Game.Inventory;
using Game.Toolbox.EditorExtensions;

namespace Game.DataManagement
{
    [System.Serializable]
    public class ItemMeta
    {
        [ReadOnly]
        public string Id;
        [ReadOnly]
        public string Name;
        [ReadOnly]
        public string Description;
        [ReadOnly]
        public ITEM_TYPE Type;
        [ReadOnly]
        public int HealthValue;
        [ReadOnly]
        public int MpValue;
        [ReadOnly]
        public int Qty;
    }

    [System.Serializable]
    public class NPCMeta
    {
        [ReadOnly]
        public string Id;
        [ReadOnly]
        public string Name;
        [ReadOnly]
        public string Description;
    }

    [System.Serializable]
    public class ShopItemMeta
    {
        [ReadOnly]
        public string Id;
        [ReadOnly]
        public int Qty;
    }

    [System.Serializable]
    public class ShopMeta
    {
        [ReadOnly]
        public string Id;
        [ReadOnly]
        public string Description;
        [ReadOnly]
        public List<ShopItemMeta> ItemsSold;
    }
}
