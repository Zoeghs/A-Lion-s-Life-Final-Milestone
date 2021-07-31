using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    private float walkingBobbingSpeed = 14f;
    private float bobbingAmount = 0.05f;
    private float currentBobbingSpeed;
    private float sprintingBobbingSpeed = 20f;
    private float currentBobbingAmount;
    public PlayerMovement playerMovement;

    float defaultPosY = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;

        // Set bobbing speed & amount to walking by default
        currentBobbingSpeed = walkingBobbingSpeed;
        currentBobbingAmount = bobbingAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(playerMovement.currentSpeed) > 0.01f)
        {
            // If player is walking
            if (playerMovement.isSprinting == false)
            {
                currentBobbingSpeed = walkingBobbingSpeed;
                currentBobbingAmount = bobbingAmount;
            }
            // If player is sprinting
            else
            {
                currentBobbingSpeed = sprintingBobbingSpeed;
                currentBobbingAmount = bobbingAmount * 2;
            }

            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
        }
    }
}
