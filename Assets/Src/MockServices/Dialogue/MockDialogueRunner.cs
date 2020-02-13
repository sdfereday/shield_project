/* This is a fake service trigger that will bind the dialogue manager
 * to some static data. In reality after actual data is loaded, you would
 * no longer use this method. This literally just parks the data to use. */
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Dialogue;
using Game.Entities;

namespace Game.MockServices
{
    public class MockDialogueRunner : MonoBehaviour
    {
        private string FirstNodeId = "n1";
        private DialogueManager Chat;
        private List<DialogueNode> LoadedChatNodes;
        private Entity[] MockEntities;
        private List<DialogueNode> ChatNodeData;

        private System.Action OnChatCompleteCallback;

        private void OnEnable()
        {
            DialogueManager.OnNext += OnNextChatNode;
            DialogueManager.OnConversationComplete += OnConversationComplete;
        }

        private void OnDisable()
        {
            DialogueManager.OnNext -= OnNextChatNode;
            DialogueManager.OnConversationComplete -= OnConversationComplete;
        }

        // ...
        private void OnConversationComplete() => OnChatCompleteCallback?.Invoke();

        // These are some useful methods, keep them.
        private string ParseName(string actorId)
        {
            Entity currentActor = MockEntities.FirstOrDefault(y => y.Id == actorId);
            return currentActor ? currentActor.Name : "{ Undefined }";
        }

        private string ParseText(string original, string[] textParams)
        {
            string[] namesFromParams = textParams != null ? textParams
                .Select(id =>
                {
                    Entity current = MockEntities.FirstOrDefault(y => y.Id == id);
                    return current != null ? current.Name : "{ Undefined }";
                })
                .ToArray() : new string[] { "{ Undefined }" };

            return string.Format(original, namesFromParams);
        }

        private void Start()
        {
            // Not the most performent thing, but it works (it won't work efficiently
            // in a game with lots of npc's though, you need to get what you need,
            // maybe put the id's in the chat data at the top?)
            MockEntities = FindObjectsOfType<Entity>();
            Chat = GetComponent<DialogueManager>();

            // Bootstrap the new conversation (again implementation may vary)
            ChatNodeData = new List<DialogueNode>(MockDialogueData.ChatNodes).Select(node =>
            {
                node.ActorName = ParseName(node.ActorId);
                node.Text = ParseText(node.Text, node.TextParams);

                if (node.Choices != null) {
                    node.Choices
                        .ForEach(subNode =>
                        {
                            subNode.ActorName = ParseName(node.ActorId);
                            subNode.Text = ParseText(subNode.Text, subNode.TextParams);
                        });
                }

                return node;
            }).ToList();
        }

        private void OnNextChatNode(DialogueNode nodeData)
        { }

        public void StartTest(System.Action _OnChatComplete = null)
        {
            if (!Chat.IsActive)
            {
                Chat.StartDialogue(FirstNodeId, ChatNodeData);
                OnChatCompleteCallback = _OnChatComplete;
            }
        }
    }
}