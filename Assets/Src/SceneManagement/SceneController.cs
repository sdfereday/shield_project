using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game.Constants;

namespace Game.SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        public delegate void SceneLoadStartedAction();
        public static event SceneLoadStartedAction OnSceneLoadStarted;

        public delegate void SceneLoadedAction();
        public static event SceneLoadedAction OnSceneLoadComplete;

        public RawImage fadeOutUIImage;
        public RawImage fadeOutLoadingImage;
        public float fadeSpeed = 0.8f;
        public enum FadeDirection
        {
            In, // Alpha = 1
            Out // Alpha = 0
        }

        private SceneData currentSceneData;
        private Guid activePortalGUID;
        private string playerStartName = GlobalConsts.DEFAULT_PLAYER_START;

        /// <summary>
        /// Scene controller functionality
        /// </summary>
        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

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
                throw new UnityException(GlobalConsts.ERROR_NO_PLAYER_START + transform.name);
            }

            RepositionCamera(currentSceneData.defaultCameraContainer);

            Fade(FadeDirection.Out);
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
                FadeInOut(() => RepositionCamera(cameraContainerInput), () => OnEndTransition());
            }
        }

        /* Will trigger a complete new scene load and will re-init the scene controller
         * to find new player starts and active cam portal possibilities. */
        public void DoScenePortal(string SceneName, string ProvidedStartLocation = GlobalConsts.DEFAULT_PLAYER_START)
        {
            if (string.IsNullOrEmpty(SceneName))
            {
                throw new UnityException(GlobalConsts.ERROR_STRING_EMPTY + transform.name);
            }

            playerStartName = ProvidedStartLocation;

            OnSceneLoadStarted?.Invoke();
            FadeIn(() => SceneManager.LoadScene(SceneName));
        }

        
        /// <summary>
        /// UI & Scene Effects
        /// </summary>
        private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
        {
            fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
            fadeOutLoadingImage.color = new Color(fadeOutLoadingImage.color.r, fadeOutLoadingImage.color.g, fadeOutLoadingImage.color.b, alpha);

            alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
        }

        private IEnumerator DoFade(FadeDirection fadeDirection)
        {
            float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
            float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;

            if (fadeDirection == FadeDirection.Out)
            {
                while (alpha >= fadeEndValue)
                {
                    SetColorImage(ref alpha, fadeDirection);
                    yield return null;
                }
                fadeOutUIImage.enabled = false;
                fadeOutLoadingImage.enabled = false;
            }
            else
            {
                fadeOutUIImage.enabled = true;
                fadeOutLoadingImage.enabled = true;
                while (alpha <= fadeEndValue)
                {
                    SetColorImage(ref alpha, fadeDirection);
                    yield return null;
                }
            }
        }

        private IEnumerator FadeAndExec(FadeDirection fadeDirection, Action onTransitionComplete)
        {
            yield return DoFade(fadeDirection);
            onTransitionComplete?.Invoke();
        }

        private IEnumerator FadeInExecAndOut(Action onMidTransition = null, Action onTransitionComplete = null)
        {
            yield return DoFade(FadeDirection.In);
            onMidTransition?.Invoke();
            yield return DoFade(FadeDirection.Out);
            onTransitionComplete?.Invoke();
        }

        public void Fade(FadeDirection fadeDirection, Action onTransitionComplete = null) =>
            StartCoroutine(FadeAndExec(fadeDirection, onTransitionComplete));

        public void FadeIn(Action onTransitionComplete = null) =>
            StartCoroutine(FadeAndExec(FadeDirection.In, onTransitionComplete));

        public void FadeOut(Action onTransitionComplete = null) =>
            StartCoroutine(FadeAndExec(FadeDirection.Out, onTransitionComplete));

        public void FadeInOut(Action onMidTransition = null, Action onTransitionComplete = null) =>
            StartCoroutine(FadeInExecAndOut(onMidTransition, onTransitionComplete));
    }
}