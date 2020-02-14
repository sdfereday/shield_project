/* This is mock data that is used alongside the dialogue runner
 * tests and should not be used in production. */
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Dialogue;
using Game.Entities;

namespace Game.MockServices
{
    public class MockDialogueService : MonoBehaviour
    {
        private Entity[] MockEntities;
        public List<DialogueNode> ChatNodeData;

        /* Nodes could easily have a dialogue editor built for them. */
        private List<DialogueNode> ChatNodes = new List<DialogueNode>()
        {
            new DialogueNode()
            {
                Id = "n1",
                To = "n2",
                Text = "Hello {0}!",
                ActorId = "playerId",
                TextParams = new string[] {
                    "npcId"
                }
            },
            new DialogueNode()
            {
                Id = "n2",
                To = "n3",
                Text = "Hello back {0}!",
                ActorId = "npcId",
                TextParams = new string[] {
                    "playerId"
                }
            },
            new DialogueNode()
            {
                Id = "n3",
                Choices = new List<DialogueNode>()
                {
                    new DialogueNode()
                    {
                        Id = "choice1",
                        To = "n4",
                        Text = "I think I've had enough.",
                        TextParams = new string[] { }
                    },
                    new DialogueNode()
                    {
                        Id = "choice2",
                        To = "n1",
                        Text = "Wait, let's start over {0}!",
                        TextParams = new string[] {
                            "npcId"
                        }
                    }
                },
                Text = "I have some choices for you {0}. Would you consider visiting {1}?",
                ActorId = "npcId",
                TextParams = new string[] {
                    "playerId",
                    "locationId"
                }
            },
            new DialogueNode()
            {
                Id = "n4",
                Text = "Goodbye from choice 1, {0}!",
                ActorId = "playerId",
                IsLast = true,
                TextParams = new string[] {
                    "npcId"
                }
            }
        };

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

        private void Awake()
        {
            // Not the most performent thing, but it works (it won't work efficiently
            // in a game with lots of npc's though, you need to get what you need,
            // maybe put the id's in the chat data at the top?)
            MockEntities = FindObjectsOfType<Entity>();
            
            // Bootstrap the new conversation (again implementation may vary)
            ChatNodeData = new List<DialogueNode>(ChatNodes).Select(node =>
            {
                node.ActorName = ParseName(node.ActorId);
                node.Text = ParseText(node.Text, node.TextParams);

                if (node.Choices != null)
                {
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
    }
}