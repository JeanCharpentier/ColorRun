using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scaler : MonoBehaviour
{
    float _resX;
    float _resY;

    CanvasScaler _can;

    private void Start()
    {
        _can = GetComponent<CanvasScaler>();
        SetInfo();
    }

    void SetInfo()
    {
        _resX = (float)Screen.currentResolution.width;
        _resY = (float)Screen.currentResolution.height;

        _can.referenceResolution = new Vector2(_resX, _resY);
        Debug.Log(_resX + ":" + _resY);
    }
}
