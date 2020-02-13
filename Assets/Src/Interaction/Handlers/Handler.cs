using UnityEngine;

namespace Game.Interaction
{
    public abstract class Handler : MonoBehaviour
    {
        public abstract void Run(Transform interactibleTransform, System.Action onHandlerFinished = null);
    }
}