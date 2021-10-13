using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITakeDamage : MonoBehaviour
{
    #region Variables

    // How close the player has to be to land an attack
    private float attackDistance = 5f;

    // Access to player game object and components
    [SerializeField] GameObject player;
    private PlayerAttacks playerAttacks;

    // Hit detection bool
    [HideInInspector] public bool isHit = false;

    // Get UI elements
    [SerializeField] Image hitOne;
    [SerializeField] Image hitTwo;
    [SerializeField] Image hitThree;
    [SerializeField] Image hitFour;

    // Visual delay timer
    private float originalTime;
    private float timer = 0.5f;

    #endregion

    void Start()
    {
        // Make hit UI invisable
        hitOne.enabled = false;
        hitTwo.enabled = false;
        hitThree.enabled = false;
        hitFour.enabled = false;

        // Get attack script off of player
        playerAttacks = player.GetComponent<PlayerAttacks>();

        originalTime = timer;
    }

    void Update()
    {
        // Check is delay is required
        HitVisualDelay();
    }

    private void OnMouseDown()
    {
        // When object is clicked and is within distance
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= attackDistance && isHit == false)
        {
            // Update UI
            hitOne.enabled = true;
            hitTwo.enabled = true;
            hitThree.enabled = true;
            hitFour.enabled = true;

            // Object has been hit
            isHit = true;
        }
    }

    public void OnMouseUp()
    {
        // The object is no longer being hit
        isHit = false;
    }

    private void HitVisualDelay()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            hitOne.enabled = false;
            hitTwo.enabled = false;
            hitThree.enabled = false;
            hitFour.enabled = false;

            // Reset timer
            timer = originalTime;
        }
    }
}
