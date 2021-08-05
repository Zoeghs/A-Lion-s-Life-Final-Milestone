using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    // Access to 3rd person camera point
    [SerializeField] GameObject thridPersonPoint;

    // Bool for mode detection
    [HideInInspector] public bool inThirdPerson = false;

    void Start()
    {
        // Set main camera defaults
        SetDefaults();
    }

    void Update()
    {
        // If player presses F5 and is not already in third person
        if (Input.GetKeyDown(KeyCode.F5) && inThirdPerson == false)
        {
            // Swap to 3rd person camera
            SwapToThird();

            // Player is now in thrid person mode
            inThirdPerson = true;
        }
        // If player presses F5 and is already in third person mode
        else if (Input.GetKeyDown(KeyCode.F5) && inThirdPerson == true)
        {
            // Swap back to first person camera
            SetDefaults();

            // Player is no longer in third person mode
            inThirdPerson = false;
        }
    }

    private void SetDefaults()
    {
        // Set main camera defaults
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        // Turn on player look script
        gameObject.GetComponentInParent<PlayerLook>().enabled = true;

        // Turn on bobbing scipt
        gameObject.GetComponent<Bobbing>().enabled = true;

        // Turn off 3rd person camera brain
        gameObject.GetComponent<CinemachineBrain>().enabled = false;

        // Turn off 3rd person camera point
        thridPersonPoint.SetActive(false);
    }

    private void SwapToThird()
    {
        // Swap to 3rd person camera

        // Turn off player look script
        gameObject.GetComponentInParent<PlayerLook>().enabled = false;

        // Turn off bobbing scipt
        gameObject.GetComponent<Bobbing>().enabled = false;

        // Turn on 3rd person camera point
        thridPersonPoint.SetActive(true);

        // Turn on 3rd person camera brain
        gameObject.GetComponent<CinemachineBrain>().enabled = true;
    }
}
