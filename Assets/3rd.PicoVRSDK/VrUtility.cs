using UnityEngine;
using System.Collections;

public static class VrUtility
{
    public static bool IsVR { get; private set; }
    public static bool IsPicoVR { get; private set; }
    static VrUtility()
    {
        IsVR = true || Application.platform == RuntimePlatform.Android;
        IsPicoVR = true || SystemInfo.deviceModel.Contains("Pico");
    }
}
