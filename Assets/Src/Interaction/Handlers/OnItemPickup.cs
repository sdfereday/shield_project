using UnityEngine;
using Game.Entities;
using Game.Inventory;
using Game.Constants;

namespace Game.Interaction
{
    public class OnItemPickup : Handler
    {
        private GameObject gameContext;
        private PlayerInventory inventory;

        private void Start()
        {
            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            inventory = gameContext.GetComponent<PlayerInventory>();
        }

        public override void Run(Transform interactibleTransform, System.Action onHandlerFinished = null)
        {
            Debug.Log("Picked up a " + interactibleTransform.GetComponent<Item>().Name + ".");
            inventory.AddItem(interactibleTransform.GetComponent<Item>().itemDataObject);
        }
    }
}