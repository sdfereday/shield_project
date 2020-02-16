using UnityEngine;
using Game.Constants;
using Game.DataManagement;
using Game.Entities;

namespace Game.Dialogue
{
    public class DialogueEntity : MonoBehaviour
    {
        public string defaultDialogueId;

        private GameObject gameContext;
        private SessionController sessionController;
        private InteractibleEntity interactibleEntity;

        private void Start()
        {
            if (string.IsNullOrEmpty(defaultDialogueId))
            {
               // throw new UnityException(GlobalConsts.ERROR_NO_DIALOGUE_START_ID);
            }

            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            sessionController = gameContext.GetComponent<SessionController>();

            interactibleEntity = GetComponent<InteractibleEntity>();
        }
        /*
       public string GetCurrentDialogueStartId()
       {





           SessionController.StoryLocation currentStoryLocation = sessionController.GetCurrentStoryLocation();

           string requiredNpcId = currentStoryLocation.triggeredByActor;
           string startId = currentStoryLocation.storyPointId;

           return interactibleEntity.Id == requiredNpcId ?
               startId : defaultDialogueId; 
    }*/
    }
}