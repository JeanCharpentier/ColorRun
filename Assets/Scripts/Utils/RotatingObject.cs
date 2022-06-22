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
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentsInChildren<MeshFilter>()[0].transform.Rotate(rotX,rotY,rotZ);
    }
}
