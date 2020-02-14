using UnityEngine;

namespace Game.Interaction
{
    public class InteractionRouter : MonoBehaviour
    {
        public OnItemPickup itemPickupHandler;
        public OnDialogueTriggered dialogueTriggeredHandler;
        
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