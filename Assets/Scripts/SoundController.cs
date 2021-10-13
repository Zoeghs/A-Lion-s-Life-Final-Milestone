using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Array to store all audio sources in the scene
    private AudioSource[] audioSources;

    // Make this a singleton instance
    public static SoundController instance = null;

    private void Awake()
    {
        // This becomes the sound controller if there is not one already
        if (instance == null)
        {
            instance = this;
        }
        // If an instnace already exists, destroy this component
        else if (instance != this)
        {
            Destroy(this);
        }

        // Prevent this controller from being destroyed when a reloading the scene
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        // Instantiate the audio source array and get all audio sources in the scene
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    private AudioSource FindClip(string clipName)
    {
        // Search the audio source array for the specified sound clip
        for (int i = 0; i < audioSources.Length; i++)
        {
            // If the name of the clip matches the specified name
            if (audioSources[i].clip.name == clipName)
            {
                // Get the audio source with that name
                AudioSource clip = audioSources[i];

                // Return the clip if found
                return clip;
            }
        }

        // Return null if the clip is not found
        return null;
    }

    #region Clip Name Database
    // Clip name database:
    /*
        "Play_Button_Opt4"
        "Controls_Button_Opt8"
        "Credits_Button_Opt5"
        "Exit_Button_Opt7"
        "Back_Button_Opt6"
        
        "Walking"
        "Sprinting"
        "Sprinting 2x"
        "Sprinting 4x"

        "Quick Scratch"
        "Pounce_Roar"

        "Eating"
        "Drinking"
    */
    #endregion

    #region Menu Sound Functions

    #region Play Button Sound
    public void PlayPlaySound()
    {
        // Find the sound
        AudioSource playSound = FindClip("Play_Button_Opt4");

        // Once found, play the sound
        playSound.Play();
    }

    public void StopPlaySound()
    {
        // Find the sound
        AudioSource playSound = FindClip("Play_Button_Opt4");

        // Once found, stop the sound
        playSound.Stop();
    }
    #endregion

    #region Controls Button Sound
    public void PlayControlsSound()
    {
        // Find the sound
        AudioSource controlsSound = FindClip("Controls_Button_Opt8");

        // Once found, play the sound
        controlsSound.Play();
    }

    public void StopControlsSound()
    {
        // Find the sound
        AudioSource controlsSound = FindClip("Controls_Button_Opt8");

        // Once found, stop the sound
        controlsSound.Stop();
    }
    #endregion

    #region Credits Button Sound
    public void PlayCreditsSound()
    {
        // Find the sound
        AudioSource creditsSound = FindClip("Credits_Button_Opt5");

        // Once found, play the sound
        creditsSound.Play();
    }

    public void StopCreditsSound()
    {
        // Find the sound
        AudioSource creditsSound = FindClip("Credits_Button_Opt5");

        // Once found, stop the sound
        creditsSound.Stop();
    }
    #endregion

    #region Exit Button Sound
    public void PlayExitSound()
    {
        // Find the sound
        AudioSource exitSound = FindClip("Exit_Button_Opt7");

        // Once found, play the sound
        exitSound.Play();
    }

    public void StopExitSound()
    {
        // Find the sound
        AudioSource exitSound = FindClip("Exit_Button_Opt7");

        // Once found, stop the sound
        exitSound.Stop();
    }
    #endregion

    #region Back Button Sound
    public void PlayBackSound()
    {
        // Find the sound
        AudioSource backSound = FindClip("Back_Button_Opt6");

        // Once found, play the sound
        backSound.Play();
    }

    public void StopBackSound()
    {
        // Find the sound
        AudioSource backSound = FindClip("Back_Button_Opt6");

        // Once found, stop the sound
        backSound.Stop();
    }
    #endregion

    #endregion

    #region Lion Sound Functions

    #region Walking Sound
    public void PlayWalkingSound()
    {
        // Find the sound
        AudioSource walkingSound = FindClip("Walking");
        print(walkingSound.clip.ToString());

        // Once found, play the sound
        walkingSound.Play();
    }

    public void StopWalkingSound()
    {
        // Find the sound
        AudioSource walkingSound = FindClip("Walking");
        print(walkingSound.name);

        // Once found, stop the sound
        walkingSound.Stop();
    }
    #endregion

    #region Sprinting Sound
    public void PlaySprintingSound()
    {
        // Find the sound
        AudioSource sprintingSound = FindClip("Sprinting");

        // Once found, play the sound
        sprintingSound.Play();
    }

    public void StopSprintingSound()
    {
        // Find the sound
        AudioSource sprintingSound = FindClip("Sprinting");

        // Once found, stop the sound
        sprintingSound.Stop();
    }
    #endregion

    #region Sprinting 2x Sound
    public void PlaySprinting2xSound()
    {
        // Find the sound
        AudioSource sprinting2xSound = FindClip("Sprinting 2x");

        // Once found, play the sound
        sprinting2xSound.Play();
    }

    public void StopSprinting2xSound()
    {
        // Find the sound
        AudioSource sprinting2xSound = FindClip("Sprinting 2x");

        // Once found, stop the sound
        sprinting2xSound.Stop();
    }
    #endregion

    #region Sprinting 4x Sound
    public void PlaySprinting4xSound()
    {
        // Find the sound
        AudioSource sprinting4xSound = FindClip("Sprinting 4x");

        // Once found, play the sound
        sprinting4xSound.Play();
    }

    public void StopSprinting4xSound()
    {
        // Find the sound
        AudioSource sprinting4xSound = FindClip("Sprinting 4x");

        // Once found, stop the sound
        sprinting4xSound.Stop();
    }
    #endregion

    #region Quick Scratch Sound
    public void PlayQuickScratchSound()
    {
        // Find the sound
        AudioSource quickScratchSound = FindClip("Quick Scratch");

        // Once found, play the sound
        quickScratchSound.Play();
    }

    public void StopQuickScratchSound()
    {
        // Find the sound
        AudioSource quickScratchSound = FindClip("Quick Scratch");

        // Once found, stop the sound
        quickScratchSound.Stop();
    }
    #endregion

    #region Pounce Sound
    public void PlayPounceSound()
    {
        // Find the sound
        AudioSource pounceSound = FindClip("Pounce_Roar");

        // Once found, play the sound
        pounceSound.Play();
    }

    public void StopPounceSound()
    {
        // Find the sound
        AudioSource pounceSound = FindClip("Pounce_Roar");

        // Once found, stop the sound
        pounceSound.Stop();
    }
    #endregion

    #region Eating Sound
    public void PlayEatingSound()
    {
        // Find the sound
        AudioSource eatingSound = FindClip("Eating");

        // Once found, play the sound
        eatingSound.Play();
    }

    public void StopEatingSound()
    {
        // Find the sound
        AudioSource eatingSound = FindClip("Eating");

        // Once found, stop the sound
        eatingSound.Stop();
    }
    #endregion

    #region Drinking Sound
    public void PlayDrinkingSound()
    {
        // Find the sound
        AudioSource drinkingSound = FindClip("Drinking");

        // Once found, play the sound
        drinkingSound.Play();
    }

    public void StopDrinkingSound()
    {
        // Find the sound
        AudioSource drinkingSound = FindClip("Drinking");

        // Once found, stop the sound
        drinkingSound.Stop();
    }
    #endregion

    #endregion
}
