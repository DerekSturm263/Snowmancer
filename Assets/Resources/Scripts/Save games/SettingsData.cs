using UnityEngine;

[SerializeField]
public class SettingsData
{
    public bool isFullscreen;
    public bool useParticles;
    public bool usePostProcessing;
    public bool hasAntiAliasing;

    public float musicVolume;
    public float sfxVolume;

    public bool useHints;

    public SettingsData(bool[] boolValues, float[] floatValues)
    {
        isFullscreen = boolValues[0];
        useParticles = boolValues[1];
        usePostProcessing = boolValues[2];
        hasAntiAliasing = boolValues[3];
        useHints = boolValues[4];

        musicVolume = floatValues[0];
        sfxVolume = floatValues[1];
    }
}
