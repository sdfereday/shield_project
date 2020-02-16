using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Constants;
using Game.MockServices;
using Game.DataManagement;

namespace Game.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        public List<ItemMeta> itemsField;
        public List<ItemMeta> Items { get => itemsField; }

        private WorldLogger logger;

        private void Awake()
        {
            GameObject gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            logger = gameContext.GetComponent<WorldLogger>();
        }

        // TODO: Usually a data load would likely trigger the Init method, so that needs sorting.
        public void Init(List<ItemMeta> loadedItems = null)
        {
            // Notice how it's the meta that's getting stored here. The SO doesn't get touched
            // and is only used for reference. This might be what needs to be done on the 
            // scene context also...
            itemsField = loadedItems != null ? new List<ItemMeta>(loadedItems) : new List<ItemMeta>();
        }

        public void AddItem(string Id, int qty = 1)
        {
            // TODO: Maybe don't find by type. An id is far more effective (use a table for this).
            // Check for existing and increase qty if so.
            var existing = itemsField.Find(x => x.Id == Id);
            
            if (existing != null)
            {
                if (existing.Type == ITEM_TYPE.KEY_ITEM)
                {
                    throw new UnityException(GlobalConsts.ERROR_KEYITEM_UNIQUE + transform.name);
                }

                existing.Qty += qty;
            }
            else
            {
                // TODO: Make sure we're checking against a schema to cut down on splicing hacks (if it's not in schema, it's not getting in).
                var itemData = MockInventoryData.items.Find(x => x.Id == Id);
                
                itemsField.Add(new ItemMeta()
                {
                    Id = itemData.Id,
                    Name = itemData.Name,
                    Description = itemData.Description,
                    Qty = qty,
                    Type = itemData.Type,
                    HealthValue = itemData.HealthValue,
                    MpValue = itemData.MpValue
                });

                // Tell the world logger we picked up an important item
                if (itemData.Type == ITEM_TYPE.KEY_ITEM)
                {
                    logger.AddEntry(new LogEntry()
                    {
                        Id = itemData.Id,
                        Desc = "Added a key item to inventory: " + itemData.Id
                    });
                }
            }
        }

        public void RemoveItem(string Id, int qty = 1)
        {
            var existing = itemsField.Find(x => x.Id == Id);

            if (existing != null)
            {
                if (existing.Type == ITEM_TYPE.KEY_ITEM)
                {
                    return;
                }

                if (existing.Qty - qty > 0)
                {
                    existing.Qty -= qty;
                }
                else
                {
                    itemsField.RemoveAll(x => x.Id == Id);
                }
            }
        }

        public bool HasItem(string Id) => itemsField.Any(x => x.Id == Id);
    }
}