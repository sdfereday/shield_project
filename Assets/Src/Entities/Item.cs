using UnityEngine;
using Game.Interaction;
using Game.Inventory;

namespace Game.Entities
{
    public class Item : InteractibleEntity
    {
        public CollectibleItem itemDataObject;

        public override INTERACTIBLE_TYPE InteractibleType => INTERACTIBLE_TYPE.COLLECTIBLE;

        public override void Cancel(Transform originTransform)
        {
            // Play a sound, etc, or do nothing
        }

        public override void Trigger(Transform originTransform)
        {
            // Play a sound, etc, or do nothing
            Destroy(gameObject);
        }

        private void Awake()
        {
            if (itemDataObject != null)
            {
                Id = itemDataObject.Id;
                Name = itemDataObject.CollectibleItemName;
            }
        }
    }
}