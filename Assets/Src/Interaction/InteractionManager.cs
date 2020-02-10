using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Toolbox.Helpers;

namespace Game.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public delegate void OnInteracted(INTERACTIBLE_TYPE type, Transform transform);
        public delegate void OnCancelled(INTERACTIBLE_TYPE type, Transform transform);

        public float interactRadius = 1f;
        public Transform interactPosition;
        public Collider interactCollider;
        public LayerMask selectObjectsToHit;
        public Collider[] ignoreColliders;
        
        private void Start()
        {
            interactPosition = interactPosition == null ? transform : interactPosition;
            ignoreColliders = ignoreColliders == null ? GetComponents<Collider>() : ignoreColliders;

            interactCollider = interactCollider ? interactCollider : GetComponent<Collider>();
            interactCollider.isTrigger = true;
        }

        private List<Collider> GetInteractees()
        {
            return Physics.OverlapSphere(interactPosition.position, interactRadius, selectObjectsToHit)
                .Where(x => ignoreColliders.Any(col => x != col))
                .OrderBy(x => Vectors.Dist(x.transform.position, transform.position))
                .Where(x => x != null)
                .ToList();
        }

        private Collider GetClosestInteractee()
        {
            var interactees = GetInteractees();
            return interactees.Count() > 0 ? interactees[0] : null;
        }

        public void Interact(OnInteracted onInteracted = null)
        {
            Collider closestInteractee = GetClosestInteractee();
            IInteractible interactible = closestInteractee?.GetComponent<IInteractible>();

            if (interactible != null)
            {
                onInteracted?.Invoke(interactible.InteractibleType, interactible.Transform);
                interactible.Trigger(INTERACTIBLE_TYPE.USER, transform);
            }
        }

        public void Cancel(OnCancelled onCancelled = null)
        {
            Collider closestInteractee = GetClosestInteractee();
            IInteractible interactible = closestInteractee?.GetComponent<IInteractible>();

            if (interactible != null)
            {
                onCancelled?.Invoke(interactible.InteractibleType, interactible.Transform);
                interactible.Cancel(INTERACTIBLE_TYPE.USER, transform);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}