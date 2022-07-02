using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    Toggle tgl_Quality;
    [SerializeField]
    Toggle tgl_Vibration;

    private void Start()
    {
        tgl_ColorBlind.isOn = CF._CB;

        Application.targetFrameRate = 600;

        if (PlayerPrefs.HasKey("score"))
        {
            transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("score").ToString());
        }

        if(PlayerPrefs.HasKey("playerName"))
        {
            // Si le joueur s'est déjà enregistré
            EnableCanvas(0);
            transform.GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>().SetText(PlayerPrefs.GetString("playerName"));
        }else
        {
            EnableCanvas(3);
        }

        SetToggle(tgl_Quality, "quality",1);
        SetToggle(tgl_ColorBlind, "colorblind",0);
        SetToggle(tgl_Vibration, "vibration",1);
    }
    private void OnEnable()
    {
        transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("score").ToString());
    }

    void SetToggle(Toggle pTgl,string pPref, int pVal) // Met a jour les boutons switch des options
    {
        if (!PlayerPrefs.HasKey(pPref))
        {
            PlayerPrefs.SetInt(pPref, pVal);
        }
        else
        {
            if (PlayerPrefs.GetInt(pPref) == 0)
            {
                pTgl.isOn = false;
            }
            else
            {
                pTgl.isOn = true;
            }
        }
    }
    void EnableCanvas(int pCanvas)
    {
        int _canvasCount;
        _canvasCount = transform.childCount;
        for(int i = 0;i<=_canvasCount-1;i++)
        {
            if(i == pCanvas)
            {
                transform.GetChild(i).GetComponentInChildren<Canvas>().enabled = true;
            }else
            {
                transform.GetChild(i).GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }
    public void PlayMenu()
    {
        EnableCanvas(1);
    }

    public void OptionsMenu()
    {
        EnableCanvas(2);
    }

    public void BackMenu()
    {
        EnableCanvas(0);
    }

    public void ControlsMenu()
    {
        EnableCanvas(4);
    }

    public void ShareMenu()
    {
        EnableCanvas(5);
    }

    public void Leaderboard()
    {
        SceneManager.LoadScene("MainLeaderboard");
    }

    public void SetColorBlind()
    {
        if(tgl_ColorBlind.isOn)
        {
            CF.ColorBlind(true);
            PlayerPrefs.SetInt("colorblind", 1);
        }else
        {
            CF.ColorBlind(false);
            PlayerPrefs.SetInt("colorblind", 0);
        }
    }

    public void SetQuality()
    {
        if(tgl_Quality.isOn)
        {
            PlayerPrefs.SetInt("quality", 1);
        }else
        {
            PlayerPrefs.SetInt("quality", 0);
        }
    }

    public void SetVibration()
    {
        if (tgl_Vibration.isOn)
        {
            PlayerPrefs.SetInt("vibration", 1);
        }
        else
        {
            PlayerPrefs.SetInt("vibration", 0);
        }
    }
    public void PlayNormal()
    {
        SetSeed(_seed);
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene("MainScene");
    }

    public void PlayDaily()
    {
        SetSeed(CF.DateToInt('n'));
        PlayerPrefs.SetInt("mode", 2);
        SceneManager.LoadScene("MainScene");
    }

    public void PlayRandom()
    {
        SetSeed(Random.Range(400, 9000));
        PlayerPrefs.SetInt("mode", 3);
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

    public void SavePlayerName()
    {
        string year = CF.DateToInt('y').ToString().Substring(CF.DateToInt('y').ToString().Length - 2); // Formattage de l'année pour créer la clé Player
        string playerKey = "#" + CF.DateToInt('n').ToString() + year; // Création de la clé Player
        string playerName = transform.GetChild(3).GetComponentInChildren<TMP_InputField>().text + playerKey;
        PlayerPrefs.SetString("playerName", playerName); // Enregistrement
        PlayerPrefs.Save();
        transform.GetChild(0).GetComponentInChildren<Canvas>().enabled = true;
        transform.GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>().SetText(playerName);

        transform.GetChild(3).GetComponentInChildren<Canvas>().enabled = false;
    }

    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("MainMenu");
    }
}
