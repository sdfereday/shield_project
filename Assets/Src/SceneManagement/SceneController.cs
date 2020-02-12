using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Constants;
using Game.Toolbox.Effects;

namespace Game.SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        public delegate void SceneLoadStartedAction();
        public static event SceneLoadStartedAction OnSceneLoadStarted;

        public delegate void SceneLoadedAction();
        public static event SceneLoadedAction OnSceneLoadComplete;

        private SceneData currentSceneData;
        private SceneCurtain curtain;
        private Guid activePortalGUID;
        private string playerStartName = GlobalConsts.DEFAULT_PLAYER_START;

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        private void Awake() => curtain = GetComponent<SceneCurtain>();

        private void Init()
        {
            activePortalGUID = Guid.Empty;
            currentSceneData = GameObject.FindGameObjectWithTag(GlobalConsts.SCENE_CONTEXT_TAG)
                .GetComponent<SceneData>();

            var startLoc = GameObject.Find(playerStartName);

            if (startLoc != null)
            {
                var player = GameObject.FindGameObjectWithTag(GlobalConsts.PLAYER_TAG);
                player.transform.position = new Vector3(startLoc.transform.position.x, player.transform.position.y, startLoc.transform.position.z);
            } else
            {
                throw new UnityException(GlobalConsts.ERROR_NO_PLAYER_START);
            }

            if (currentSceneData.defaultCameraContainer != null)
            {
                RepositionCamera(currentSceneData.defaultCameraContainer);
            }
            
            curtain.Fade(SceneCurtain.FadeDirection.Out);
        }

        private void RepositionCamera(GameObject cameraContainerInput)
        {
            var cameraTransform = Camera.main.transform;

            cameraTransform.SetParent(cameraContainerInput.transform);

            cameraTransform.localRotation = Quaternion.identity;
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localScale = Vector3.one;
        }

        private void OnEndTransition() => OnSceneLoadComplete?.Invoke();

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Init();
        
        /* Will trigger a camera change to new area within the same scene, will not cause
         * a re-init in the scene controller. */
        public void DoCamPortal(GameObject cameraContainerInput, Guid portalGUID)
        {
            /* Assumes that if no portal GUID is active, then we can't have left an area yet, so
             * we set that area from this point onwards. */
            if (activePortalGUID == Guid.Empty)
            {
                activePortalGUID = portalGUID;
            }

            if (activePortalGUID != portalGUID)
            {
                activePortalGUID = portalGUID;
                OnSceneLoadStarted?.Invoke();
                curtain.FadeInOut(() => RepositionCamera(cameraContainerInput), () => OnEndTransition());
            }
        }

        /* Will trigger a complete new scene load and will re-init the scene controller
         * to find new player starts and active cam portal possibilities. */
        public void DoScenePortal(string SceneName, string ProvidedStartLocation = GlobalConsts.DEFAULT_PLAYER_START)
        {
            if (string.IsNullOrEmpty(SceneName))
            {
                throw new UnityException(GlobalConsts.ERROR_STRING_EMPTY);
            }

            playerStartName = ProvidedStartLocation;

            OnSceneLoadStarted?.Invoke();
            curtain.FadeIn(() => SceneManager.LoadScene(SceneName));
        }
    }
}