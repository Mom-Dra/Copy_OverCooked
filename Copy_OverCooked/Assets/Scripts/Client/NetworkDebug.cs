using UnityEngine;
using System;

public static class NetworkDebug
{
    public static void Log(string message)
    {
        Debug.Log($"<color=yellow>{message}</color>");
    }

}