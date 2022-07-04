using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [HideInInspector]
    public float _speed;
    public int _length;
    public bool isTP;

    IBackgroundManager srvBGManager;
    IPlayer srvPlayer;
    void Awake()
    {
        isTP = false;
    }
    void Start()
    {
        srvBGManager = ServicesLocator.GetService<IBackgroundManager>();
        srvPlayer = ServicesLocator.GetService<IPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Remise dans le sac avant tirage
        if(!isTP)
        {
            if (transform.position.x < srvPlayer.GetPos().x + 2)
            {
                srvBGManager.ResetPlatform(this);
                isTP = true;
            }
        }
    }
    public void ResetThisPlatform()
    {
        isTP = false;
    }
}