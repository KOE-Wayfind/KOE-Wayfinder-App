using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private ARSession _session;
    [SerializeField] private ARSessionOrigin _sessionOrigin;

    [SerializeField] private GameObject _environmentObject;

    [SerializeField] private TMP_Text _arButtonText;

    public void ResetSession()
    {
        _session.Reset();

        _sessionOrigin.transform.position = Vector3.zero;
        _sessionOrigin.transform.rotation = Quaternion.identity;
    }

    public void adjustOpacity(float value)
    {
        // adjust the opacity for all gameobjects in the environment
        foreach (Transform child in _environmentObject.transform)
        {
            Debug.Log("Child name is " + child.name);
            child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, value);
        }
    }

    public void ToggleArAbility()
    {
        _session.enabled = !_session.enabled;
        _arButtonText.text = _session.enabled ? "Disable AR" : "Enable AR";
    }
}