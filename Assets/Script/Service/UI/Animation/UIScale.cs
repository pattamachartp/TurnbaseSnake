using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScale : MonoBehaviour
{
    public float time;
    public bool isNeedTrigger;

    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f), new Keyframe(1f, 1f, 1f, 0f));

    private float t;
    private float newScale;
    private float animationTime;
    private Coroutine update;

    void OnEnable()
    {
        newScale = animationCurve.Evaluate(t);
        transform.localScale = new Vector3(newScale, newScale, 1);
        animationTime = animationCurve[animationCurve.length - 1].time;
        if (!isNeedTrigger)
            update= MainService.UpdateMedthod(TweenScale);

    }

  /*  public void TweenScale()
    {
        t += Time.fixedDeltaTime / time;
        newScale = Mathf.Lerp(0, 1, t);
        float  newScale2 = animationCurve.Evaluate(newScale);
        transform.localScale = new Vector3(newScale2, newScale2, 1);
        if (t >= 1 && update != null)
            StopCoroutine(update);
    }*/

    public void TweenScale()
    {
        t += Time.fixedDeltaTime;

        newScale = animationCurve.Evaluate(t);
        transform.localScale = new Vector3(newScale, newScale, 1);
        if ( update != null && t >= animationTime)
            Clear();
    }

    public void Trigger()
    {
        update = MainService.UpdateMedthod(TweenScale);
    }

    public void Clear()
    {
        t = 0;
        newScale = 0; 
        StopCoroutine(update);
    }
}
