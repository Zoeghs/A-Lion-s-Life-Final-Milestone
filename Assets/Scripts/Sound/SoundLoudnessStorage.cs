using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoudnessStorage
{
    public string name;
    public float loudness;

    // How much extra to add or subtract on top of the original sound value
    public float loudnessOffset;

    public SoundLoudnessStorage(string newName, float newLoudness, float newLoudnessOffset)
    {
        name = newName;
        loudness = newLoudness;
        loudnessOffset = newLoudnessOffset;
    }

    public float AddLoudness()
    {
        float combinedLoudness = loudness + loudnessOffset;
        return combinedLoudness;
    }
}
