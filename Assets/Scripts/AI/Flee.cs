using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : MonoBehaviour
{
    // What to flee from
    [SerializeField] bool fleeFromPlayer;
    [SerializeField] bool fleeFromPredators;

    // Access to nav mesh agent
    private NavMeshAgent navAgent;

    // How close the player / predator has to be in order to run away (NEEDS TO LATER ACCOUNT FOR SOUND INSTEAD OF DISTANCE)
    private float fleeDistance = 7f;

    // Speed at which the AI object can run away
    //private float fleeSpeed = 6f;

    void Start()
    {
        // Get this object's nav mesh agent
        navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Calculate distance from both player & predators
        float distanceFromPlayer = CalculateDistance("Player");
        float distanceFromPredator = CalculateDistance("Predator");

        // If this object needs to flee from the player
        if (fleeFromPlayer == true && distanceFromPlayer <= fleeDistance)
        {
            // Flee from the player
            FleeFrom("Player");
        }

        // If this object needs to flee from predators
        if (fleeFromPredators == true && distanceFromPredator <= fleeDistance)
        {
            // Flee from the player
            FleeFrom("Predator");
        }

        // AI is listening
        Hearing();

        // AI is looking
        Vision();
    }

    private void FleeFrom(string fleeTag)
    {
        // Get specified object to flee from
        GameObject toFleeFrom = GameObject.FindGameObjectWithTag(fleeTag);

        // Find vector from specified object to this object
        Vector3 distanceVector = gameObject.transform.position - toFleeFrom.transform.position;

        // Calcualte new position
        Vector3 newPos = gameObject.transform.position + distanceVector;

        // Move the object away from the specified object
        navAgent.SetDestination(newPos);
    }

    private float CalculateDistance(string distanceTag)
    {
        // Get gameobject with specified tag
        GameObject toFleeFrom = GameObject.FindGameObjectWithTag(distanceTag);

        // Calculate distance
        if (toFleeFrom != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, toFleeFrom.transform.position);
            return distance;
        }

        // Returns distance that is our of range for fleeing if there is no predator in the scene
        return 100;

    }

    private void Hearing()
    {

    }

    private void Vision()
    {

    }
    
    private void OnDrawGizmos()
    {
        // Head pos
        Vector3 head = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z + 0.4f);

        // Draw hearing range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3);

        // Draw vision cone
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(head, 5, transform.forward.z + 270, 0, 5);

        // Draw how far the AI can see
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(head, new Vector3(head.x, head.y, head.z + 5));
    }
}