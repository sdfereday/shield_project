using UnityEngine;
using Game.Toolbox.EditorExtensions;

namespace Game.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public string Id;

        [ReadOnly]
        public string Name;
    }
}