using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private GameObject arCamera;
    [SerializeField] private GameObject playerIndicator;
    [SerializeField] private GameObject topDownCamera;

    // Update is called once per frame
    void Update()
    {
        var arCameraPosition = arCamera.transform.position;
        
        // player indicator will follow ar camera position
        playerIndicator.transform.position = arCameraPosition;
        
        // top down camera will follow AR rotation in y and the player position
        var currentTopDownCameraHeight = topDownCamera.transform.position.y;
        topDownCamera.transform.rotation = Quaternion.Euler(90, arCamera.transform.rotation.eulerAngles.y, 0);
        topDownCamera.transform.position = new Vector3(arCameraPosition.x, currentTopDownCameraHeight, arCameraPosition.z);
    }
}