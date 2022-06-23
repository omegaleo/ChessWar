using System;
using UnityEngine;

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