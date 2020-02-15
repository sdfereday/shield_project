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
        private List<DialogueNode> NonLinearNodes = new List<DialogueNode>()
        {
            new DialogueNode()
            {
                Id = "npcId2Shop",
                Text = "Heya pal, how can I help?",
                ActorId = "npcId2",
                Choices = new List<DialogueNode>()
                {
                    new DialogueNode()
                    {
                        Id = "choice1",
                        To = "n4",
                        Text = "Browse wears.",
                        IsLast = true,
                        Actions = new List<DialogueAction>
                        {
                            new DialogueAction()
                            {
                                actionKey = "openShop",
                                actionValue = "shopId"
                            }
                        }
                    },
                    new DialogueNode()
                    {
                        Id = "choice2",
                        To = "s1",
                        Text = "Ask about the hat."
                    }
                },
            }
        };

        private List<DialogueNode> StoryChainNodes = new List<DialogueNode>()
        {
            new DialogueNode()
            {
                Id = "n1",
                To = "n2",
                Text = "Are you {0}?",
                ActorId = "playerId",
                TextParams = new string[] {
                    "npcId"
                }
            },
            new DialogueNode()
            {
                Id = "n2",
                To = "n3",
                Text = "I am, you must be {0}, I've heard a lot about you!",
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
                        Text = "Okay."
                    },
                    new DialogueNode()
                    {
                        Id = "choice2",
                        To = "n5",
                        Text = "Forget it."
                    }
                },
                Text = "So, I need you to do something for me {0}. Would you consider visiting {1} and asking them where my hat is?",
                ActorId = "npcId",
                TextParams = new string[] {
                    "playerId",
                    "locationId"
                }
            },
            new DialogueNode()
            {
                Id = "n5",
                Text = "Sorry, I'm a little too busy right now.",
                ActorId = "playerId",
                IsLast = true
            },
            new DialogueNode()
            {
                Id = "n4",
                Text = "Alright, I'll see what I can do {0}.",
                ActorId = "playerId",
                IsLast = true,
                TextParams = new string[] {
                    "npcId"
                },
                Actions = new List<DialogueAction>
                {
                    new DialogueAction()
                    {
                        actionKey = "setStoryPoint",
                        actionValue = "s1"
                    }
                }
            },
            // ^ Forward shift in story progress someone, only IF the quest was accepted.
            new DialogueNode()
            {
                Id = "s1",
                To = "s2Check",
                Text = "I heard you have a hat or something..?",
                ActorId = "playerId",
            },
            new DialogueNode()
            {
               Id = "s2Check",
               ActorId = "playerId",
               Route = new DialogueRoute()
               {
                    PositiveId = "s2a",
                    NegativeId = "s2b",
                    RouteAction = new DialogueAction()
                    {
                        actionKey = "checkForItem",
                        actionValue = "cashmereHat"
                    }
               }
            },
            new DialogueNode()
            {
                Id = "s2a",
                To = "s3a",
                Text = "Why are you still asking about that? I already gave you the damn hat!",
                ActorId = "npcId2",
            },
            new DialogueNode()
            {
                Id = "s3a",
                Text = "Oh yeah... my mistake.",
                ActorId = "playerId",
                IsLast = true
            },
            new DialogueNode()
            {
                Id = "s2b",
                To = "s3b",
                Text = "They still talkin' about that damn hat? Alright alright, here, take it.",
                ActorId = "npcId2",
            },
            new DialogueNode()
            {
                Id = "s3b",
                Text = "I appreciate it.",
                ActorId = "playerId",
                IsLast = true,
                Actions = new List<DialogueAction>
                {
                    new DialogueAction()
                    {
                        actionKey = "setStoryPoint",
                        actionValue = "q1"
                    },
                    new DialogueAction()
                    {
                        actionKey = "addKeyItem",
                        actionValue = "cashmereHat"
                    }
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

            return string.IsNullOrEmpty(original) ?
                string.Empty : string.Format(original, namesFromParams);
        }

        private void Start()
        {
            // Not the most performent thing, but it works (it won't work efficiently
            // in a game with lots of npc's though, you need to get what you need,
            // maybe put the id's in the chat data at the top?)
            MockEntities = FindObjectsOfType<Entity>();

            foreach(Entity thing in MockEntities)
            {
                Debug.Log(thing.Id);
            }

            // Combine all parsable nodes (careful you don't get ID conflicts)
            List<DialogueNode> allNodes = new List<DialogueNode>();
            allNodes.AddRange(StoryChainNodes);
            allNodes.AddRange(NonLinearNodes);

            // Bootstrap the new conversation (again implementation may vary)
            ChatNodeData = new List<DialogueNode>(allNodes).Select(node =>
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