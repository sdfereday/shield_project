using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Entities;
using Game.MockServices;

namespace Game.Dialogue
{
    public class DialogueService : MonoBehaviour
    {
        private Entity[] ActorEntities;
        public List<DialogueWrapper> Conversations;

        private string ParseName(string actorId)
        {
            Entity currentActor = ActorEntities.FirstOrDefault(y => y.Id == actorId);
            return currentActor ? currentActor.Name : "Undefined";
        }

        private string ParseText(string original, string[] textParams)
        {
            string[] namesFromParams = textParams != null ? textParams
                .Select(id =>
                {
                    Entity current = ActorEntities.FirstOrDefault(y => y.Id == id);
                    return current != null ? current.Name : "Undefined";
                })
                .ToArray() : new string[] { "Undefined" };

            return string.IsNullOrEmpty(original) ?
                string.Empty : string.Format(original, namesFromParams);
        }

        private List<DialogueNode> ParseNodes(List<DialogueNode> nodes)
        {
            return nodes.Select(node =>
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

        public void SetValidationOnConvo(string cnvId, bool state)
        {
            List<DialogueWrapper> cClone = new List<DialogueWrapper>(Conversations);
            cClone[cClone.FindIndex(ind => ind.Id == cnvId)].Valid = state;
            Conversations = cClone;
        }

        public void Reload()
        {
            /* TODO: Make this reload call more efficient, at the moment we reparse
            * on every conversation which isn't great.
            * Not the most performent thing, but it works (it won't work efficiently
            * in a game with lots of npc's though, you need to get what you need,
            * maybe put the id's in the chat data at the top?) */
            ActorEntities = FindObjectsOfType<Entity>();

            // TODO: Create a checker for node id conflicts.
            List<DialogueWrapper> data = MockDialogueData.Items;
            List<DialogueWrapper> tmp = new List<DialogueWrapper>();

            data.ForEach(cnv =>
            {
                cnv.Nodes = ParseNodes(cnv.Nodes);
                tmp.Add(cnv);
            });

            Conversations = tmp;
        }

        private void Start() => Reload();
    }
}