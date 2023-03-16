using System;
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

    /// <summary>
    /// Mark this intersection is passed by the player during AR session
    /// </summary>
    public bool isPassed;

    /// <summary>
    /// The arrangement of the intersection cube in the path
    /// May not be in order. ie. it can be [1,3,5] and so on
    /// The smaller value come first, followed by the bigger value 
    /// </summary>
    public int pathOrder;
}