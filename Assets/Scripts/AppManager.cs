using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AppManager : MonoBehaviour
{
    private void Start()
    {
        // Disable screen lock
        // ref: https://docs.unity3d.com/ScriptReference/Screen-sleepTimeout.html
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}