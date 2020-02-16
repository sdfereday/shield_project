using UnityEngine;
using Game.Interaction;
using Game.DataManagement;
using Game.Constants;
using Game.MockServices;

namespace Game.Entities
{
    public class Item : InteractibleEntity
    {
        public string itemSessionId;
        public ItemMeta itemData;

        public override INTERACTIBLE_TYPE InteractibleType => INTERACTIBLE_TYPE.COLLECTIBLE;

        private GameObject gameContext;
        private SceneCache sceneCache;

        public override void Cancel(Transform originTransform)
        {
            // Play a sound, etc, or do nothing
        }

        public override void Trigger(Transform originTransform)
        {
            sceneCache.RegisterItem(itemSessionId);
            Destroy(gameObject);
        }

        private void Awake()
        {
            itemData = MockItemData.items.Find(x => x.Id == Id);
            Name = itemData.Name;
        }

        private void Start()
        {         
            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            sceneCache = gameContext.GetComponent<SceneCache>();

            /* We don't use the inventory because of the player uses the item
             * and it gets destroyed, there's no longer a point of reference.
             * We also don't call this when the item is added via the inventory
             * because that means we're dealing with get and set in two places
             * when we only need them here in one place. */
            if (!string.IsNullOrEmpty(itemSessionId))
            {
                bool alreadyOwned = sceneCache.ItemExists(itemSessionId);

                if (alreadyOwned)
                {
                    Destroy(gameObject);
                }
            } else
            {
                throw new UnityException(GlobalConsts.ERROR_STRING_EMPTY);
            }
        }
    }
}