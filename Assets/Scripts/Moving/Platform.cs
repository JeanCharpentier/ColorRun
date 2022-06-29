using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    [HideInInspector]
    public float _speed;
    public int _length;
    Vector3 _pos;
    public bool isTP;
    public int _state;

    MeshRenderer rend;
    float _opacity;
    IPlatformManager srvPManager;
    void Awake()
    {
        ServicesLocator.AddService<IPlatform>(this);
        isTP = false;
    }
    void Start()
    {
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _pos = Vector3.left * _speed * Time.deltaTime;
        transform.position += _pos;

        /*for (int i = 0; i <= _length - 1; i++) // Variation d'Alpha pour simuler apparition de l'Emissive dans le Fog
        {
            rend = GetComponentsInChildren<MeshFilter>()[i*2].GetComponent<MeshRenderer>();
            rend.materials[1].shader = Shader.Find("Shader Graphs/SG_Glow");
            if (rend.materials[1].GetFloat("_Alpha") < 1.0f)
            {
                float dist = Vector3.Distance(new Vector3(0,0,0), transform.position);
                _opacity = 1 - (dist/100);
                Debug.Log("Opacity Dist :" + _opacity);
                rend.materials[1].SetFloat("_Alpha", _opacity);
            }
        }*/

        // Remise dans le sac avant tirage
        if (transform.position.x < (-4) && !isTP)
        {
            srvPManager.ResetPlatform(this);
            isTP = true;
        }
    }

    public int GetState()
    {
        return _state;
    }

    public void ChangeSpeed(float pSpeed)
    {
        _speed = pSpeed;
    }

    public void Kill()
    {
        Destroy(gameObject);
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