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

    private void Start()
    {
        tgl_ColorBlind.isOn = CF._CB;

        Application.targetFrameRate = 60;

        if (PlayerPrefs.HasKey("score"))
        {
            transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("score").ToString());
        }

        if(PlayerPrefs.HasKey("playerName"))
        {
            // Si le joueur s'est d�j� enregistr�
            EnableCanvas(0);
            transform.GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>().SetText(PlayerPrefs.GetString("playerName"));
        }else
        {
            EnableCanvas(3);
        }
    }
    private void OnEnable()
    {
        transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("score").ToString());
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
        string year = CF.DateToInt('y').ToString().Substring(CF.DateToInt('y').ToString().Length - 2); // Formattage de l'ann�e pour cr�er la cl� Player
        string playerKey = "#" + CF.DateToInt('n').ToString() + year; // Cr�ation de la cl� Player
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
