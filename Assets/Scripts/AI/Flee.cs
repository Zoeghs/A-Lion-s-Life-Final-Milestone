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

    // How close the player / predator has to be in order to run away
    private float fleeDistance = 50f;

    // Speed at which the AI object can run away
    //private float fleeSpeed = 6f;

    // Vars for hearing and vision colliders
    [SerializeField] GameObject hearingRange;
    [SerializeField] GameObject visionCone;
    private AIHearing hearingScript;
    private AIVision visionScript;

    // Distance vars
    private float distanceFromPlayer;
    private float distanceFromPredator;

    // Corutine bool to make sure it isn't running mutiple of itself
    private bool currentlyFleeing = false;

    // Access to sound controller
    private SoundController soundController;


    void Start()
    {
        // Get this object's nav mesh agent
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        // Get vision & hearing scripts
        visionScript = visionCone.GetComponent<AIVision>();
        hearingScript = hearingRange.GetComponent<AIHearing>();

        // Find sound controller in the scene
        soundController = FindObjectOfType<SoundController>();
    }

    void Update()
    {
        // Calculate distance from both player & predators
        distanceFromPlayer = CalculateDistance("Player");
        distanceFromPredator = CalculateDistance("Predator");

        // If this object needs to flee from the player
        if (fleeFromPlayer == true && visionScript.playerIsSeen == true && currentlyFleeing == false)
        {
            StartCoroutine(nameof(FleeFrom), "Player");
        }
        else if (fleeFromPlayer == true && hearingScript.playerIsHeardIn && soundController.totalLoudness > 0.005f && currentlyFleeing == false)
        {
            StartCoroutine(nameof(FleeFrom), "Player");
        }
        // if the AI hears the player, turn towards them
        else if (fleeFromPlayer == true && hearingScript.playerIsHeardOut == true && soundController.totalLoudness > 0.005f)
        {
            TurnTowards("Player");
        }
        else if (fleeFromPlayer == true && hearingScript.playerIsHeardIn == true && soundController.totalLoudness < 0.005f)
        {
            TurnTowards("Player");
        }

        // If this object needs to flee from predators
        if (fleeFromPredators == true && distanceFromPredator <= fleeDistance && currentlyFleeing == false)
        {
            StartCoroutine(nameof(FleeFrom), "Predator");
        }
    }

    private void TurnTowards(string turnTag)
    {
        GameObject subject = GameObject.FindGameObjectWithTag(turnTag);

        gameObject.transform.LookAt(subject.transform);
    }

    IEnumerator FleeFrom(string fleeTag)
    {
        // Only run if not currenly running
        currentlyFleeing = true;

        distanceFromPlayer = CalculateDistance("Player");

        // Get specified object to flee from
        GameObject toFleeFrom = GameObject.FindGameObjectWithTag(fleeTag);

        // Find vector from specified object to this object
        Vector3 distanceVector = gameObject.transform.position - toFleeFrom.transform.position;

        // Calcualte new position
        Vector3 newPos = gameObject.transform.position + distanceVector;

        // Move the object away from the specified object
        navAgent.SetDestination(newPos);

        // If the AI is far away enough from the player, it stops running
        if (distanceFromPlayer > fleeDistance)
        {
           currentlyFleeing = false;
        }

        yield return null;
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

        // Returns distance that is out of range for fleeing if there is no predator in the scene
        return 100;

    }
    
    private void OnDrawGizmos()
    {
        // Head pos
        Vector3 head = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z + 0.4f);

        // Draw inner hearing range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 7);

        // Draw outer hearing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 12);

        // Draw vision cone
        //Gizmos.color = Color.red;
        //Gizmos.DrawFrustum(head, 5, transform.forward.z * 270, 0, 5);

        // Draw how far the AI can see
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(head, new Vector3(head.x, head.y, head.z + 3 * transform.forward.z));
    }
}