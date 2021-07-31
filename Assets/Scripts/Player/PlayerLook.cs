using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Vars for mouse inputs
    private float mouseX;
    private float mouseY;

    // Mouse speed
    [SerializeField] float mouseSpeed = 100f;

    // Camera's rotation along the x axis
    private float xRot;

    // Var for player transform
    [SerializeField] Transform playerTransform;

    // Var for locking looking
    public bool allowLooking = true;

    void Start()
    {
        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Player can only look around when not in a menu
        if (allowLooking == true)
        {
            // Get mouse inputs
            GetMouseInputs();

            // Rotate camera when looking up or down
            RotateCamera();

            // Rotate player when looking side to side
            playerTransform.transform.Rotate(Vector3.up * mouseX);
        }
    }

    private void GetMouseInputs()
    {
        // Get x and y inputs
        mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        // Rotate camera when looking up or down

        // Save rotation based on input
        xRot -= mouseY;

        // Clamp rotation
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        // Apply rotation
        gameObject.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
