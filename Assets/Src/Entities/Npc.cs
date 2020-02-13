using UnityEngine;
using Game.Interaction;

namespace Game.Entities
{
    public class Npc : InteractibleEntity
    {
        public override INTERACTIBLE_TYPE InteractibleType => INTERACTIBLE_TYPE.NPC;

        public override void Cancel(Transform originTransform)
        {
            // Play a sound, etc, or do nothing
        }

        public override void Trigger(Transform originTransform)
        {
            // Play a sound, etc, or do nothing
            Transform.LookAt(originTransform);
        }
    }
}