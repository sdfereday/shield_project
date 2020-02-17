using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Constants;
using Game.DataManagement;
using Game.UserInput;
using Game.Inventory;

namespace Game.UI
{
    public class UIInventory : MonoBehaviour
    {
        public Transform container;
        public Transform slotParent;
        public Button closeButton;
        public Text titleText;
        public GameObject slotPrefab;
        private PlayerInventory playerInventory;

        private void OnEnable()
        {
            InputController.OnInventory += OpenInventory;
        }

        private void OnDisable()
        {
            InputController.OnInventory -= OpenInventory;
        }

        private void Start()
        {
            playerInventory = GameObject
                .FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG)
                .GetComponent<PlayerInventory>();

            container.gameObject.SetActive(false);
            
            /* onClick is deprecated */
            closeButton.GetComponent<Button>()
                .onClick.AddListener(() => container.gameObject.SetActive(false));

            // Constant please
            titleText.text = "Inventory";
        }

        public void OpenInventory(INPUT_TYPE type)
        {
            container.gameObject.SetActive(true);

            if (slotParent.transform.childCount > 0)
            {
                foreach (Transform child in slotParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
            List<GameObject> prefCache = new List<GameObject>();

            if (playerInventory.Items.Count == 0)
            {
                // Apparently you have to null it before it'll pick up the actual button after that.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
                return;
            }

            playerInventory.Items.ForEach(item =>
            {
                GameObject inst = Instantiate(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity, slotParent);

                inst.GetComponent<UIEventButtonHover>()
                    .RegisterOnEnter(item, (ItemMeta holdsItem) => {
                        titleText.text = "Inventory: " + holdsItem.Name;
                    });

                prefCache.Add(inst);
            });

            // Apparently you have to null it before it'll pick up the actual button after that.
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(prefCache[0]);
        }
    }
}