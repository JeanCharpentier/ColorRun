using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatform
{
    [HideInInspector]
    public float _speed;
    public int _length;
    Vector3 _pos;
    public bool _isTP;
    public int _state;

    MeshRenderer rend;
    float opacity;
    IPlatformManager srvPManager;

    void Awake()
    {
        ServicesLocator.AddService<IPlatform>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        srvPManager = ServicesLocator.GetService<IPlatformManager>();
        _isTP = false;
    }

    // Update is called once per frame
    void Update()
    {
        _pos = Vector3.left * _speed * Time.deltaTime;
        transform.position += _pos;

        for (int i = 0; i <= _length - 1; i++)
        {
            rend = GetComponentsInChildren<MeshFilter>()[i*2].GetComponent<MeshRenderer>();
            rend.materials[1].shader = Shader.Find("Shader Graphs/SG_Glow");
            if (rend.materials[1].GetFloat("_Alpha") < 1.0f)
            {
                //float dist = Vector3.Distance(other.position, transform.position);
                opacity = transform.position.x * (transform.position.x / 100);
                Debug.Log(opacity);
                rend.materials[1].SetFloat("_Alpha", opacity);
            }
        }

        // Remise dans le sac avant tirage
        if (transform.position.x < (-4) && !_isTP)
        {
            srvPManager.ResetPlatform(this);
            _isTP = true;
        }
    }

    public int GetState()
    {
        return _state;
    }
}