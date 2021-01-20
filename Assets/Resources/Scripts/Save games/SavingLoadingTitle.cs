using UnityEngine;

public class SavingLoadingTitle : MonoBehaviour
{
    public string startScene;
    public Player startPlayerStats;
    public GameObject startCam;
    public bool[] elementStartData = new bool[5];

    public static bool resetSave = false;

    public bool[] defaultBoolValues = new bool[5];
    public float[] defaultFloatValues = new float[2];

    private void Awake()
    {
        try
        {
            if (!SaveLoad.isSave() || resetSave)
            {
                resetSave = false;

                SaveSystem.SavePlayer(startPlayerStats);
                SaveSystem.SaveCamera(startCam.transform.position);
                SaveSystem.SaveScene(startScene);
                SaveSystem.SaveElementData(elementStartData);
                SaveSystem.SaveSettingsData(defaultBoolValues, defaultFloatValues);

                Debug.Log("Succesfully saved data to " + Application.persistentDataPath);
            }
        }
        catch(System.Exception E)
        {
            Debug.LogError(E);
        }
    }
}
