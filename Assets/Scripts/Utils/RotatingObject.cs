using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField]
    float rotX;
    [SerializeField]
    float rotY;
    [SerializeField]
    float rotZ;

    Transform _transform;
    private void Start()
    {
        _transform = GetComponentsInChildren<MeshFilter>()[0].transform;
    }
    void Update()
    {
        _transform.Rotate(rotX,rotY,rotZ);
    }
}
