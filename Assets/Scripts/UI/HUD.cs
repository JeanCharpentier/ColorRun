using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IHUD
{
    [SerializeField]
    Sprite[] _lifesList;

    TextMeshProUGUI _txtFPS;
    TextMeshProUGUI _txtScore;
    TextMeshProUGUI _txtSpeed;

    float _maxFPS;

    IGameManager srvGManager;
    private void Awake()
    {
        ServicesLocator.AddService<IHUD>(this);
    }
    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
        _maxFPS = 0f;
        _txtFPS = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        _txtScore = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _txtSpeed = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        _txtFPS.SetText(Mathf.Ceil(fps).ToString());
        if(fps > _maxFPS)
        {
            _maxFPS = fps;
            //Debug.LogWarning("_maxFPS FPS :" + _maxFPS);
        }
    }
    public void ChangeSprite(int pLifes)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = _lifesList[pLifes];
    }

    public void UpdateScore(int pScore)
    {
        _txtScore.SetText(pScore.ToString());
    }

    public void UpdateSpeed(float pSpeed)
    {
        _txtSpeed.SetText(pSpeed.ToString("F1"));
    }
}
