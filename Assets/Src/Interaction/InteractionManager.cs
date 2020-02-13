using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Toolbox.Helpers;

namespace Game.Interaction
{
    [RequireComponent(typeof(InteractionHandlerService))]
    public class InteractionManager : MonoBehaviour
    {
        public float interactRadius = 1f;
        public Transform interactPosition;
        public Collider interactCollider;
        public LayerMask selectObjectsToHit;
        public Collider[] ignoreColliders;

        private InteractionHandlerService interactionHandler;
        
        private void Start()
        {
            interactPosition = interactPosition == null ? transform : interactPosition;
            interactCollider.isTrigger = true;

            interactionHandler = GetComponent<InteractionHandlerService>();
        }

        private List<Collider> GetInteractees()
        {
            return Physics.OverlapSphere(interactPosition.position, interactRadius, selectObjectsToHit)
                .Where(x => !ignoreColliders.Any(col => x == col))
                .OrderBy(x => Vectors.Dist(x.transform.position, transform.position))
                .Where(x => x != null)
                .ToList();
        }

        private Collider GetClosestInteractee()
        {
            var interactees = GetInteractees();
            return interactees.Count() > 0 ? interactees[0] : null;
        }

        public void Interact()
        {
            Collider closestInteractee = GetClosestInteractee();
            IInteractible interactible = closestInteractee?.GetComponent<IInteractible>();
            
            if (interactible != null)
            {
                interactionHandler.OnInteracted(interactible.InteractibleType, interactible.Transform);
                interactible.Trigger(transform);
            }
        }

        public void Cancel()
        {
            Collider closestInteractee = GetClosestInteractee();
            IInteractible interactible = closestInteractee?.GetComponent<IInteractible>();

            if (interactible != null)
            {
                interactionHandler.OnCancelled(interactible.InteractibleType, interactible.Transform);
                interactible.Cancel(transform);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactPosition.position, interactRadius);
        }
    }
}