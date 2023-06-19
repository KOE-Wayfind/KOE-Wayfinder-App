using System;
using UnityEngine;

/// <summary>
/// Target name must matches the [navigationTargets] in the MainMenuManager
/// </summary>
[Serializable]
public class Target
{
    public string name;
    public Vector3 position;
    
    public Target(string name, Vector3 position)
    {
        this.name = name;
        this.position = position;
    }
}