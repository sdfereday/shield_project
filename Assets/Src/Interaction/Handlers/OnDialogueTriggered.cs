using UnityEngine;
using Game.Dialogue;
using Game.UserInput;
using Game.Entities;
using Game.Constants;
using Game.Inventory;
using Game.DataManagement;

namespace Game.Interaction
{
    public class OnDialogueTriggered : Handler
    {
        private InputController inputController;
        private PlayerInventory inventory;
        private GameObject gameContext;
        private DialogueManager dialogueManager;
        private QuestLogger questLogger;
        private Transform interactionTarget;

        private void Start()
        {
            inputController = GameObject
                .FindGameObjectWithTag(GlobalConsts.PLAYER_TAG)
                .GetComponent<InputController>();

            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            inventory = gameContext.GetComponent<PlayerInventory>();
            questLogger = gameContext.GetComponent<QuestLogger>();
            dialogueManager = gameContext.GetComponent<DialogueManager>();
        }

        private void OnEnable()
        {
            DialogueManager.OnConversationComplete += OnConversationComplete;
            DialogueManager.OnAddItem += OnAddItem;
            DialogueManager.OnAddLogEntry += OnAddLogEntry;
        }

        private void OnDisable()
        {
            DialogueManager.OnConversationComplete -= OnConversationComplete;
            DialogueManager.OnAddItem -= OnAddItem;
            DialogueManager.OnAddLogEntry -= OnAddLogEntry;
        }

        public override void Run(Transform interactibleTransform, System.Action onHandlerFinished = null)
        {
            Debug.Log("Started a conversation.");

            var id = interactibleTransform.GetComponent<Entity>().Id;
            dialogueManager.Mount(id);

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

        public void OnAddItem(string itemId)
        {
            inventory.AddItem(itemId);
        }

        public void OnAddLogEntry(string entryValue)
        {
            questLogger.AddEntry(new LogEntry()
            {
                Value = entryValue
            });
        }        
    }
}