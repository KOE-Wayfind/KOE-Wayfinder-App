using System;
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
    [SerializeField] private GameObject intersectionBoxes;

    [SerializeField] private TMP_Text lineHeightValue;

    private NavMeshPath _path; // current calculated path
    private LineRenderer _line; // line renderer for path
    private Vector3 _targetPosition = Vector3.zero; // current target position
    
    private bool _lineToggle;
    private bool _isPathCalculated;
    
    float lineYPos = -.5f; // adjust this value to change the amount to lower the line

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
            RenderNavigationPath();

            lineHeightValue.text = lineYPos.ToString();

            // TODO: Make line renderer connect to nearest intersection point
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

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 point = pathPoints[i];
            point.y = lineYPos;
            pathPoints[i] = point;
        }

        for (int i = 1; i < pathPoints.Count; i++)
        {
            if (i == pathPoints.Count - 1) break;
            var dir =DirectionCommand(pathPoints[i-1], pathPoints[i], pathPoints[i+1]);
            Debug.Log(dir);
        }
    
        // set path starting with player position and ending with destination position
        // in between, add intersection cube that are hit
        _line.positionCount = pathPoints.Count;
        _line.SetPositions(pathPoints.ToArray());
    }

    private String DirectionCommand(Vector3 previousPosition, Vector3 currentPosition, Vector3 nextPosition)
    {
        // Vector3 previousPosition = new Vector3(0f, -1f, 0f);
        // Vector3 currentPosition = new Vector3(0.25f, -0.59f, 20.41f);
        // Vector3 nextPosition = new Vector3(-11.33f, -0.59f, 20.41f);

        Vector3 v1 = currentPosition - previousPosition;
        Vector3 v2 = nextPosition - currentPosition;

        Vector3 cross = Vector3.Cross(v1, v2);

        if (cross.y > 0)
        {
            return "Turn right";
        }
        else if (cross.y < 0)
        {
            return "Turn left";
        }
        else
        {
            return "Going straight";
        }
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

    public void AdjustLineHeight(float value)
    {
        lineYPos = value;
    }
}
