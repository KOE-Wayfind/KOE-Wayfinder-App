using System;
using UnityEngine;

/// <summary>
/// Target name must matches the [navigationTargets] in the MainMenuManager
/// </summary>
[Serializable]
public class Target
{
    public string name;
    public GameObject positionObject;
}