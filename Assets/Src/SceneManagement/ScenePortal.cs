using UnityEngine;
using Game.Constants;

namespace Game.SceneManagement
{
    public class ScenePortal : MonoBehaviour
    {
        public string transitionToScene;
        public string spawnLocationName;

        private SceneController sceneController;

        private void Start()
        {
            sceneController = GameObject
                .FindGameObjectWithTag(GlobalConsts.CONTEXT_TAG)
                .GetComponent<SceneController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GlobalConsts.PLAYER_TAG))
            {
                spawnLocationName = !string.IsNullOrEmpty(spawnLocationName) ? spawnLocationName : GlobalConsts.DEFAULT_PLAYER_START;
                sceneController.DoScenePortal(transitionToScene, spawnLocationName);
            }
        }
    }
}