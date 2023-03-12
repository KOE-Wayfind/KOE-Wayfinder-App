using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    // [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private GameObject indicatorSphere;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private GameObject intersectionBoxes;


    private NavMeshPath _path; // current calculated path
    private LineRenderer _line; // line renderer for path
    private Vector3 _targetPosition = Vector3.zero; // current target position


    private bool _lineToggle;
    private bool _isPathCalculated;

    // Start is called before the first frame update
    void Start()
    {
        _path = new NavMeshPath();
        _line = transform.GetComponent<LineRenderer>();
        _line.enabled = _lineToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lineToggle && _targetPosition != Vector3.zero)
        {
            if (!_isPathCalculated)
            {
                NavMeshPathWalkthrough();
                _isPathCalculated = true;

                // clear rendered path
                _line.positionCount = 0;
            }

            // TODO: Make line renderer connect to nearest intersection point
            RenderNavigationPath();
        }
    }

    private void NavMeshPathWalkthrough()
    {
        var res = NavMesh.CalculatePath(indicatorSphere.transform.position, _targetPosition, NavMesh.AllAreas, _path);
        // Debug.Log("Found path? " + res);
        _line.positionCount = _path.corners.Length;
        _line.SetPositions(_path.corners);

        CheckIntersection();
    }

    /// <summary>
    /// Check which IntersectionCube(s) the line renderer intersects with
    /// and set the properties `isHit` to true
    /// </summary>
    private void CheckIntersection()
    {
        // Get all children in the intersection boxes and store in gameObjectsToCheck
        List<GameObject> gameObjectsToCheck = new List<GameObject>();
        for (int i = 0; i < intersectionBoxes.transform.childCount; i++)
        {
            gameObjectsToCheck.Add(intersectionBoxes.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < gameObjectsToCheck.Count; i++)
        {
            Collider myCollider = gameObjectsToCheck[i].GetComponent<Collider>();
            // skip if the game object does not have a collider component
            if (myCollider == null) continue;

            for (int j = 0; j < _line.positionCount - 1; j++)
            {
                Vector3 startPos = _line.GetPosition(j);
                Vector3 endPos = _line.GetPosition(j + 1);
                if (Physics.Raycast(startPos, endPos - startPos, out var hitInfo, Vector3.Distance(startPos, endPos)))
                {
                    // make raycast visible
                    Debug.DrawLine(startPos, endPos, Color.yellow);

                    // gameobject references
                    GameObject intersectionCube = hitInfo.collider.gameObject;
                    intersectionCube.GetComponent<IntersectionBox>().isHit = true;

                    hitInfo.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;

                    // if (hitInfo.collider == collider)
                    // {
                    //     // The line renderer intersects with the collider of the game object
                    //     Debug.Log("Intersection detected: " + gameObjectsToCheck[i].name);
                    //     // Store the game object's reference or any relevant information you need
                    //     
                    //     // change the gameobject's color to red
                    //     gameObjectsToCheck[i].GetComponent<Renderer>().material.color = Color.red;
                    // }
                }
            }
        }
    }

    private void RenderNavigationPath()
    {
        var playerPosition = indicatorSphere.transform.position;

        var pathPoints = new List<Vector3>();
        pathPoints.Add(playerPosition);

        List<Vector3> intersectionCubePositions = new List<Vector3>();
        for (int i = 0; i < intersectionBoxes.transform.childCount; i++)
        {
            // add only cube that have isHit set to true
            if (intersectionBoxes.transform.GetChild(i).GetComponent<IntersectionBox>().isHit)
            {
                intersectionCubePositions.Add(intersectionBoxes.transform.GetChild(i).position);
            }
        }

        // add intersection cube that are hit
        for (int i = 0; i < intersectionCubePositions.Count; i++)
        {
            if (intersectionCubePositions[i] != Vector3.zero)
            {
                pathPoints.Add(intersectionCubePositions[i]);
            }
        }

        pathPoints.Add(_targetPosition);

        // offset the line so that it lower than the player eye level
        const float yOffset = 1f; // adjust this value to change the amount to lower the line

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 point = pathPoints[i];
            point.y -= yOffset;
            pathPoints[i] = point;
        }


        // set path starting with player position and ending with destination position
        // in between, add intersection cube that are hit
        _line.positionCount = pathPoints.Count;
        _line.SetPositions(pathPoints.ToArray());
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        _targetPosition = Vector3.zero;
        // string selectedText = navigationTargetDropdown.options[selectedValue].text;
        // Target currentTarget = navigationTargetObjects.Find(x => x.name == selectedText);
        Target currentTarget = navigationTargetObjects[selectedValue];

        if (currentTarget != null)
        {
            Debug.Log("Setted to " + currentTarget.name);
            _targetPosition = currentTarget.positionObject.transform.position;
        }
    }

    public void ToggleVisibility()
    {
        _lineToggle = !_lineToggle;
        _line.enabled = _lineToggle;

        if (_line.enabled) SetCurrentNavigationTarget(0); // hardcoded the enginius office

        buttonText.text = _lineToggle ? "Hide Path" : "Show Path";
    }
}