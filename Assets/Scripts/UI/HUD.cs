using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IHUD
{
    [SerializeField]
    Sprite[] _lifesList;

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
    }

    private void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().SetText(Mathf.Ceil(fps).ToString());
        if(fps > _maxFPS)
        {
            _maxFPS = fps;
            //Debug.LogWarning("_maxFPS FPS :" + _maxFPS);
        }
    }
    public void ChangeSprite(int pLifes)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = _lifesList[srvGManager.GetLifes()];
    }

    public void UpdateScore(int pScore)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(pScore.ToString());
    }

    public void UpdateSpeed(float pSpeed)
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(pSpeed.ToString("F1"));
    }
}
