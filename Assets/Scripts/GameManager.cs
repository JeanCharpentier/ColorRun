//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    int _gameMode; //1 = normal, 2 = daily, 3 = random


    // Timer Augmentation vitesse
    float _speedTimer;
    [SerializeField]
    float _speedTimerRate;

    // Timer Augmentation Score
    float _scoreTimer;
    [SerializeField]
    float _scoreTimerRate;

    IHUD srvHUD;
    IMovingManager srvMManager;
    IPlatformManager srvPManager;
    IPlayer srvPlayer;
    IGOMenu srvGOMenu;
    IScoreManager srvSManager;
    void Awake()
    {
        ServicesLocator.AddService<IGameManager>(this);
        Random.InitState(PlayerPrefs.GetInt("seed"));
    }
    void Start()
    {
        srvHUD = ServicesLocator.GetService<IHUD>();
        srvMManager = ServicesLocator.GetService<IMovingManager>();
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
        srvPlayer = ServicesLocator.GetService<IPlayer>();
        srvGOMenu = ServicesLocator.GetService<IGOMenu>();
        srvSManager = ServicesLocator.GetService<IScoreManager>();

        _score = 0;
        _continues = 3;
        _speed = _baseSpeed;
        _lifes = _baseLifes;
        _gameMode = PlayerPrefs.GetInt("mode");

        srvHUD.UpdateSpeed(_speed);

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_speedTimer >= _speedTimerRate) // Timer augmentation vitesse
        {
            _speed = IncreaseSpeed();
            srvMManager.ChangeSpeed(_speed);
            srvHUD.UpdateSpeed(_speed);
            _speedTimer = 0;
        }
        _speedTimer += Time.deltaTime;

        if(_scoreTimer >= _scoreTimerRate/_speed)
        {
            _score += 1;
            srvHUD.UpdateScore(_score);
            _scoreTimer = 0;
        }
        _scoreTimer += Time.deltaTime;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float pSpeed, bool pReset)
    {
        if(pReset)
        {
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

    public void ResetGame()
    {
        if(_continues > 0)
        {
            srvPManager.ReplayGame();
            SetLifes(0, true);
            SetSpeed(0.0f, true);
            srvPlayer.ResetPlayer();
            _continues--;
            
            srvGOMenu.ChangeContinues(_continues); // Changer l'affichage sur le GOMenu
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void FillContinues()
    {
        _continues++;
        srvGOMenu.ChangeContinues(_continues); // Changer l'affichage sur le GOMenu
    }

    public int GetScore()
    {
        return _score;
    }

    public void SaveScore()
    {
        if(PlayerPrefs.GetInt("mode") == 1) // On upgrade le Highscore seulement si le mode est "normal"
        {
            if(_score > PlayerPrefs.GetInt("score") || PlayerPrefs.HasKey("score"))
            {
                PlayerPrefs.SetInt("score", _score);
                //srvSManager.SetHighscore(_score, PlayerPrefs.GetString("playerName"));//Upload to Leaderboard 
            }
        }
        
    }
}
