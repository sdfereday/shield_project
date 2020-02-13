/* This is mock data that is used alongside the dialogue runner
 * tests and should not be used in production. */
using System.Collections.Generic;
using Game.Dialogue;

namespace Game.MockServices
{
    public static class MockDialogueData
    {
        /* Nodes could easily have a dialogue editor built for them. */
        public static List<DialogueNode> ChatNodes = new List<DialogueNode>()
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
    }
}