//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
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

    int _quality;
    PostProcessVolume _ppVolume;
    Bloom _bloomVolume;

    // Audio
    AudioSource _srcAudio;
    AudioViz _vizAudio;
    float _volAudio;


    // Timer Augmentation vitesse
    float _speedTimer;
    [SerializeField]
    float _speedTimerRate;

    // Timer Augmentation Score
    float _scoreTimer;
    [SerializeField]
    float _scoreTimerRate;

    IHUD srvHUD;
    IPlatformManager srvPManager;
    IBackgroundManager srvBGManager;
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
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
        srvBGManager = ServicesLocator.GetService<IBackgroundManager>();
        srvPlayer = ServicesLocator.GetService<IPlayer>();
        srvGOMenu = ServicesLocator.GetService<IGOMenu>();
        srvSManager = ServicesLocator.GetService<IScoreManager>();

        _score = 0;
        _continues = 3;
        _speed = _baseSpeed;
        _lifes = _baseLifes;
        _gameMode = PlayerPrefs.GetInt("mode");

        srvHUD.UpdateSpeed(_speed);
        srvPlayer.ChangeSpeed(_speed);

        Time.timeScale = 1; // On remet le temps "en route"

        _quality = PlayerPrefs.GetInt("quality");


        _srcAudio = GetComponent<AudioSource>();
        _vizAudio = GetComponent<AudioViz>();
        _volAudio = PlayerPrefs.GetFloat("volume");

        if(_volAudio <= 0f)
        {
            _srcAudio.enabled = false;
            _vizAudio.enabled = false;
        }else
        {
            AudioListener.volume = _volAudio;
        }
        
        _ppVolume = Camera.main.GetComponent<PostProcessVolume>();

        if(_quality == 0)
        {
            _ppVolume.enabled = false;
        }else
        {
            _ppVolume.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_speedTimer >= _speedTimerRate) // Timer augmentation vitesse
        {
            _speed = IncreaseSpeed();
            srvPlayer.ChangeSpeed(_speed);
            srvHUD.UpdateSpeed(_speed);
            _speedTimer = 0;
        }
        _speedTimer += Time.deltaTime;
    }

    public void ChangeBloom(float pBloom)
    {
        _ppVolume.profile.TryGetSettings(out _bloomVolume);
        if(_bloomVolume != null)
        {
            _bloomVolume.intensity.value = pBloom;
        }
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float pSpeed, bool pReset)
    {
        if(pReset)
        {
            srvPlayer.ChangeSpeed(_speed);
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
        Time.timeScale = 1;
        if (_continues > 0)
        {
            
            srvPManager.ReplayGame();
            srvBGManager.ReplayGame();
            SetLifes(0, true);
            SetSpeed(0.0f, true);

            srvPlayer.ResetPlayer();//SceneManager.LoadScene("MainScene");

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
}
