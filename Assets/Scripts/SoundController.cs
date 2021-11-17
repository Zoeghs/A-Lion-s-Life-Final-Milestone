using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Array to store all audio sources in the scene
    private AudioSource[] audioSources;

    // Make this a singleton instance
    public static SoundController instance = null;

    // Clip loudness vars
    private float updateStep = 0.1f;
    private int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;
    private float clipLoudness;
    private float[] clipSampleData;
    private float totalLoudness = 0f;

    private float[] sampleLoudness;
    private string[] sampleNames;

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

        // Set up clip sample data array with the appropriate length
        if (audioSources == null)
        {
            Debug.LogError(GetType() + "Error in Start: there are no audio sources in the scene.");
        }
        else
        {
            clipSampleData = new float[sampleDataLength];

            // Find out how loud each clip is
            CalculateClipLoudness();
        }
    }

    private void Update()
    {
        // This needs to funcion only as adding up how loud the player is being not calculating how loud each clip is
        //PlayerTotalLoudness();
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

    private void CalculateClipLoudness()
    {
        // Loops through all the samples and seaches each clip to calculate how loud it is.
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].clip.GetData(clipSampleData, audioSources[i].timeSamples);
            clipLoudness = 0f;

            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }

            clipLoudness /= sampleDataLength;
            print(clipLoudness);

            // Attach the name to how loud the clip is
            //sampleNames[i] = audioSources[i].name;
            //sampleLoudness[i] = clipLoudness;
        }

    }


    public void PlayerTotalLoudness()
    {
        // This function looks for what sound clips are playing and adds them to the total loudness.

        currentUpdateTime += Time.deltaTime;

        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;

            // Check what sound clips are playing
            for (int i = 0; i < sampleNames.Length; i++)
            {
                //if ()
                //{
                    // Add current clip at position i to total loudness
                    //totalLoudness += clipLoudness;
                    //print(totalLoudness);
                //}
                //else
                //{
                    // Subtract from total loudness if clip is not playing
                    //totalLoudness -= clipLoudness;
                //}
            }
        }
    }
}
