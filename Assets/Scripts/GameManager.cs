//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,IGameManager
{
    // Start is called before the first frame update
    [SerializeField]
    float _currentSpeed;
    [SerializeField]
    float _incSpeed;

    void Awake()
    {
        ServicesLocator.AddService<IGameManager>(this);
        Random.InitState(PlayerPrefs.GetInt("seed"));
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetSpeed()
    {
        return _currentSpeed;
    }

    public void SetSpeed(float pSpeed)
    {
        _currentSpeed += pSpeed;
    }

    public float IncreaseSpeed()
    {
        SetSpeed(_incSpeed);
        return _currentSpeed;
    }

    public float DecreaseSpeed()
    {
        throw new System.NotImplementedException();
        //return _currentSpeed;
    }
}
