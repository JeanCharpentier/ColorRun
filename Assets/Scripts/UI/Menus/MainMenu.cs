using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    int _seed;
    [SerializeField]
    Toggle tgl_ColorBlind;
    [SerializeField]
    Toggle tgl_Language;

    private void Start()
    {
        tgl_ColorBlind.isOn = CF._CB;
    }

    public void PlayMenu()
    {
        transform.GetChild(0).GetComponentInChildren<Canvas>().enabled = false;
        transform.GetChild(1).GetComponentInChildren<Canvas>().enabled = true;
        transform.GetChild(2).GetComponentInChildren<Canvas>().enabled = false;
    }

    public void OptionsMenu()
    {
        transform.GetChild(0).GetComponentInChildren<Canvas>().enabled = false;
        transform.GetChild(1).GetComponentInChildren<Canvas>().enabled = false;
        transform.GetChild(2).GetComponentInChildren<Canvas>().enabled = true;
    }

    public void BackMenu()
    {
        transform.GetChild(0).GetComponentInChildren<Canvas>().enabled = true;
        transform.GetChild(1).GetComponentInChildren<Canvas>().enabled = false;
        transform.GetChild(2).GetComponentInChildren<Canvas>().enabled = false;
    }

    public void SetColorBlind()
    {
        if(tgl_ColorBlind.isOn)
        {
            CF.ColorBlind(true);
            
        }else
        {
            CF.ColorBlind(false);
        }
    }
    public void PlayNormal()
    {
        SetSeed(_seed);
        SceneManager.LoadScene("MainScene");
    }

    public void PlayDaily()
    {
        SetSeed(CF.DateToInt());
        SceneManager.LoadScene("MainScene");
    }

    public void PlayRandom()
    {
        SetSeed(Random.Range(400, 9000));
        SceneManager.LoadScene("MainScene");
    }

    private void SetSeed(int pSeed)
    {
        PlayerPrefs.SetInt("seed", pSeed);
    }

    public void SwitchLanguage()
    {
        if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            PlayerPrefs.SetInt("locale", 1);
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            PlayerPrefs.SetInt("locale", 0);
        }
    }
}
