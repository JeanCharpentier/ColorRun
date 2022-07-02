using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour,IGOMenu
{
    IGameManager srvGManager;


    private void Awake()
    {
        ServicesLocator.AddService<IGOMenu>(this);
    }
    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
    }
    public void BackMenu()
    {
        srvGManager.SaveScore();
        SceneManager.LoadScene("MainMenu");
    }

    public void GetMoreContinue()
    {
        // Lancer une pub ?
        srvGManager.FillContinues();
        //srvGManager.SaveScore();
    }

    public void Retry()
    {
        this.GetComponent<Canvas>().enabled = false;
        srvGManager.ResetGame();
        Time.timeScale = 1;
        //srvGManager.SaveScore();
    }

    public void ChangeContinues(int pContinues)
    {
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().SetText(pContinues.ToString());
    }
}
