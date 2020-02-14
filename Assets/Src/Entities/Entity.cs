using UnityEngine;
using Game.Toolbox.EditorExtensions;

namespace Game.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        [ReadOnly]
        public string Id;

        [ReadOnly]
        public string Name;
    }
}