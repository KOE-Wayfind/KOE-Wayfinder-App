using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private ARSession session;
    [SerializeField] private ARSessionOrigin sessionOrigin;

    [SerializeField] private GameObject environmentObject;

    [SerializeField] private TMP_Text arButtonText;

    [SerializeField] private List<GameObject> allTargets;
    [SerializeField] private RawImage imageTopPanel;

    [SerializeField] private TMP_Text textTopPanel;
    
    [SerializeField] private GameObject locationPinPrefab;
    [SerializeField] private int destinationPinOffsetMultiplier = 1; 

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject navigationCommand;
    

    private Target _originTarget;
    private Target _destinationTarget;

    private void Start()
    {
        // default value are assigned for debugging purposes
        // set origin destination
        _originTarget = GetDestinationTarget(MySceneManager.OriginLocationName ?? "Conference Room A");
        NavigationManager.OriginTarget = _originTarget;
        Debug.Log("We'll start from " + _originTarget.name + " at " + _originTarget.position);

        // set destination target
        _destinationTarget = GetDestinationTarget(MySceneManager.DestinationLocationName ?? "OSCENT");
        NavigationManager.DestinationTarget = _destinationTarget;
        Debug.Log("We'll go to " + _destinationTarget.name + " at " + _destinationTarget.position);

        MoveArSessionToUserOrigin();
        SetTopPanelInformation();
        SetLocationPinToDestination();
    }

    private void MoveArSessionToUserOrigin()
    {
        sessionOrigin.transform.position = _originTarget.position;
        sessionOrigin.transform.rotation = new Quaternion(0f, 100f, 0f, 1f);
    }

    private void SetTopPanelInformation()
    {
        var imagesDict = new Dictionary<string, string>
        {
            { "Conference Room A", "Places/conference-a_18" },
            { "ODDAI", "Places/oddai_2" },
            { "Main Entrance", "Places/main-entrance_19" },
            { "Enginius Office", "Places/enginius-office_23" },
            { "OSCENT", "Places/oscent_9" },
            { "Toilet", "Places/toilet-e1" },
            { "Stairs", "Places/tangga-e1" }
        };
        string imagePath;

        try
        {
            imagePath = imagesDict[_destinationTarget.name];
        }
        catch
        {
            imagePath = "Places/default-no-location";
        }

        // read the image from the resources folder
        imageTopPanel.texture = Resources.Load<Texture>(imagePath);

        // set the text
        textTopPanel.text = _destinationTarget.name;
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

    private Target GetDestinationTarget(string name)
    {
        // find the relevant gameobject from the allTargets list
        var targetGameObject = allTargets.Find(x => x.name == name);

        return new Target(targetGameObject.name, targetGameObject.transform.position);
    }
    
    private void SetLocationPinToDestination()
    {
        // instantiate prefab at destinationTarget position
        var offset = Vector3.down * destinationPinOffsetMultiplier;
        Instantiate(locationPinPrefab, _destinationTarget.position - offset, Quaternion.identity);
    }

    /// <summary>
    /// Hide Start Button & show destination command panel
    /// </summary>
    public void StartNavigation()
    {
        startPanel.SetActive(false);
        navigationCommand.SetActive(true);
    } 
}