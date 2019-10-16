using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] float scrollSpeed = 15f;
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 5f;

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        playerCam.orthographicSize -= scroll * scrollSpeed * Time.deltaTime; //scroll the camera
        if (playerCam.orthographicSize < minSize)
            playerCam.orthographicSize = minSize;
        if (playerCam.orthographicSize > maxSize)
            playerCam.orthographicSize = maxSize;
        transform.rotation = Quaternion.Euler(Vector3.zero); //set the camera's rotation to zero (it's parented to the player ship so we don't want it to rotate if the player does)
    }
}
