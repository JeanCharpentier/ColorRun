using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    int _score;
    string _name;

    float _scoreTimer;
    [SerializeField]
    float _scoreTimerRate;

    IHUD srvHUD;
    IGameManager srvGManager;
    private void Awake()
    {
        ServicesLocator.AddService<IScoreManager>(this);
    }

    private void Start()
    {
        srvHUD = ServicesLocator.GetService<IHUD>();
        srvGManager = ServicesLocator.GetService<IGameManager>();
    }
    private void Update()
    {
        if(Time.timeScale == 1)
        {
            if (_scoreTimer >= _scoreTimerRate / srvGManager.GetSpeed() / 3)
            {
                _score += 1;
                srvHUD.UpdateScore(_score);
                _scoreTimer = 0;
            }
            _scoreTimer += Time.deltaTime;
        }
        
    }

    public void BonusScore(int pBonus)
    {
        _score += pBonus;
    }
    public void SaveScore()
    {
        if (PlayerPrefs.GetInt("mode") == 1) // On upgrade le Highscore seulement si le mode est "normal"
        {
            if (_score > PlayerPrefs.GetInt("score") || !PlayerPrefs.HasKey("score"))
            {
                PlayerPrefs.SetInt("score", _score);
                SetHighscore(_score, PlayerPrefs.GetString("playerName")); // Upload to Leaderboard 
            }
        }
    }

    public void SetHighscore(int pScore, string pPlayerName)
    {
        _score = pScore;
        _name = pPlayerName;
        StartCoroutine(UploadHighscore());
    }
    IEnumerator UploadHighscore()
    {
        WWWForm posts = new WWWForm();
        posts.AddField("player", _name);
        posts.AddField("score", _score.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://outofreality.org/Games/highway_roller/insert.php", posts);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
