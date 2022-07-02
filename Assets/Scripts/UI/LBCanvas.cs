using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LBCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject _line;
    
    UnityWebRequest www;
    string url;
    void Start()
    {
        url = "https://outofreality.org/Games/highway_roller/unityboard.php";
        
        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string _scores = www.downloadHandler.text;
            Debug.Log(_scores);


            JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data));

            if(jsonData == null)
            {
                Debug.LogWarning("No Data !");
            }else
            {
                int i = 0;
                foreach(JSONNode s in jsonData)
                {
                    Debug.Log("Name :" + s["Player"]);
                    Debug.Log("Score :" + s["HighScore"]);
                    
                    //GameObject _tmpLine = Instantiate(_line, new Vector3(gameObject.transform.position.x, 400 - (i * 40.0f), gameObject.transform.position.z), Quaternion.identity);
                    GameObject _tmpLine = Instantiate(_line, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
                    //_tmpLine.transform.parent = gameObject.transform;
                    _tmpLine.transform.SetParent(gameObject.transform);
                    _tmpLine.GetComponent<TextMeshProUGUI>().SetText(s["Player"] + " | " + s["HighScore"]);
                    i++;
                }
                
            }
            
        }
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
