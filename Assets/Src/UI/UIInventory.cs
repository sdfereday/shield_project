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
        private ItemMeta currentlySelectedItem;

        private void OnEnable()
        {
            InputController.OnInventory += OpenInventory;
            InputController.OnDirectional += OnDirectional;
        }

        private void OnDisable()
        {
            InputController.OnInventory -= OpenInventory;
            InputController.OnDirectional -= OnDirectional;
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

        private void UpdateText()
        {
            var currentObj = EventSystem.current
                .currentSelectedGameObject;

            if (currentObj != null)
            {
                UIEventButton uiItem = currentObj.GetComponent<UIEventButton>();

                if (uiItem == null)
                {
                    return;
                }

                titleText.text = "Selected: " + uiItem.holdsItem.Name;
            }
        }

        /* This is a temporary bandaid to solve the problem of
         * a hover event with controller. I'm fairly sure there's
         * a better way but I haven't found it yet. */
        private void OnDirectional(INPUT_TYPE type) =>
            UpdateText();

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

                inst.GetComponent<UIEventButton>()
                    .RegisterItem(item);

                prefCache.Add(inst);
            });

            // Apparently you have to null it before it'll pick up the actual button after that.
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(prefCache[0]);

            UpdateText();
        }
    }
}