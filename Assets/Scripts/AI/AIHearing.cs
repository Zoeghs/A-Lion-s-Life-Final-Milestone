using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT IS TO BE ATTACHED TO AN AI HEARING SPHERE

public class AIHearing : MonoBehaviour
{
    [HideInInspector] public bool playerIsHeard = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player has entered the AI's hearing range
            playerIsHeard = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player has left the AI's hearing range
            playerIsHeard = false;
        }
    }
}
