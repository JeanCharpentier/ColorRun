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
    public static Color[] _colList { get; private set; } = { _colPrimary, _colSecondary };

    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }
}
