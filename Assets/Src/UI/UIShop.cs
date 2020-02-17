using UnityEngine;
using Game.MockServices;
using Game.DataManagement;
using Game.Dialogue;

namespace Game.UI
{
    public class UIShop : MonoBehaviour
    {
        public GameObject slotPrefab;

        private void OnEnable()
        {
            DialogueManager.OnOpenShop += OpenShop;
        }

        private void OnDisable()
        {
            DialogueManager.OnOpenShop -= OpenShop;
        }

        public void OpenShop(string shopId)
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