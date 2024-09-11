using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainService : MonoBehaviour
{
    public static MainService _instance;

    private int gameSlot = 0;

    public void Awake()
    {
        _instance = this;
    }

  
    IEnumerator _InvokeDelegate(Action aAction, float aDelay)
    {
        yield return new WaitForSeconds(aDelay); aAction();
    }

    public static Coroutine InvokeDelegate(float aDelay, Action aAction)
    {
        return _instance.StartCoroutine(_instance._InvokeDelegate(aAction, aDelay));
    }
    public static void StopDelegate(Coroutine corotine)
    {
        _instance.StopCoroutine(corotine);
    }

    public static Coroutine UpdateMedthod(Action medthod)
    {
        return _instance.StartCoroutine(_instance.UpdateMedthodCoroutine(medthod));
    }

    IEnumerator UpdateMedthodCoroutine(Action medthod)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            medthod();
        }
    }

    public static Coroutine UpdateMedthod(Action medthod, float time)
    {
        return _instance.StartCoroutine(_instance.UpdateMedthodCoroutine(medthod, time));
    }

    IEnumerator UpdateMedthodCoroutine(Action medthod, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            medthod();
        }
    }



    public static Coroutine UpdateMedthodTimer(Action medthod, float time, Action done = null)
    {
        return _instance.StartCoroutine(_instance.UpdateMedthodTimerCoroutine(medthod, time, done));
    }

    IEnumerator UpdateMedthodTimerCoroutine(Action medthod, float time, Action done)
    {
        bool isDone = false;
        InvokeDelegate(time, () => { isDone = true; });
        while (!isDone)
        {
            yield return new WaitForFixedUpdate();
            medthod();
        }
        if (done != null)
            done.Invoke();
    }
}


public static class DictionaryExtensions
{
    public static TValue GetSafe<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        if (dict == null)
        {
            Debug.LogError("Dictionary is null.");
            return default;
        }

        if (dict.TryGetValue(key, out TValue value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"Key '{key}' not found in dictionary.");
            return default;
        }
    }

    public static void SetSafe<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict == null)
        {
            Debug.LogError("Dictionary is null.");
            return;
        }

        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }

    public static void RemoveSafe<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        if (dict == null)
        {
            Debug.LogError("Dictionary is null.");
            return;
        }

        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
        }
        else
        {
            Debug.LogError($"Key '{key}' not found. Cannot remove.");
        }
    }
}


public static class VariableExtensions
{
    public static int ToInit(this float value)
    {
        return Mathf.RoundToInt(value); 
    }
}