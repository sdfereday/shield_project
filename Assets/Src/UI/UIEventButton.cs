using UnityEngine;
using Game.DataManagement;

namespace Game.UI
{
    public class UIEventButton : MonoBehaviour
    {
        [HideInInspector]
        public ItemMeta holdsItem;

        public void RegisterItem(ItemMeta linkItem)
            => holdsItem = linkItem;
    }
}