using UnityEngine;
using Game.Interaction;

namespace Game.Entities
{
    public class Npc : InteractibleEntity
    {
        public override INTERACTIBLE_TYPE InteractibleType => INTERACTIBLE_TYPE.NPC;
        private Transform target;

        public override void Cancel(Transform originTransform)
        {
            target = null;
        }

        public override void Trigger(Transform originTransform)
        {
            target = originTransform;
        }

        private void FaceTarget()
        {
            if (target == null)
            {
                return;
            }

            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void Update() => FaceTarget();
    }
}