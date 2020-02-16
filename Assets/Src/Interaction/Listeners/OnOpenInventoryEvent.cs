using UnityEngine;
using Game.DataManagement;
using Game.Inventory;
using Game.UserInput;

namespace Game.Interaction
{
    public class OnOpenInventoryEvent : MonoBehaviour
    {
        public PlayerInventory playerInventory;

        private void OnEnable()
        {
            InputController.OnInventory += OnInventory;
        }

        private void OnDisable()
        {
            InputController.OnInventory -= OnInventory;
        }

        public void OnInventory(INPUT_TYPE type)
        {
            if (playerInventory.Items.Count == 0)
            {
                Debug.Log("There's no inventory items to display yet.");
            } else
            {
                Debug.Log("Trigger an inventory open command and display.");
            }

            foreach (ItemMeta itemMeta in playerInventory.Items)
            {
                Debug.Log(itemMeta.Name + " x" + itemMeta.Qty);
            }
        }
    }
}