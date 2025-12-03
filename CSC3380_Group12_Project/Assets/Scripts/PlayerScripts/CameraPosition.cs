using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        // Gets the camera's position from the cameraPos object in Player prefab
        transform.position = cameraPosition.position;
    }
}
