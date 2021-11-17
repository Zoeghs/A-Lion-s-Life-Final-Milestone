using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT IS TO BE ATTACHED TO AN AI VISION CONE

public class AIVision : MonoBehaviour
{
    [HideInInspector] public bool playerIsSeen = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player has entered the AI's vision cone
            playerIsSeen = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player has left the AI's vision cone
            playerIsSeen = false;
        }
    }
}
