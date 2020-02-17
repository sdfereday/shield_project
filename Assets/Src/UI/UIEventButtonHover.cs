using UnityEngine.EventSystems;
using UnityEngine;
using Game.DataManagement;

namespace Game.UI
{
    public class UIEventButtonHover : MonoBehaviour, IPointerEnterHandler
    {
        private System.Action<ItemMeta> onHover;
        private ItemMeta holdsItem;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover?.Invoke(holdsItem);
        }

        public void RegisterOnEnter(ItemMeta linkItem, System.Action<ItemMeta> act)
        {
            holdsItem = linkItem;
            onHover = act;
        }
    }
}