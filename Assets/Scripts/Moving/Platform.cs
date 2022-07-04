using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    public int _length;
    public bool isTP;
    public int _state;

    Color _colSupport;
    public Color _colBorder { private get; set; }

    IPlatformManager srvPManager;
    IPlayer srvPlayer;
    void Awake()
    {
        ServicesLocator.AddService<IPlatform>(this);
        
    }
    void Start()
    {
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
        srvPlayer = ServicesLocator.GetService<IPlayer>();

        isTP = false;

        _colSupport = GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.grey;
        _colBorder = GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Remise dans le sac avant tirage
        if(!isTP)
        {
            if (transform.position.x < srvPlayer.GetPos().x - 5)
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
    public void ResetThisPlatform(float pOffset)
    {

        _state = 0;
        transform.position = new Vector3(pOffset, 0, 0);
        for (int i = 0; i <= _length - 1; i++)
        {
            GetComponentsInChildren<MeshFilter>()[i * 2].GetComponent<MeshRenderer>().materials[1].color = CF._colList[0];
        }
        _colSupport = Color.grey;
    }

    public void ChangeColor()
    {
        throw new System.NotImplementedException();
    }
}