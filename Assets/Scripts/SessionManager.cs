using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<GameObject> originTargets;

    private string _originUserLocation;
    private string _destinationUserLocation;

    private void Start()
    {
        // default value are assigned for debugging purposes
        _originUserLocation = MySceneManager.OriginLocation ?? "Conference Room A"; 
        Debug.Log("We'll start from " + _originUserLocation);
        _destinationUserLocation = MySceneManager.DestinationTarget ?? "Toilet";
        Debug.Log("We'll go to " + _destinationUserLocation);
        
        MoveArSessionToUserOrigin();
    }

    private void MoveArSessionToUserOrigin()
    {
        GameObject originTarget = null;
        // Find the gameobject that matches the origin location
        originTarget = originTargets.FirstOrDefault(target => target.name == _originUserLocation);
        
        sessionOrigin.transform.position = originTarget.transform.position;
        sessionOrigin.transform.rotation = originTarget.transform.rotation;
    }

    public void ResetSession()
    {
        session.Reset();

        sessionOrigin.transform.position = Vector3.zero;
        sessionOrigin.transform.rotation = Quaternion.identity;
    }

    public void ToggleArAbility()
    {
        session.enabled = !session.enabled;
        arButtonText.text = session.enabled ? "Disable AR" : "Enable AR";
    }
}