//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,IGameManager
{
    // Start is called before the first frame update
    [SerializeField]
    float _speed;
    [SerializeField]
    float _incSpeed;
    [SerializeField]
    [Range(1,3)]
    int _lifes;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (tTimer >= tTimerRate)
        {
            _speed = IncreaseSpeed();
            srvMManager.ChangeSpeed(_speed);
            tTimer = 0;
            Debug.LogWarning("Speed = " + _speed);
        }
        tTimer += Time.deltaTime;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float pSpeed)
    {
        _speed += pSpeed;
    }

    public float IncreaseSpeed()
    {
        SetSpeed(_incSpeed);
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

    public void SetLifes(int pLifes)
    {
        _lifes = pLifes;
        srvHUD.ChangeSprite(_lifes);
    }
}
