using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    [HideInInspector]
    public float _speed;
    public int _length;
    public bool isTP;
    public int _state;

    IPlatformManager srvPManager;
    IPlayer srvPlayer;
    void Awake()
    {
        ServicesLocator.AddService<IPlatform>(this);
        isTP = false;
    }
    void Start()
    {
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
        srvPlayer = ServicesLocator.GetService<IPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Remise dans le sac avant tirage
        if(!isTP)
        {
            if (transform.position.x < srvPlayer.GetPos().x - 10)
            {
                srvPManager.ResetPlatform(this);
                isTP = true;
            }
        }
    }
    public int GetState()
    {
        return _state;
    }
    public void ResetThisPlatform()
    {
        _state = 0;
        isTP = false;
        for (int i = 0; i <= _length - 1; i++)
        {
            GetComponentsInChildren<MeshFilter>()[i * 2].GetComponent<MeshRenderer>().materials[1].color = CF._colList[0];
        }
        GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.grey;
    }
}