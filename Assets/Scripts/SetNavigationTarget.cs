using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

/// <summary>
/// This guy is attached to LinePath GameObject
/// </summary>
public class SetNavigationTarget : MonoBehaviour
{
    // [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private GameObject indicatorSphere;
    [SerializeField] private GameObject intersectionBoxes;
    [SerializeField] private GameObject topPanel;

    private NavMeshPath _path; // current calculated path
    private LineRenderer _line; // line renderer for path
    private Vector3 _targetPosition = Vector3.zero; // current target position

    private bool _lineToggle;
    private bool _isPathCalculated;

    [SerializeField] private float lineYPos = -.5f; // reduce this value to get lower line

    // Start is called before the first frame update
    private void Start()
    {
        _path = new NavMeshPath();
        _line = transform.GetComponent<LineRenderer>();
        _line.enabled = _lineToggle;
    }

    // Update is called once per frame
    private void Update()
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

            // hide the top panel if player is arriving at destination
            if (IsCloseToDestination()) topPanel.SetActive(false);

            // TODO: Make line renderer connect to nearest intersection point
        }
    }


    private bool IsCloseToDestination()
    {
        var distance = GetDistanceFromPlayerToDestination();
        return distance is > 5 and < 10f;
    }

    private bool IsArrived()
    {
        var distance = GetDistanceFromPlayerToDestination();
        return distance < 5;
    }

    private float GetDistanceFromPlayerToDestination()
    {
        return Vector3.Distance(_line.GetPosition(0), _line.GetPosition(_line.positionCount - 1));
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

            int cubePathOrder = 0;

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
                    intersectionCube.GetComponent<IntersectionBox>().pathOrder = ++cubePathOrder;

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

    /// <summary>
    /// Draw straight line between player & intersection points
    /// </summary>
    private void RenderNavigationPath()
    {
        // first point is the player position
        var pathPoints = new List<Vector3> { indicatorSphere.transform.position };

        var intersectionCubes = new List<GameObject>();
        for (var i = 0; i < intersectionBoxes.transform.childCount; i++)
        {
            var intersectionProp = intersectionBoxes.transform.GetChild(i).GetComponent<IntersectionBox>();

            // add only cube that are in the navmesh path and
            // cube that the player has not passed yet
            // TODO: This method have one problem, when the player 'berpatah balik' the line renderer
            // will not connect to the nearest intersection point
            if (intersectionProp.isHit && !intersectionProp.isPassed)
            {
                intersectionCubes.Add(intersectionBoxes.transform.GetChild(i).gameObject);
            }
        }

        // rearrange the intersectionCubes list based on the pathOrder
        intersectionCubes = intersectionCubes.OrderBy(x => x.GetComponent<IntersectionBox>().pathOrder).ToList();

        // add those points to the pathPoints list
        pathPoints.AddRange(intersectionCubes.Select(t => t.transform.position));

        // finally, the destination position
        pathPoints.Add(_targetPosition);

        // Offset the line renderer position from the ground
        for (var i = 0; i < pathPoints.Count; i++)
        {
            var point = pathPoints[i];
            point.y = lineYPos;
            pathPoints[i] = point;
        }


        // Determine the direction command for each intersection cube
        for (var i = 1; i < pathPoints.Count; i++)
        {
            if (i == pathPoints.Count - 1) break;
            var dir = DirectionCommand(pathPoints[i - 1], pathPoints[i], pathPoints[i + 1]);
            intersectionCubes[i - 1].GetComponent<IntersectionBox>().directionCommand = dir;
        }

        _line.positionCount = pathPoints.Count;
        _line.SetPositions(pathPoints.ToArray());
    }

    /// <summary>
    /// Determine the direction command for each intersection cube using right-hand rule
    /// 
    /// </summary>
    /// <param name="previousPosition"></param>
    /// <param name="currentPosition"></param>
    /// <param name="nextPosition"></param>
    /// <returns>Direction command (eg: "Turn right")</returns>
    private String DirectionCommand(Vector3 previousPosition, Vector3 currentPosition, Vector3 nextPosition)
    {
        // if player already close to the target, just replace the command with "You have arrived" or smth
        // ideally, I would do this as OnTriggerEnter on smth, but I don't have much time heuehuhe
        if (IsArrived())
        {
            return "You have arrived";
        }

        Vector3 v1 = currentPosition - previousPosition;
        Vector3 v2 = nextPosition - currentPosition;

        // If the resulting vector points upwards (in the y-axis direction), then the turn is to the right.
        // If it points downwards (in the negative y-axis direction), then the turn is to the left.
        Vector3 cross = Vector3.Cross(v1, v2);

        if (cross.y > 0)
        {
            return "Turn right";
        }

        if (cross.y < 0)
        {
            return "Turn left";
        }

        return "Going straight";
    }


    public void ToggleVisibility()
    {
        _lineToggle = !_lineToggle;
        _line.enabled = _lineToggle;

        Target currentTarget = NavigationManager.DestinationTarget;

        if (_line.enabled)
        {
            if (currentTarget != null)
            {
                Debug.Log("Setted to " + currentTarget.name);
                _targetPosition = currentTarget.position;
            }
        }
    }

    public void AdjustLineHeight(float value)
    {
        lineYPos = value;
    }
}