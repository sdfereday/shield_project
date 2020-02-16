using UnityEngine;
using Game.MockServices;
using Game.DataManagement;
using Game.Dialogue;

namespace Game.Interaction
{
    public class OnOpenShopEvent : MonoBehaviour
    {
        private void OnEnable()
        {
            DialogueManager.OnOpenShop += OnOpenShop;
        }

        private void OnDisable()
        {
            DialogueManager.OnOpenShop -= OnOpenShop;
        }

        public void OnOpenShop(string shopId)
        {
            Debug.Log("Trigger a shop open command and display using shop Id of " + shopId + ".");

            var shop = MockShopData.shops.Find(x => x.Id == shopId);

            foreach (ShopItemMeta itemMeta in shop.ItemsSold)
            {
                Debug.Log(MockItemData.items.Find(itemData => itemData.Id == itemMeta.Id).Name);
            }
        }
    }
}