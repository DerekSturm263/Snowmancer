using UnityEngine;

public class SavingLoadingTitle : MonoBehaviour
{
    public string startScene;
    public Player startPlayerStats;
    public GameObject startCam;

    private void Awake()
    {
        try
        {
            if (!SaveLoad.isSave())
            {
                SaveSystem.SavePlayer(startPlayerStats);
                SaveSystem.SaveCamera(startCam.transform.position);
                SaveSystem.SaveScene(startScene);

                Debug.Log("Succesfully saved data to " + Application.persistentDataPath);
            }
        }
        catch(System.Exception E)
        {
            Debug.LogError(E);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            try
            {
                if (!SaveLoad.isSave())
                {
                    SaveSystem.SavePlayer(startPlayerStats);
                    SaveSystem.SaveCamera(startCam.transform.position);
                    SaveSystem.SaveScene(startScene);

                    Debug.Log("Succesfully saved data to " + Application.persistentDataPath);
                }
            }
            catch (System.Exception E)
            {
                Debug.LogError(E);
            }
        }
    }
}
