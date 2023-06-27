using System;
using System.Collections;
using System.Collections.Generic;
using Google.MaterialDesign.Icons;
using TMPro;
using UnityEngine;

/// <summary>
/// Attached to Player
/// </summary>
public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] private TMP_Text directionCommandText;
    [SerializeField] private GameObject arrowIcon;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            // Debug.Log("collider inside OnTriggerStay is: " + other.name);
            var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = directionCommand;
            
            ChangeIcon(directionCommand);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = directionCommand;
            
            ChangeIcon(directionCommand);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Intersection"))
        {
            // var directionCommand = other.gameObject.GetComponent<IntersectionBox>().directionCommand;
            directionCommandText.text = "Continue straight";
            
            // mark intersection as passed, so the line can be detached
            other.gameObject.GetComponent<IntersectionBox>().isPassed = true;
            
            ChangeIcon("Continue straight");
        }
    }

    private void ChangeIcon(string command)
    {
        if (command.Contains("arrived"))
        {
            arrowIcon.SetActive(false);
            return;
        }
        
        if (command.Contains("straight"))
        {
            arrowIcon.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (command.Contains("left"))
        {
            arrowIcon.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (command.Contains("right"))
        {
            arrowIcon.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (command.Contains("back"))
        {
            arrowIcon.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}
