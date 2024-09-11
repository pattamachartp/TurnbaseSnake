using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private Action callback;
    [Header("Scale")]
    public float time;

    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f), new Keyframe(1f, 1f, 1f, 0f));

    private float t;
    private float newScale;
    private float animationTime;
    private Coroutine update;

    [SerializeField] UIAnimationType UIAnimationType;
    [SerializeField] AnimationSpeedType AnimationSpeedType;

    public void PlayAnimation(Action callback)
    {
        this.callback = callback;
        if (time > 0)
        {
            newScale = animationCurve.Evaluate(t);
            transform.localScale = new Vector3(newScale, newScale, 1);
            update = MainService.UpdateMedthod(TweenScale);
            animationTime = animationCurve[animationCurve.length - 1].time;
        }
        if (timeFade > 0)
        {
            if (canvas == null)
                canvas = gameObject.AddComponent<CanvasGroup>();
            if (fadeType.value == FadeType.FADE_IN)
                canvas.alpha = 0;
            else
                canvas.alpha = 1;
            t = 0;
            update = MainService.UpdateMedthod(TweenAlpha);
        }
    }

    public void TweenScale()
    {
        t += Time.fixedDeltaTime;

        newScale = animationCurve.Evaluate(t);
        transform.localScale = new Vector3(newScale, newScale, 1);
        if (update != null && t >= animationTime)
            Clear();
    }

    [Header("Fade")]
    public float timeFade;
    public FadeSriptable fadeType;

    private CanvasGroup canvas;

    public void TweenAlpha()
    {
        if (fadeType.value == FadeType.FADE_IN)
        {
            t += Time.fixedDeltaTime / timeFade;
            float newAlpha = Mathf.Lerp(0, 1, t);
            canvas.alpha = newAlpha;
            if (t >= 1 && update != null)
                Clear();
        }
        else
        {
            t += Time.fixedDeltaTime / timeFade;
            float newAlpha = Mathf.Lerp(1, 0, t);
            canvas.alpha = newAlpha;
            if (t >= 1 && update != null)
                Clear();
        }
    }

    public void Clear()
    {
        Debug.LogError("stop");
        t = 0;
        newScale = 0;
        StopCoroutine(update);
        if (callback != null)
        {
            callback.Invoke();
            callback = null;
        }
    }

}


[Serializable]
public enum UIAnimationType
{
    NONE,
    FADE_IN,
    FADE_OUT,
    SCALE_UP,
    SCALE_UP_BIGGER,
    SCALE_DOWN,
    MOVE
}

[Serializable]
public enum AnimationSpeedType
{
    LINEAR,
    FAST_START,
    SLOW_START
}