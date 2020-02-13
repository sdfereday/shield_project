using UnityEngine;
using Game.Interaction;

namespace Game.Entities
{
    public abstract class InteractibleEntity : Entity, IInteractible
    {
        public Transform Transform { get => transform; }

        public virtual INTERACTIBLE_TYPE InteractibleType => INTERACTIBLE_TYPE.VOID;

        public abstract void Trigger(Transform originTransform);

        public abstract void Cancel(Transform originTransform);
    }
}