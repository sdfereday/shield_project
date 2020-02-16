using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Dialogue;
using Game.DataManagement;
using Game.UserInput;
using Game.Entities;
using Game.Constants;
using Game.Inventory;
using Game.MockServices;

namespace Game.Interaction
{
    public class OnDialogueTriggered : Handler
    {
        private GameObject player;
        private InputController inputController;
        private PlayerInventory inventory;

        private GameObject gameContext;
        private GameObject sceneContext;
        private SessionController sessionController;
        private DialogueManager dialogueManager;
        private MockDialogueService mockDialogueService;
        private WorldLogger logger;

        private Transform interactionTarget;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(GlobalConsts.PLAYER_TAG);
            inputController = player.GetComponent<InputController>();

            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            inventory = gameContext.GetComponent<PlayerInventory>();
            sessionController = gameContext.GetComponent<SessionController>();
            logger = gameContext.GetComponent<WorldLogger>();

            sceneContext = GameObject.FindGameObjectWithTag(GlobalConsts.SCENE_CONTEXT_TAG);
            dialogueManager = sceneContext.GetComponent<DialogueManager>();
            mockDialogueService = sceneContext.GetComponent<MockDialogueService>();
        }

        private void OnEnable()
        {
            DialogueManager.OnConversationComplete += OnConversationComplete;
            DialogueManager.OnAddItem += OnAddItem;
            DialogueManager.OnAddLogEntry += OnAddLogEntry;
            DialogueManager.OnValidateSet += OnValidateSet;
        }

        private void OnDisable()
        {
            DialogueManager.OnConversationComplete -= OnConversationComplete;
            DialogueManager.OnAddItem -= OnAddItem;
            DialogueManager.OnAddLogEntry -= OnAddLogEntry;
            DialogueManager.OnValidateSet -= OnValidateSet;
        }

        public override void Run(Transform interactibleTransform, System.Action onHandlerFinished = null)
        {
            Debug.Log("Started a conversation.");

            /* Find all conversations triggered by the interacted transform (unoptimized at present */
            var id = interactibleTransform.GetComponent<Entity>().Id;
            var mostRelevantConvo =
                mockDialogueService.Conversations.Where(x => x.TriggeredBy == id && x.Valid)
                .ToList()
                .FirstOrDefault();

            var nodeData = mockDialogueService.GetChatNodeData(mostRelevantConvo.Id);
            var startId = nodeData.FirstOrDefault().Id;
                            
            dialogueManager.StartDialogue(startId, nodeData);

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
            Debug.Log("Add entry.");

            logger.AddEntry(new LogEntry()
            {
                Id = entryValue,
                Desc = "Had a conversation and logged value of: " + entryValue
            });
        }

        public void OnValidateSet(string cnvId, bool state)
        {
            mockDialogueService.SetValidationOnConvo(cnvId, state);
        }
    }
}