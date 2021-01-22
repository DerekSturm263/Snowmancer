using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public bool isFullscreen;
    public bool useParticles;
    public bool usePostProcessing;
    public bool hasAntiAliasing;

    public float musicVolume;

    public bool useHints;

    public SettingsData(bool[] boolValues, float musicVal)
    {
        isFullscreen = boolValues[0];
        useParticles = boolValues[1];
        usePostProcessing = boolValues[2];
        hasAntiAliasing = boolValues[3];
        useHints = boolValues[4];

        musicVolume = musicVal;
    }
}
