using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private ARSession session;
    [SerializeField] private ARSessionOrigin sessionOrigin;

    [SerializeField] private GameObject environmentObject;

    [SerializeField] private TMP_Text arButtonText;

    private void Start()
    {
        var destination = SceneManager.destinationTarget;
        Debug.Log("We'll go to " + destination);
    }

    public void ResetSession()
    {
        session.Reset();

        sessionOrigin.transform.position = Vector3.zero;
        sessionOrigin.transform.rotation = Quaternion.identity;
    }

    public void AdjustOpacity(float value)
    {
        // adjust the opacity for all gameobjects in the environment
        foreach (Transform child in environmentObject.transform)
        {
            Debug.Log("Child name is " + child.name);
            child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, value);
        }
    }

    public void ToggleArAbility()
    {
        session.enabled = !session.enabled;
        arButtonText.text = session.enabled ? "Disable AR" : "Enable AR";
    }
}