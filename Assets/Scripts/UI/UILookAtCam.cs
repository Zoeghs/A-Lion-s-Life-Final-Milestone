using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCam : MonoBehaviour
{
    // Get camera to look at
    [SerializeField] Camera mainCamera;

    void Update()
    {
        // Have UI look at the player's camera
        FollowCam();
    }

    private void FollowCam()
    {
        // Point health canvas at the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);

    }
}
