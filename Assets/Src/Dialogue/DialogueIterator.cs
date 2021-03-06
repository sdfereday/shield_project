﻿using System;
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
        private DialogueNode PreviousNode;

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

        public void PushNext(string query)
        {
            DialogueNode NextNode = QueryNode(query);
            ChatQueue.Enqueue(NextNode);
        }
   
        public DialogueNode GoToNext(string query = null)
        {
            if (ChatQueue.Count == 0 && query == null)
            {
                Log.Out("This seems to be the entry call for the conversation. You must pass a query to it.");
                return null;
            }

            DialogueNode CurrentNode = query != null ? QueryNode(query) : ChatQueue.Dequeue();

            if (CurrentNode == null) return null;

            return CurrentNode;
        }
    }
}