using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour,IGOMenu
{

    [SerializeField]
    TextMeshProUGUI _continues;
    IGameManager srvGManager;


    private void Awake()
    {
        ServicesLocator.AddService<IGOMenu>(this);
    }
    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
        _continues = transform.GetChild(5).GetComponent<TextMeshProUGUI>();

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
        srvGManager.ResetGame();
        this.GetComponent<Canvas>().enabled = false;
    }

    public void ChangeContinues(int pContinues)
    {
        //transform.GetChild(5).GetComponent<TextMeshProUGUI>().SetText(pContinues.ToString());
        _continues.SetText(pContinues.ToString());
    }
}
