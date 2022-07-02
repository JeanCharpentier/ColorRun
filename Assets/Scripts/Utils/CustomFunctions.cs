using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CF
{
    public static Color _colPrimary { get; private set; } = new Color32(230, 0, 126, 255);
    public static Color _colSecondary { get; private set; } = new Color32(0, 130, 230, 255);
    public static Color _colTertiary { get; private set; } = new Color32(255, 255, 0, 255);
    public static Color[] _colList { get; private set; } = { _colPrimary, _colSecondary };

    public static bool _CB { get; private set; } = false;

    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static void ColorBlind(bool pBool)
    {
        _CB = pBool;
        if (pBool)
        {
            _colList[0] = _colTertiary;
            _colList[1] = _colSecondary;
        }else
        {
            _colList[0] = _colPrimary;
            _colList[1] = _colSecondary;
        }
    }
    public static int DateToInt(char pType)
    {
        int date;
        switch (pType)
        {
            case 'd':
                date = DateTime.Now.Day;
                break;
            case 'm':
                date = DateTime.Now.Month;
                break;
            case 'y':
                date = DateTime.Now.Year;
                break;
            case 'n':
                date = DateTime.Now.DayOfYear;
                break;
            default:
                date = DateTime.Now.DayOfYear;
                break;

        }
        return date;
    }
}
