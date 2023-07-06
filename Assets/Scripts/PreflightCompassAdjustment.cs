using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PreflightCompassAdjustment : MonoBehaviour
{
    [SerializeField] private TMP_Text compassText;
    [SerializeField] private ARSession session;
    [SerializeField] private ARSessionOrigin sessionOrigin;
    
    // Adjust these values to control the smoothness
    [SerializeField] private float rotationSpeed = 5f;
    // [SerializeField] private float angleThreshold = 1f;
    
    // Phone level detection
    [SerializeField] private GameObject phoneLevelPanel;

    private bool _startTracking;
    private bool _startTrackingEditor;
    private float _compassValue;
    private bool _navigationStarted;

    private const float BuildingHeading = 265f; // yeah sometimes not correct, need more calibration
    private float _correctedHeading;

    private void Start()
    {
        // to disable compass input from ArSession interfere with our compass input
        session.GetComponent<ARInputManager>().enabled = false;
        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(InitializeCompass());
    }
    
    /// <summary>
    /// To be attached the Start Navigation Button
    /// </summary>
    public void StartNavigation()
    {
        _navigationStarted = true;
        session.Reset();

        // set the starting AR position and rotation to be the origin position
        sessionOrigin.transform.position = NavigationManager.OriginTarget.position;
        sessionOrigin.transform.rotation = Quaternion.Euler(0, _correctedHeading, 0);
        
        // Stop Input.Compass from handling the operation
        // Let AR things to the job
        // _startTracking = false;
        // _startTrackingEditor = false;
        session.GetComponent<ARInputManager>().enabled = true;
    }

    IEnumerator InitializeCompass()
    {
        yield return new WaitForSeconds(1f);
        _startTracking |= Input.compass.enabled;
        
        // for editor
        _startTrackingEditor = true;
    }

    /// <summary>
    /// Manual offset of the compass input value
    /// </summary>
    /// <param name="value">Slider value</param>
    public void SliderValueChanged(float value)
    {
        _compassValue = value;
    }

    private void Update()
    {
        if (_startTrackingEditor && Application.isEditor)
        {
            var targetHeading = BuildingHeading - _compassValue;
            targetHeading = (targetHeading + 360) % 360; // Ensure the target is within 0-360 range
    
            float smoothedHeading = Mathf.LerpAngle(sessionOrigin.transform.eulerAngles.y, targetHeading,
                rotationSpeed * Time.deltaTime);
            _correctedHeading = smoothedHeading;
    
            sessionOrigin.transform.rotation = Quaternion.Euler(0, smoothedHeading, 0);
            compassText.text = (int)smoothedHeading + "° " + DegreesToCardinalDetailed(smoothedHeading);
        }
        
        // Phone's screen pointing to the sky to get better, smoother & consistent compass reading
        if (_startTracking)
        {
            var targetHeading = Input.compass.trueHeading - BuildingHeading - _compassValue;
            targetHeading = (targetHeading + 360) % 360; // Ensure the target is within 0-360 range
    
            float smoothedHeading = Mathf.LerpAngle(sessionOrigin.transform.eulerAngles.y, targetHeading,
                rotationSpeed * Time.deltaTime);
            _correctedHeading = smoothedHeading;

            if (!_navigationStarted)
            {
                // only edit the ar pose before the navigation starts
                sessionOrigin.transform.rotation = Quaternion.Euler(0, smoothedHeading, 0);
            
                // check orientation
                CheckPhoneOrientation();
            }
            compassText.text = (int)smoothedHeading + "° " + DegreesToCardinalDetailed(smoothedHeading);
        }
    }

    private void CheckPhoneOrientation()
    {
        // Get the current acceleration vector from the device
        Vector3 acceleration = Input.acceleration;

        // Calculate the absolute value of the Z-axis acceleration
        float zAcceleration = Mathf.Abs(acceleration.z);

        // Define a threshold value to determine if the screen is pointing up
        float threshold = 0.7f;

        // Check if the Z-axis acceleration is greater than the threshold
        bool isScreenUp = zAcceleration > threshold;

        phoneLevelPanel.SetActive(!isScreenUp);
    }
    

    private static string DegreesToCardinalDetailed(double degrees)
    {
        string[] cardinals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
        return cardinals[(int)Math.Round((degrees * 10 % 3600) / 225)];
    }
}
