using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    // Access to camera
    [SerializeField] Camera mainCamera;

    // How close the player has to be to land an attack
    private float attackDistance = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        // Check for quick scratch
        QuickScratch();
    }

    private void QuickScratch()
    {
        // If the player left clicks and right click is not being held
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) == false)
        {
            // Do a quick scratch
            print("Quick Scratch");

            // vv Add visuals for quick scratch here vv
        }
    }
}
