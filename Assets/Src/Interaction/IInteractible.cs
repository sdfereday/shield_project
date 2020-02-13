using UnityEngine;

namespace Game.Interaction
{
    public interface IInteractible
    {
        void Trigger(Transform originTransform);
        void Cancel(Transform originTransform);
        Transform Transform { get; }
        INTERACTIBLE_TYPE InteractibleType { get; }
    }
}