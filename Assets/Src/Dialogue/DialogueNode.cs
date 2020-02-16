using System.Collections.Generic;

namespace Game.Dialogue
{
    [System.Serializable]
    public class RouteBool
    {
        public string method;
        public string value;
    }

    [System.Serializable]
    public class DialogueAction
    {
        public string actionKey;
        public string actionValue;
        // TODO: Setting default may mess with data parse, take note.
        public bool waitForFinish = true;
    }

    [System.Serializable]
    public class DialogueRoute
    {
        public string PositiveId { get; set; }
        public string NegativeId { get; set; }
        public RouteBool RouteBool { get; set; }
    }

    [System.Serializable]
    public class DialogueNode
    {
        /// <summary>
        /// Set in data structures
        /// </summary>
        public string Id { get; set; }
        public string ActorId { get; set; }
        public string[] Targets { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FindIn { get; set; }
        public string Text { get; set; }
        public string VisibleWithQuest { get; set; }
        public bool IsLast { get; set; }
        public List<DialogueNode> Choices { get; set; }
        public List<DialogueAction> Actions { get; set; }
        public DialogueRoute Route { get; set; }

        /// <summary>
        /// Can be populated externally or set in data, up to you
        /// </summary>
        public string ActorName { get; set; }
        public string[] TextParams { get; set; }

        /// <summary>
        /// Simple statuses for this node
        /// </summary>
        public bool HasOrigin => From != null;
        public bool HasConnection => To != null;
        public bool HasChoices => Choices != null ? Choices.Count > 0 : false;
        public bool HasActions => Actions != null ? Actions.Count > 0 : false;
        public bool HasRoute => Route != null;
    }
}