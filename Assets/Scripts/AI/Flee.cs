using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : MonoBehaviour
{
    #region Variables

    public string state = "";

    // What to flee from
    [SerializeField] bool fleeFromPlayer;
    [SerializeField] bool fleeFromPredators;

    // Access to nav mesh agent
    private NavMeshAgent navAgent;

    // How close the player / predator has to be in order to run away
    private float fleeTime = 10f;

    // Speed at which the AI object can run away
    //private float fleeSpeed = 6f;

    // Vars for hearing and vision colliders
    [SerializeField] GameObject hearingRange;
    [SerializeField] GameObject visionCone;
    private AIVision visionScript;

    // Distance vars
    private float distanceFromPlayer;
    private float distanceFromPredator;

    // Access to sound controller
    private SoundController soundController;

    // Hearing range distances
    private float outerHearingRange = 12;
    private float innerHearingRange = 7;

    // If the AI is currently fleeing
    private bool currentlyFleeing = false;

    #endregion


    void Start()
    {
        // Get this object's nav mesh agent
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        // Get vision & hearing scripts
        visionScript = visionCone.GetComponent<AIVision>();
        //hearingScript = hearingRange.GetComponent<AIHearing>();

        // Find sound controller in the scene
        soundController = FindObjectOfType<SoundController>();
    }

    void Update()
    {
        // Calculate distance from both player & predators
        distanceFromPlayer = CalculateDistance("Player");
        distanceFromPredator = CalculateDistance("Predator");

        // Check if player is within hearing ranges
        HearingVisionCheck("Player", distanceFromPlayer);

        // Check if predators are withing hearing ranges
        // WILL CURRENTLY NOT WORK BECAUSE TOTAL LOUDNESS ONLY APPLIES TO THE PLAYER

        Debug.Log("state of animalia: " + state);
    }

    private void updateState()
    {
        // Use switches here

        // when a state finishes running its code, switch to the default state
        // Default state looks for what state it needs to switch into
    }



    private void HearingVisionCheck(string targetTag, float distance)
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);

        // If the player is in the outer hearing range and is making less than 0.005 in noise, no reaction from AI

        // If the player is in the outer hearing range
        if (distance < outerHearingRange && distance > innerHearingRange)
        {
            // If the player is making greater than 0.005 in noise, the AI turns towards the player
            if (soundController.totalLoudness > 0.005f && currentlyFleeing == false)
            {
                TurnTowards(targetTag);
                state = "TURNING";
            }
        }
        // If the player is in the inner hearing range
        else if (distance < innerHearingRange)
        {
            // If the player is in the AI vision cone (this behaviour is checked first as it overrides any sound behaviours)
            if (visionScript.playerIsSeen == true && currentlyFleeing == false)
            {
                FleeFrom(targetTag);
                state = "FLEEING";
            }
            // If the player is making less than 0.005 in noise, the AI turns towards the player
            else if (soundController.totalLoudness < 0.005f && currentlyFleeing == false)
            {
                TurnTowards(targetTag);
                state = "TURNING";
            }
            // If the player is making greater than 0.005 in noise, the AI turns away from the player and runs
            else if (soundController.totalLoudness < 0.005f && currentlyFleeing == false)
            {
                FleeFrom(targetTag);
                state = "FLEEING";
            }
        }
    }

    private void TurnTowards(string turnTag)
    {
        GameObject subject = GameObject.FindGameObjectWithTag(turnTag);

        gameObject.transform.LookAt(subject.transform);
    }

    private void FleeFrom(string fleeTag)
    {
        // Set current time
        float currentFleeTime = fleeTime;

        currentlyFleeing = true;

        // Loop for how long is specified
        for (int i = 0; i < currentFleeTime; i++)
        {
            // Get specified object to flee from
            GameObject toFleeFrom = GameObject.FindGameObjectWithTag(fleeTag);

            // Find vector from specified object to this object
            Vector3 distanceVector = gameObject.transform.position - toFleeFrom.transform.position;

            // Calcualte new position
            Vector3 newPos = gameObject.transform.position + distanceVector;

            // Move the object away from the specified object
            navAgent.SetDestination(newPos);

            // Update time
            currentFleeTime -= Time.deltaTime;
        }

        currentlyFleeing = false;
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