using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Attached to Player
/// </summary>
public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] private TMP_Text directionCommandText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            // Debug.Log("collider inside OnTriggerStay is: " + other.name);
            var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = directionCommand;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = directionCommand;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = "Continue straight";
        }
    }
}