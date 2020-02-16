﻿using UnityEngine;
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

        private Transform interactionTarget;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(GlobalConsts.PLAYER_TAG);
            inputController = player.GetComponent<InputController>();

            gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            inventory = gameContext.GetComponent<PlayerInventory>();
            sessionController = gameContext.GetComponent<SessionController>();

            sceneContext = GameObject.FindGameObjectWithTag(GlobalConsts.SCENE_CONTEXT_TAG);
            dialogueManager = sceneContext.GetComponent<DialogueManager>();
            mockDialogueService = sceneContext.GetComponent<MockDialogueService>();
        }

        private void OnEnable()
        {
            DialogueManager.OnConversationComplete += OnConversationComplete;
            DialogueManager.OnAddItem += OnAddItem;
            DialogueManager.OnSetStoryPoint -= OnSetStoryPoint;
        }

        private void OnDisable()
        {
            DialogueManager.OnConversationComplete -= OnConversationComplete;
            DialogueManager.OnAddItem -= OnAddItem;
            DialogueManager.OnSetStoryPoint -= OnSetStoryPoint;
        }

        public override void Run(Transform interactibleTransform, System.Action onHandlerFinished = null)
        {
            Debug.Log("Started a conversation.");

            var startId = interactibleTransform
                .GetComponent<DialogueEntity>()
                .GetCurrentDialogueStartId();

            dialogueManager.StartDialogue(startId, mockDialogueService.ChatNodeData);

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

        public void OnSetStoryPoint(string pointId)
        {
            sessionController.SetStoryStep(pointId);
        }
    }
}