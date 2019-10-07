using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] float scrollSpeed = 15f;
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        playerCam.orthographicSize -= scroll * scrollSpeed * Time.deltaTime;
        if (playerCam.orthographicSize < minSize)
            playerCam.orthographicSize = minSize;
        if (playerCam.orthographicSize > maxSize)
            playerCam.orthographicSize = maxSize;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
