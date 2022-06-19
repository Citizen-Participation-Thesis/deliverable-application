using System;
using System.Collections.Generic;
using UnityEngine;

public class Services : Singleton<Services>
{
    private static readonly Dictionary<Type, GameObject> Registry = new Dictionary<Type, GameObject>();
    
    public static T Get<T>() where T : Component
    {
        if (Registry.TryGetValue(typeof(T), out var instance))
        {
            return instance.GetComponent<T>();
        }

        var existingInstance = FindObjectOfType<T>();

        if (existingInstance != null)
        {
            Registry.Add(typeof(T), existingInstance.gameObject);
            return existingInstance;
        }

        var newGameObject = new GameObject(typeof(T).Name);
        var newInstance = newGameObject.AddComponent<T>();
        newGameObject.transform.SetParent(Instance.transform);
        
        Registry.Add(typeof(T), newGameObject);

        return newInstance;
    }

    // Just in case
    public static void Register<T>(GameObject gameObject) where T : Component
    {
        if (!Registry.TryGetValue(typeof(T), out _))
        {
            Registry.Add(typeof(T), gameObject);
        }
    }

    public static void Deregister<T>() where T : Component
    {
        if (Registry.TryGetValue(typeof(T), out _))
        {
            Registry.Remove(typeof(T));
        }
    }
}