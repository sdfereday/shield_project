using UnityEngine;
using System;
using Game.Constants;

namespace Game.SceneManagement
{
    public class CamPortal : MonoBehaviour
    {
        public GameObject activateCameraContainer;

        /* GUID's make me feel uncomfortable... */
        private Guid portalGUID = Guid.NewGuid();
        private SceneController sceneController;

        private void Start()
        {
            sceneController = GameObject
                .FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG)
                .GetComponent<SceneController>();

            if (sceneController == null)
            {
                throw new UnityException(GlobalConsts.ERROR_COMPONENT_NULL);
            }

            if (activateCameraContainer == null)
            {
                throw new UnityException(GlobalConsts.ERROR_CAM_CONTAINER_NULL);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            /* When we change the camera, the axis of
             * the players movement changes also. This can be a bit janky so
             * perhaps look at ways in which to improve it. FF7 or Silver to my knowledge
             * didn't worry about this since all scenes were separated. That's something worth
             * considering. Resident evil 2 however... I wonder... */
            if (col.CompareTag(GlobalConsts.PLAYER_TAG))
            {
                sceneController.DoCamPortal(activateCameraContainer, portalGUID);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (activateCameraContainer != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, activateCameraContainer.transform.position);
            }
        }
    }
}