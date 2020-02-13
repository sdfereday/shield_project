using UnityEngine;
using Game.Constants;
using Game.Inventory;
using Game.Dialogue;
using Game.MockServices;

namespace Game.Interaction
{
    public class InteractionRouter : MonoBehaviour
    {
        private GameObject gameContext;
        private DialogueManager dialogueManager;
        private PlayerInventory inventory;
        private MockDialogueService mockDialogueService;

        public OnItemPickup itemPickupHandler;
        public OnDialogueTriggered dialogueTriggeredHandler;
        
        private void Start()
        {
            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            dialogueManager = gameContext.GetComponent<DialogueManager>();
            inventory = GetComponent<PlayerInventory>();
        }

        public void OnInteracted(INTERACTIBLE_TYPE interactibleType, Transform interactibleTransform)
        {
            switch (interactibleType)
            {
                case INTERACTIBLE_TYPE.COLLECTIBLE:
                    itemPickupHandler.Run(interactibleTransform);
                    break;
                case INTERACTIBLE_TYPE.NPC:
                    dialogueTriggeredHandler.Run(interactibleTransform);
                    break;
            }
        }

        public void OnCancelled(INTERACTIBLE_TYPE interactibleType, Transform interactibleTransform)
        {
            // ...
        }
    }
}