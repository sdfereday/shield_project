using UnityEngine;
using Game.Dialogue;
using Game.UserInput;
using Game.Entities;
using Game.Constants;
using Game.MockServices;

namespace Game.Interaction
{
    public class OnDialogueTriggered : Handler
    {
        private GameObject player;
        private InputController inputController;

        private GameObject sceneContext;
        private DialogueManager dialogueManager;
        private MockDialogueService mockDialogueService;

        private Transform interactionTarget;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(GlobalConsts.PLAYER_TAG);
            inputController = player.GetComponent<InputController>();

            sceneContext = GameObject.FindGameObjectWithTag(GlobalConsts.SCENE_CONTEXT_TAG);
            dialogueManager = sceneContext.GetComponent<DialogueManager>();
            mockDialogueService = sceneContext.GetComponent<MockDialogueService>();
        }

        private void OnEnable()
        {
            DialogueManager.OnConversationComplete += OnConversationComplete;
        }

        private void OnDisable()
        {
            DialogueManager.OnConversationComplete -= OnConversationComplete;
        }

        public override void Run(Transform interactibleTransform, System.Action onHandlerFinished = null)
        {
            Debug.Log("Started a conversation.");

            dialogueManager.StartDialogue("n1", mockDialogueService.ChatNodes);

            inputController.ToggleMovement(false);
            inputController.ToggleInteractions(false);

            interactionTarget = interactibleTransform;
        }

        public void OnConversationComplete()
        {
            Debug.Log("Ended a conversation.");

            inputController.ToggleMovement(true);
            inputController.ToggleInteractions(true);

            interactionTarget.GetComponent<InteractibleEntity>()
                .Cancel(transform);

            interactionTarget = null;
        }
    }
}