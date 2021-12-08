using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT IS TO BE ATTACHED TO AN AI HEARING SPHERE

public class PlayerCover : MonoBehaviour
{
    [HideInInspector] public bool playerIsInCover;

    private void OnTriggerEnter(Collider collision)
    {
        // If player has entered a cover's collider
        if (collision.gameObject.tag == "Cover")
        {
            playerIsInCover = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // If player has left a cover's collider
        if (collision.gameObject.tag == "Cover")
        {
            playerIsInCover = false;
        }
    }
}
