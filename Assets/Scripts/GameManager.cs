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
    [SerializeField]
    [Range(1,3)]
    int _lifes;

    IHUD srvHUD;
    void Awake()
    {
        ServicesLocator.AddService<IGameManager>(this);
        Random.InitState(PlayerPrefs.GetInt("seed"));
    }
    void Start()
    {
        srvHUD = ServicesLocator.GetService<IHUD>();
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

    public int GetLifes()
    {
        return _lifes;
    }

    public void SetLifes(int pLifes)
    {
        _lifes = pLifes;
        srvHUD.ChangeSprite(_lifes);
    }
}
