using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    // [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private GameObject indicatorSphere;
    [SerializeField] private TMP_Text buttonText;

    
    private NavMeshPath path; // current calculated path
    private LineRenderer line; // line renderer for path
    private Vector3 targetPosition = Vector3.zero; // current target position

    private bool lineToggle;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)
        {
            var res = NavMesh.CalculatePath(indicatorSphere.transform.position, targetPosition, NavMesh.AllAreas, path);
            Debug.Log("Found path? " + res);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
        }
    }
    
    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPosition = Vector3.zero;
        // string selectedText = navigationTargetDropdown.options[selectedValue].text;
        // Target currentTarget = navigationTargetObjects.Find(x => x.name == selectedText);
        Target currentTarget = navigationTargetObjects[selectedValue];
        
        if (currentTarget != null)
        {
            Debug.Log("Setted to "+ currentTarget.name);
            targetPosition = currentTarget.positionObject.transform.position;
        }
    }

    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
        
        if (line.enabled) SetCurrentNavigationTarget(0); // hardcoded the enginius office
        
        buttonText.text = lineToggle ? "Hide Path" : "Show Path";
    }
}