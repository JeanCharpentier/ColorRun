using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour
{
    IGameManager srvGManager;

    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GetContinue()
    {
        // Lancer une pub ?
    }

    public void Retry()
    {
        this.GetComponent<Canvas>().enabled = false;
        srvGManager.SetLifes(0, true);
        srvGManager.SetSpeed(0.0f, true);
        Time.timeScale = 1;

        // Decrease COntinues
    }
}
