using UnityEngine;
using Game.Constants;
using Game.Entities;
using Game.Inventory;
using Game.MockServices;

namespace Game.Interaction
{
    public class InteractionHandlerService : MonoBehaviour
    {
        // TODO: This is a temporary solution.
        private bool haltInteractions = false;

        private GameObject gameContext;
        private MockDialogueRunner dialogueRunner;
        private PlayerInventory inventory;
        
        private void Start()
        {
            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            dialogueRunner = gameContext.GetComponent<MockDialogueRunner>();
            inventory = GetComponent<PlayerInventory>();
        }

        public void OnInteracted(INTERACTIBLE_TYPE interactibleType, Transform interactibleTransform)
        {
            if (haltInteractions) return;
            Debug.Log(haltInteractions);

            switch (interactibleType)
            {
                case INTERACTIBLE_TYPE.COLLECTIBLE:
                    // TODO: Run this through the dialogue manager when it's set up properly.
                    Debug.Log("Picked up a " + interactibleTransform.GetComponent<Item>().Name + ".");
                    inventory.AddItem(interactibleTransform.GetComponent<Item>().itemDataObject);
                    break;
                case INTERACTIBLE_TYPE.NPC:
                    // TODO: This is just a simple test. In reality there should be an intemediary step to load the chat up (same as above).
                    dialogueRunner.StartTest(() => {
                        haltInteractions = false;
                    });
                    haltInteractions = true;
                    break;
            }
        }

        public void OnCancelled(INTERACTIBLE_TYPE interactibleType, Transform interactibleTransform)
        {
            // ...
        }
    }
}