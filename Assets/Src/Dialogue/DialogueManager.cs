using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Game.Constants;
using Game.Inventory;
using Game.UI;
using Game.DataManagement;

namespace Game.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        /* Flow delegates */
        public delegate void StartedAction();
        public static event StartedAction OnConversationStarted;
        public delegate void CompleteAction();
        public static event CompleteAction OnConversationComplete;
        public delegate void CancelAction();
        public static event CancelAction OnCancelConversation;
        public delegate void NextAction(DialogueNode nodeData);
        public static event NextAction OnNext;

        /* Specific delegates */
        public delegate void AddItemAction(string itemId);
        public static event AddItemAction OnAddItem;
        public delegate void OpenShopAction(string shopId);
        public static event OpenShopAction OnOpenShop;
        public delegate void AddLogEntryAction(string entryValue);
        public static event AddLogEntryAction OnAddLogEntry;
        
        /* Editor visible */
        public GameObject NextButtonPrefab;
        public GameObject ChoiceButtonPrefab;
        public bool debug = false;

        /* Dependencies */
        private UIContext uiContext;
        private DialogueService dialogueService;
        private GameObject DialogueBox;
        private GameObject ButtonContainer;
        private Text NameField;
        private Text DialogueField;
        private DialogueIterator dialogueIterator;
        private List<DialogueWrapper> conversations;
        private QuestLogger questLogger;

        /* Caching */
        // TODO: If you need these to wait each time, use a queue with coroutine instead.
        private List<DialogueAction> PostActionQueue { get; set; }

        /* Component state */
        private bool WaitingForChoices = false;
        private bool IsActive = false;
        private bool ExitScheduled = false;

        private void Awake()
        {
            var gameContext = GameObject.FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG);
            questLogger = gameContext.GetComponent<QuestLogger>();
            dialogueService = gameContext.GetComponent<DialogueService>();

            uiContext = GameObject
                .FindGameObjectWithTag(GlobalConsts.UI_CONTEXT_TAG)
                .GetComponent<UIContext>();
            
            PostActionQueue = new List<DialogueAction>();

            // TODO: Set up a simple animation slide in, as this is a bit crap.
            DialogueBox = uiContext.dialogueBox;
            ButtonContainer = DialogueBox.transform.Find(GlobalConsts.UI_BUTTON_CONTAINER).gameObject;
            NameField = DialogueBox.transform.Find(GlobalConsts.UI_NAME_FIELD).GetComponent<Text>();
            DialogueField = DialogueBox.transform.Find(GlobalConsts.UI_DIALOGUE_FIELD).GetComponent<Text>();

            DialogueBox.SetActive(false);
            ClearButtons();
        }
        
        private void ClearButtons()
        {
            if (ButtonContainer.transform.childCount > 0)
            {
                foreach (Transform child in ButtonContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        // TODO: Belongs in an effects helper
        private IEnumerator TypeSentence(DialogueNode node)
        {
            NameField.text = node.ActorName;
            DialogueField.text = "";

            foreach (char letter in node.Text.ToCharArray())
            {
                DialogueField.text += letter;
                yield return null;
            }

            // Used later for setting first highlighted button
            var buttonCache = new List<GameObject>();

            if (node.HasChoices)
            {
                WaitingForChoices = true;

                Log("Choices available: " + node.Choices.Count);

                node.Choices.ForEach(choice =>
                {
                    if (!string.IsNullOrEmpty(choice.VisibleWithQuest) && !questLogger.HasEntry(choice.VisibleWithQuest))
                    {
                        return;
                    }

                    GameObject ButtonObj = Instantiate(ChoiceButtonPrefab, ButtonContainer.transform.position, Quaternion.identity, ButtonContainer.transform);
                    buttonCache.Add(ButtonObj);

                    ButtonObj.transform.Find("Text").GetComponent<Text>()
                        .text = choice.Text;
                    
                    /* onClick is deprecated */
                    ButtonObj.GetComponent<Button>()
                        .onClick.AddListener(() =>
                        {
                            WaitingForChoices = false;
                            Next(choice.To, choice.FindIn);
                        });
                });

            }
            else if(NextButtonPrefab != null)
            {
                GameObject ButtonObj = Instantiate(NextButtonPrefab, ButtonContainer.transform.position, Quaternion.identity, ButtonContainer.transform);
                buttonCache.Add(ButtonObj);

                ButtonObj.transform.Find("Text").GetComponent<Text>()
                    .text = "Next";

                /* onClick is deprecated */
                ButtonObj.GetComponent<Button>()
                    .onClick.AddListener(() => Next());
            }

            if (buttonCache.Count > 0)
            {
                // Apparently you have to null it before it'll pick up the actual button after that.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(buttonCache[0]);
            }
        }

        private void OnChatComplete()
        {
            IsActive = false;
            ExitScheduled = false;
            DialogueBox.SetActive(false);

            TriggerActions(PostActionQueue);
            PostActionQueue.Clear();

            OnConversationComplete?.Invoke();
        }

        public void StartDialogue(string startChatId, List<DialogueNode> loadedNodes, List<DialogueWrapper> loadedConversations)
        {
            if (loadedNodes.Count == 0)
            {
                throw new UnityException(GlobalConsts.LIST_WAS_EMPTY + transform.name);
            }

            dialogueService.Reload();

            conversations = new List<DialogueWrapper>(loadedConversations);
            dialogueIterator = new DialogueIterator(loadedNodes);

            OnConversationStarted?.Invoke();

            IsActive = true;
            DialogueBox.SetActive(true);
            Next(startChatId);
        }

        public void Next(string nodeId = null, string findIn = null)
        {
            if (WaitingForChoices || !IsActive) return;

            if (ExitScheduled)
            {
                OnChatComplete();
                return;
            }

            ClearButtons();

            if (findIn != null)
            {
                var switchTo = conversations.Find(x => x.Id == findIn);
                dialogueIterator = new DialogueIterator(switchTo.Nodes);

                Debug.Log("Switched to conversation: " + findIn);
            }

            DialogueNode node = dialogueIterator.GoToNext(nodeId);

            if (node == null)
            {
                Debug.LogError("Chat quit unexpectedly. Couldn't find a node to display.");
                OnChatComplete();
                return;
            }

            Log(node.Text);
            DialogueField.text = node.Text;

            ExitScheduled = node.IsLast;

            /* Connection parsing */
            if (node.HasConnection)
            {
                dialogueIterator.PushNext(node.To);
            }

            /* Route parsing */
            if (node.HasRoute)
            {
                bool outcome = false;

                /* Note: Route actions make no sense when queued, so don't bother. Also
                 * not a huge fan of doing this to detect an item existing, but for
                 * now it's all we've got. I'll figure a way to get this working
                 * via a delegate instead at some point. */
                switch (node.Route.RouteBool.method)
                {
                    case DialogueConsts.CHECK_FOR_ITEM:
                        outcome = GameObject
                            .FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG)
                            .GetComponent<PlayerInventory>()
                            .HasItem(node.Route.RouteBool.value);
                        break;
                }

                string outcomeId = outcome ? node.Route.PositiveId : node.Route.NegativeId;

                dialogueIterator.PushNext(outcomeId);
            }

            /* Action parsing & events */
            if (node.HasActions)
            {
                List<DialogueAction> postActions = node.Actions
                    .Where(x => x.waitForFinish)
                    .ToList();

                PostActionQueue.AddRange(postActions);

                List<DialogueAction> immediateActions = node.Actions
                    .Where(x => !x.waitForFinish)
                    .ToList();

                TriggerActions(immediateActions);
            }

            OnNext?.Invoke(node);

            /* Begin output of text */
            StopAllCoroutines();
            StartCoroutine(TypeSentence(node));
        }
        
        public void SetNameField(string name) => NameField.text = name;

        public void TriggerActions(List<DialogueAction> actions)
        {
            actions.ForEach(action =>
            {
                switch (action.actionKey)
                {
                    /* Note: At present you can skip ahead if in the right scene, may want
                         * to prevent this by adding 'must happen before' restrictions on 
                         * chat nodes */
                    case DialogueConsts.CANCEL_CONVERSATION:
                        OnCancelConversation?.Invoke();
                        break;
                    case DialogueConsts.ADD_KEY_ITEM:
                        OnAddItem?.Invoke(action.actionValue);
                        break;
                    case DialogueConsts.OPEN_SHOP:
                        OnOpenShop?.Invoke(action.actionValue);
                        break;
                    case DialogueConsts.ADD_LOG_ENTRY:
                        OnAddLogEntry?.Invoke(action.actionValue);
                        break;
                    case DialogueConsts.INVALIDATE_CONVERSATION:
                        dialogueService.SetValidationOnConvo(action.actionValue, false);
                        break;
                    case DialogueConsts.VALIDATE_CONVERSATION:
                        dialogueService.SetValidationOnConvo(action.actionValue, true);
                        break;
                    case DialogueConsts.TEST_ACTION:
                        Log("This is a test action, it does nothing special.");
                        break;
                }
            });
        }

        public void Mount(string entityId)
        {
            var mostRelevantConvo = dialogueService.Conversations
                .Where(x => x.TriggeredBy == entityId && x.Valid)
                .ToList()
                .FirstOrDefault();

            var startId = mostRelevantConvo.Nodes.FirstOrDefault().Id;

            StartDialogue(startId, mostRelevantConvo.Nodes, dialogueService.Conversations);
        }

        public void Log(string t)
        {
            if (debug) Debug.Log(t);
        }
    }
}