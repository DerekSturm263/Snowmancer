using UnityEngine;

public class SavingLoadingTitle : MonoBehaviour
{
    public static bool deleteSave = false;

    public string startScene;
    public Player startPlayerStats;
    public GameObject startCam;
    public bool[] elementStartData = new bool[5];

    public bool[] defaultBoolValues = new bool[5];
    public float defaultMusicValue;

    private readonly bool fireAliveDefault = true;
    private readonly bool electricAliveDefault = true;
    private readonly bool windAliveDefault = true;
    private readonly bool finalSpawnedDefault = false;
    private readonly bool finalAliveDefault = true;

    private void Awake()
    {
        try
        {
            if (deleteSave)
            {
                deleteSave = false;

                DeleteSave();
            }
        }
        catch (System.Exception E)
        {
            Debug.LogError(E);
        }
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteSaveData();
    }

    public void NewSave()
    {
        SaveSystem.SavePlayer(startPlayerStats);
        SaveSystem.SaveCamera(startCam.transform.position);
        SaveSystem.SaveScene(startScene);
        SaveSystem.SaveElementData(elementStartData);
        SaveSystem.SaveSettingsData(defaultBoolValues, defaultMusicValue);
        SaveSystem.SaveBossData(fireAliveDefault, electricAliveDefault, windAliveDefault, finalSpawnedDefault, finalAliveDefault);

        Debug.Log("Succesfully saved data to " + Application.persistentDataPath);
    }
}
