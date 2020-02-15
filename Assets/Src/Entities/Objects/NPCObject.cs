using UnityEngine;

namespace Game.Entities
{
    [CreateAssetMenu(fileName = "New NPC Object", menuName = "NPC Object", order = 52)]
    public class NPCObject : ScriptableObject
    {
        public string Id;

        public string _NPCObjectName;
        public string NPCObjectName => _NPCObjectName;

        [TextArea]
        public string _NPCObjectMeta;
        public string NPCObjectMeta => _NPCObjectMeta;
    }
}