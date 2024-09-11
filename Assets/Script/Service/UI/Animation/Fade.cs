using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float time;
    public bool isNeedTrigger;

    [Space(20)]
    [SerializeField] FadeType fadeType;

    private CanvasGroup canvas;
    private Coroutine update;
    private float t;
    void OnEnable()
    {
        if (!isNeedTrigger)
            Trigger();
    }

    public void TweenAlpha()
    {
        if (fadeType == FadeType.FADE_IN)
        {
            t += Time.fixedDeltaTime / time;
            float newAlpha = Mathf.Lerp(0, 1, t);
            canvas.alpha = newAlpha;
            if (t >= 1 && update != null)
                Clear();
        }
        else
        {
            t += Time.fixedDeltaTime / time;
            float newAlpha = Mathf.Lerp(1, 0, t);
            canvas.alpha = newAlpha;
            if (t >= 1 && update != null)
                Clear();
        }
    }

    public void Trigger()
    {
        if (canvas == null)
            canvas = gameObject.AddComponent<CanvasGroup>();

        if (fadeType == FadeType.FADE_IN)
            canvas.alpha = 0;
        else
            canvas.alpha = 1;
        t = 0;
        update = MainService.UpdateMedthod(TweenAlpha);
    }

    public void Clear()
    {
        t = 0;
        StopCoroutine(update);
    }
}

[Serializable]
public class FadeSriptable
{
    public FadeType value;
}

[Serializable]
public enum FadeType
{ 
    FADE_IN,
    FADE_OUT
}