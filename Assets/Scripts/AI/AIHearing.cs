using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT IS TO BE ATTACHED TO AN AI HEARING SPHERE

public class AIHearing : MonoBehaviour
{
    [SerializeField] bool isOuter;

    // Different AI hearing states
    [HideInInspector] public bool playerIsHeardIn = false;
    [HideInInspector] public bool playerIsHeardOut = false;

    private void OnTriggerEnter(Collider collision)
    {
        // If player has entered the AI's inner hearing range
        if (collision.gameObject.tag == "Player" && isOuter == false)
        {
            playerIsHeardIn = true;
        }
        // If player has entered the AI's outer hearing range
        if (collision.gameObject.tag == "Player" && isOuter == true)
        {
            playerIsHeardOut = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // If player has left the AI's inner hearing range
        if (collision.gameObject.tag == "Player" && isOuter == false)
        {
            playerIsHeardIn = false;
        }
        // If player has left the AI's outer hearing range
        else if (collision.gameObject.tag == "Player" && isOuter == true)
        {
            playerIsHeardOut = false;
        }
    }
}
