using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

public static class SerializableDictionaryExtensions
{
    public static TValue GetSafe<TKey, TValue>(this SerializableDictionary<TKey, TValue> dict, TKey key)
    {
        if (dict == null)
        {
            Debug.LogError("SerializableDictionary is null.");
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

    public static void SetSafe<TKey, TValue>(this SerializableDictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict == null)
        {
            Debug.LogError("SerializableDictionary is null.");
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

    public static void RemoveSafe<TKey, TValue>(this SerializableDictionary<TKey, TValue> dict, TKey key)
    {
        if (dict == null)
        {
            Debug.LogError("SerializableDictionary is null.");
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