using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Array to store all audio sources in the scene
    private AudioSource[] audioSources;

    // Make this a singleton instance
    public static SoundController instance = null;

    #region Clip Name Database
    [HideInInspector] public const string playSound = "Play_Button_Opt4";
    [HideInInspector] public const string controlsSound = "Controls_Button_Opt8";
    [HideInInspector] public const string creditsSound = "Credits_Button_Opt5";
    [HideInInspector] public const string exitSound = "Exit_Button_Opt7";
    [HideInInspector] public const string backSound = "Back_Button_Opt6";

    [HideInInspector] public const string walkingSound = "Walking";
    [HideInInspector] public const string sprintingSound = "Sprinting";
    [HideInInspector] public const string sprinting2Sound = "Sprinting 2x";
    [HideInInspector] public const string sprinting4Sound = "Sprinting 4x";

    [HideInInspector] public const string quickScratchSound = "Quick Scratch";
    [HideInInspector] public const string pounceSound = "Pounce_Roar";

    [HideInInspector] public const string eatingSound = "Eating";
    [HideInInspector] public const string drinkingSound = "Drinking";
    #endregion

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

    public void PlaySound(string soundName)
    {
        // Find the sound
        AudioSource sound = FindClip(soundName);

        // Once found, play the sound
        sound.Play();
    }

    public void StopSound(string soundName)
    {
        // Find the sound
        AudioSource sound = FindClip(soundName);

        // Once found, stop the sound
        sound.Stop();
    }
}
