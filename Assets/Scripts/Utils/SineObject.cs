using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineObject : MonoBehaviour
{
    [SerializeField]
    float sinX;
    [SerializeField]
    float sinY;
    [SerializeField]
    float sinZ;

    Vector3 _basePosition;
    //Transform _transform;
    private void Start()
    {
        //_transform = GetComponentsInChildren<MeshFilter>()[0].transform;
    }
    void Update()
    {
        transform.position = _basePosition + new Vector3(0.0f, Mathf.Sin(Time.time), 0.0f);
    }
}
