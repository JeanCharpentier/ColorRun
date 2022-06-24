using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IHUD
{
    [SerializeField]
    Sprite[] _lifesList;
    IGameManager srvGManager;
    private void Awake()
    {
        ServicesLocator.AddService<IHUD>(this);
    }
    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
    }
    public void ChangeSprite(int pLifes)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = _lifesList[srvGManager.GetLifes()];
    }
}
