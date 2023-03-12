using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private GameObject arCamera;

    // Update is called once per frame
    void Update()
    {
        // player will follow ar camera
        transform.position = arCamera.transform.position;

        // minimap camera follower will handle by Position Constraint in TopDownCamera
    }
}