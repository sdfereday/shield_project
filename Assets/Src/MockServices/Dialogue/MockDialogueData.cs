using System.Collections.Generic;
using Game.Dialogue;

namespace Game.MockServices
{
    public static class MockDialogueData
    {
        public static List<DialogueWrapper> Items = new List<DialogueWrapper>()
        {
            // Jade
            new DialogueWrapper()
            {
                Id = "cnvA1",
                TriggeredBy = "npcId",
                Valid = true,
                Nodes = new List<DialogueNode>()
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
                        Text = "So, I need you to do something for me {0}. Would you consider visiting {1} (needs fixing) and asking them where my hat is?",
                        ActorId = "npcId",
                        TextParams = new string[] {
                            "playerId",
                            "someRandomId"
                        }
                    },
                    new DialogueNode()
                    {
                        Id = "n5",
                        Text = "Sorry, I'm a little too busy right now.",
                        ActorId = "playerId",
                        IsLast = true,
                        Actions = new List<DialogueAction>
                        {
                            new DialogueAction()
                            {
                                actionKey = "addLogEntry",
                                actionValue = "questId.s0"
                            },
                            new DialogueAction()
                            {
                                actionKey = "invalidateConversation",
                                actionValue = "cnvA1"
                            },
                            new DialogueAction()
                            {
                                actionKey = "validateConversation",
                                actionValue = "cnvA2"
                            }
                        }
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
                                actionKey = "addLogEntry",
                                actionValue = "questId.s1"
                            },
                            new DialogueAction()
                            {
                                actionKey = "invalidateConversation",
                                actionValue = "cnvA1"
                            },
                            new DialogueAction()
                            {
                                actionKey = "validateConversation",
                                actionValue = "cnvA3"
                            }
                        }
                    }
                }
            },
            new DialogueWrapper()
            {
                Id = "cnvA2",
                TriggeredBy = "npcId",
                Valid = false,
                Nodes = new List<DialogueNode>() {
                    new DialogueNode()
                    {
                        Id = "a21",
                        Text = "Have you reconsidered finding my hat?",
                        ActorId = "npcId",
                        Choices = new List<DialogueNode>()
                        {
                            new DialogueNode()
                            {
                                Id = "choice1",
                                To = "n4",
                                Text = "Yes."
                            },
                            new DialogueNode()
                            {
                                Id = "choice2",
                                To = "n5",
                                Text = "No."
                            }
                        },
                    },
                    new DialogueNode()
                    {
                        Id = "n4",
                        Text = "You've persuaded me. I'll go and find it.",
                        ActorId = "playerId",
                        IsLast = true,
                        TextParams = new string[] {
                            "npcId"
                        },
                        Actions = new List<DialogueAction>
                        {
                            new DialogueAction()
                            {
                                actionKey = "addLogEntry",
                                actionValue = "questId.s1"
                            },
                            new DialogueAction()
                            {
                                actionKey = "invalidateConversation",
                                actionValue = "cnvA2"
                            },
                            new DialogueAction()
                            {
                                actionKey = "validateConversation",
                                actionValue = "cnvA3"
                            }
                        }
                    },
                    new DialogueNode()
                    {
                        Id = "n5",
                        Text = "Hmm, well let me know if you change your mind.",
                        ActorId = "npcId",
                        IsLast = true
                    }
                }
            },
            new DialogueWrapper()
            {
                Id = "cnvA3",
                TriggeredBy = "npcId",
                Valid = false,
                Nodes = new List<DialogueNode>() {
                    new DialogueNode()
                    {
                        Id = "dd3",
                        Text = "Did you get my hat yet {0}?",
                        ActorId = "npcId",
                        IsLast = true,
                        TextParams = new string[] {
                            "playerId"
                        }
                    }
                }
            },
            new DialogueWrapper()
            {
                Id = "cnvA4",
                TriggeredBy = "npcId",
                Valid = false,
                Nodes = new List<DialogueNode>() {
                    new DialogueNode()
                    {
                        Id = "dd3",
                        Text = "Thanks again for getting my hat back {0}!",
                        ActorId = "npcId",
                        IsLast = true,
                        TextParams = new string[] {
                            "playerId"
                        }
                    }
                }
            },
            // Shopkeeper
            new DialogueWrapper()
            {
                Id = "cnvB1",
                TriggeredBy = "npcId2",
                Valid = true,
                RequiredLogEntries = new List<string>() { },
                Nodes = new List<DialogueNode>()
                {
                    new DialogueNode()
                    {
                        Id = "asda2",
                        Text = "Heya pal, how can I help?",
                        ActorId = "npcId2",
                        Choices = new List<DialogueNode>()
                        {
                            new DialogueNode()
                            {
                                Id = "choice1",
                                To = "npdId2Shop",
                                Text = "Browse wears."
                            },
                            new DialogueNode()
                            {
                                Id = "choice2",
                                To = "s1",
                                Text = "Ask about the hat.",
                                FindIn = "cnvB2", // needs implementing
                                VisibleWithQuest = "questId.s1"
                            }
                        }
                    },
                    new DialogueNode()
                    {
                        Id = "npdId2Shop",
                        Text = "Excellent. If you like hats, I'm your man.",
                        ActorId = "npcId2",
                        IsLast = true,
                        Actions = new List<DialogueAction>
                        {
                            new DialogueAction()
                            {
                                actionKey = "openShop",
                                actionValue = "defaultShopId"
                            }
                        }
                    }
                }
            },
            new DialogueWrapper()
            {
                Id = "cnvB2",
                TriggeredBy = "npcId2",
                Valid = false,
                RequiredLogEntries = new List<string>() {},
                Nodes = new List<DialogueNode>()
                {
                    new DialogueNode()
                    {
                        Id = "s1",
                        Text = "I heard you have a hat or something for someone..?",
                        ActorId = "playerId",
                        Route = new DialogueRoute()
                        {
                            PositiveId = "s2a",
                            NegativeId = "s2b",
                            RouteBool = new RouteBool()
                            {
                                method = "checkForItem",
                                value = "cashmereHat"
                            }
                        }
                    },
                    new DialogueNode()
                    {
                        Id = "s2a",
                        To = "s3a",
                        Text = "Why are you still asking about that? I already gave you the damn hat!",
                        ActorId = "npcId2",
                        IsLast = true
                    },
                    new DialogueNode()
                    {
                        Id = "s2b",
                        To = "s3b",
                        Text = "They still talkin' about that damn hat? Alright alright, here, take it.",
                        ActorId = "npcId2",
                        IsLast = true,
                        Actions = new List<DialogueAction>
                        {
                            new DialogueAction()
                            {
                                actionKey = "addLogEntry",
                                actionValue = "questId.q1"
                            },
                            new DialogueAction()
                            {
                                actionKey = "addKeyItem",
                                actionValue = "cashmereHat"
                            }
                        }
                    }
                }
            }
        };
    }
}