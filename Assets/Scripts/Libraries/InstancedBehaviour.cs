using System;
using UnityEngine;

/// <summary>
/// Main class that can be used to create Instanced Mono Behaviours instead of always adding the following block of code to every Behaviour we want to be instanced.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InstancedBehaviour<T>:MonoBehaviour where T:MonoBehaviour
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
    }
}