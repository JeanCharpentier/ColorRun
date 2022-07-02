using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    int _score;
    string _name;
    private void Awake()
    {
        ServicesLocator.AddService<IScoreManager>(this);
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
        //Debug.Log("saving score...");
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
