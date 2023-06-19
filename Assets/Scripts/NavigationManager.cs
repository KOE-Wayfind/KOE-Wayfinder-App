using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store the data for navigation
/// Difference between [MySceneManager] and this?
/// This class will hold its GameObject reference, instead of just a string
/// </summary>
public static class NavigationManager
{
    public static Target OriginTarget { get; set; }

    public static Target DestinationTarget { get; set; }
}