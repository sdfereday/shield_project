using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace Game.Toolbox.Effects
{
    public class SceneCurtain : MonoBehaviour
    {
        public RawImage fadeOutUIImage;
        public float fadeSpeed = 0.8f;
        public enum FadeDirection
        {
            In, // Alpha = 1
            Out // Alpha = 0
        }

        private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
        {
            fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
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
            }
            else
            {
                fadeOutUIImage.enabled = true;
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