using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntersectionBox : MonoBehaviour
{
    /// <summary>
    /// Mark this intersection is hit by the navigation mesh agent
    /// </summary>
    public bool isHit;

    /// <summary>
    /// Direction command such as "Turn right" or "Turn left"
    /// </summary>
    public String directionCommand;
}