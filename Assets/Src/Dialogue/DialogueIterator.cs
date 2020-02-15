using System;
using System.Linq;
using System.Collections.Generic;
using Game.Toolbox.Helpers;

namespace Game.Dialogue
{
    public class DialogueIterator
    {
        public string ChainPosition { get; private set; }
        private List<DialogueNode> Collection { get; set; }
        private DialogueNode CurrentNode { get; set; }
        private Action<string> OnChatComplete { get; set; }
        private Queue<DialogueNode> ChatQueue { get; set; }

        public DialogueIterator(List<DialogueNode> _collection)
        {
            // TODO: Add to some sort of central error service somewhere.
            try {
                if (_collection.GroupBy(x => x.Id).Any(g => g.Count() > 1))
                {
                    throw new Exception("Collection had duplicate ID's, these must be unique to each node.");
                }
            } catch (Exception e)
            {
                Log.Out(e);
            }
            
            Collection = new List<DialogueNode>(_collection);
            ChatQueue = new Queue<DialogueNode>();
        }

        private bool NodeOutcomeInvalid(DialogueNode node) =>
            node.HasActions && node.HasRoute;

        private bool NodeDataNotValid(DialogueNode node) => node.To == null
            && !node.IsLast
            && !node.HasChoices
            && !node.HasActions
            && !node.HasRoute;

        private bool NodeDataConflict(DialogueNode node) => node.To != null && node.HasChoices;

        private bool ValidateNode(DialogueNode node)
        {
            // TODO: Consts please for these errors.
            if (node == null)
            {
                Log.Out("There was a problem finding a node. Try running 'start' first.");
                return false;
            }

            if (NodeDataNotValid(node))
            {
                Log.Out("The current node is invalid. It must have a 'to' OR 'choices', or, an 'endConversation' action if this was intended.");
                return false;
            }

            if (NodeOutcomeInvalid(node))
            {
                Log.Out("The current node has conflicting exit methods, either choose actions or a route, NOT both.");
                return false;
            }

            if (NodeDataConflict(node))
            {
                Log.Out("The current node is invalid. It must have either 'to' OR 'choices', and not both.");
                return false;
            }

            return true;
        }

        private DialogueNode QueryNode(string query)
        {
            // This will never (at present) scan for choice nodes (may not ever need to).
            DialogueNode nextNode = Collection.FirstOrDefault(node => node.Id == query);
            return ValidateNode(nextNode) ? nextNode : null;
        }

        private DialogueNode PreviousNode;
        public DialogueNode GoToNext(string query = null)
        {
            if (ChatQueue.Count == 0 && query == null)
            {
                Log.Out("This seems to be the entry call for the conversation. You must pass a query to it.");
                return null;
            }

            DialogueNode CurrentNode = query != null ? QueryNode(query) : ChatQueue.Dequeue();

            if (CurrentNode == null) return null;

            if (CurrentNode.HasConnection)
            {
                DialogueNode NextNode = QueryNode(CurrentNode.To);
                ChatQueue.Enqueue(NextNode);
            }

            if (CurrentNode.HasActions)
            {
                CurrentNode.Actions.ForEach(action =>
                {
                    switch(action.actionKey)
                    {
                        case DialogueConsts.SET_STORY_POINT:
                            // ... onSave, etc
                            ChainPosition = action.actionValue;
                            Log.Out("Saved chain up to ID.");
                            break;
                        case DialogueConsts.CANCEL_CONVERSATION:
                            // ... onCancel, etc
                            Log.Out("Cancelled chain, nothing saved.");
                            break;
                        case DialogueConsts.ADD_KEY_ITEM:
                            // ... onItemAdded, etc
                            Log.Out("Should add item to inventory.");
                            break;
                    }
                });
            }

            if (CurrentNode.HasRoute)
            {
                bool outcome = false;

                switch (CurrentNode.Route.RouteAction.actionKey)
                {
                    case DialogueConsts.CHECK_FOR_ITEM:
                        // check if inventory has item
                        // ... todo
                        outcome = false;
                        break;

                }

                string outcomeId = outcome ? CurrentNode.Route.PositiveId : CurrentNode.Route.NegativeId;

                DialogueNode NextNode = QueryNode(outcomeId);
                ChatQueue.Enqueue(NextNode);
            }

            return CurrentNode;
        }
    }
}