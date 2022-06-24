//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,IGameManager
{
    // Start is called before the first frame update
    [SerializeField]
    float _baseSpeed;
    float _speed;
    [SerializeField]
    float _incSpeed;
    [SerializeField]
    [Range(1, 3)]
    int _baseLifes;
    int _lifes;
    int _score;
    int _continues;

    float tTimer;
    [SerializeField]
    float tTimerRate;

    IHUD srvHUD;
    IMovingManager srvMManager;
    void Awake()
    {
        ServicesLocator.AddService<IGameManager>(this);
        Random.InitState(PlayerPrefs.GetInt("seed"));
    }
    void Start()
    {
        srvHUD = ServicesLocator.GetService<IHUD>();
        srvMManager = ServicesLocator.GetService<IMovingManager>();
        _score = 0;
        _continues = 3;
        _speed = _baseSpeed;
        _lifes = _baseLifes;
    }

    // Update is called once per frame
    void Update()
    {
        if (tTimer >= tTimerRate)
        {
            _speed = IncreaseSpeed();
            srvMManager.ChangeSpeed(_speed);
            tTimer = 0;
            //Debug.LogWarning("Speed = " + _speed);
        }
        tTimer += Time.deltaTime;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float pSpeed, bool pReset)
    {
        if(pReset)
        {
            _speed = _baseSpeed;
            srvMManager.ChangeSpeed(_speed);
        }
        else
        {
            _speed += pSpeed;
        }
    }

    public float IncreaseSpeed()
    {
        SetSpeed(_incSpeed,false);
        return _speed;
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

    public void SetLifes(int pLifes, bool pReset)
    {
        if(pReset)
        {
            _lifes = _baseLifes;
        }else
        {
            _lifes = pLifes;
        }
        srvHUD.ChangeSprite(_lifes);
    }
}
