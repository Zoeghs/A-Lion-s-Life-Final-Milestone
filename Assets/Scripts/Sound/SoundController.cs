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
    private int sampleDataLength = 1024;
    private float clipLoudness;
    private float[] clipSampleData;
    [HideInInspector] public float totalLoudness = 0f;
    private SoundLoudnessStorage[] sampleData;

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

        // Instantiate sample date array to have enough room for every sample in the scene
        sampleData = new SoundLoudnessStorage[audioSources.Length];

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
        // Adds up how loud the player is being each frame
        PlayerTotalLoudness();
    }

    #region Playing Audio Upon Request
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
    #endregion

    #region Audio Loudness In The Scene At A Given Time
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

            // If the name of the clip matches one that requires an artificial boost
            float boost = AssignLoudnessBoost(i);

            // Attach the name to how loud the clip is using custom class
            sampleData[i] = new SoundLoudnessStorage(audioSources[i].name, clipLoudness, boost);
        }
    }

    private float AssignLoudnessBoost(int i)
    {
        float boost = 0;

        if (audioSources[i].name == "Sprinting")
        {
            boost = 1;
        }
        else if (audioSources[i].name == "Sprinting 2x")
        {
            boost = 1.5f;
        }
        else if (audioSources[i].name == "Sprinting 4x")
        {
            boost = 2;
        }
        // Anything that doesn't have one of the above names gets no boost
        else
        {
            boost = 0;
        }

        return boost;
    }

    public void PlayerTotalLoudness()
    {
        // This function looks for what sound clips are playing and adds them to the total loudness.

        totalLoudness = 0;

        // Find all currently active sound clips
        for (int i = 0; i < audioSources.Length; i++)
        {
            // If the sample is currently playing
            if (audioSources[i].isPlaying == true)
            {
                // Add it to the total loudness
                float combinedLoudness = sampleData[i].AddLoudness();
                totalLoudness += combinedLoudness;
            }
        }

        print(totalLoudness);
    }
    #endregion
}
