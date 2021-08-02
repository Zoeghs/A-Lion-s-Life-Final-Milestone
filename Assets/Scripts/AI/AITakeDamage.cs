using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITakeDamage : MonoBehaviour
{
    // How close the player has to be to land an attack
    private float attackDistance = 5f;

    // Access to player game object
    [SerializeField] GameObject player;

    // Hit detection bool
    [HideInInspector] public bool isHit = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print(Vector3.Distance(player.transform.position, gameObject.transform.position));

        // When object is clicked and is within distance
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= attackDistance)
        {
            print("Hit");

            // Object has been hit
            isHit = true;
        }
        else
        {
            isHit = false;
        }

    }
}
