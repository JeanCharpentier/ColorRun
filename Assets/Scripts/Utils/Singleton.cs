﻿using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }
                else
                {
                    Debug.LogError("Trop de " + typeof(T).Name + " dans la scène !");
                }
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    public virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }else
        {
            Destroy(gameObject);
        }
    }
}

