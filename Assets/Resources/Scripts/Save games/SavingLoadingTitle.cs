using UnityEngine;

public class SavingLoadingTitle : MonoBehaviour
{
    public string startScene;
    public Player startPlayerStats;
    public GameObject startCam;
    public bool[] elementStartData = new bool[5];

    private void Awake()
    {
        try
        {
            if (!SaveLoad.isSave())
            {
                SaveSystem.SavePlayer(startPlayerStats);
                SaveSystem.SaveCamera(startCam.transform.position);
                SaveSystem.SaveScene(startScene);
                SaveSystem.SaveElementData(elementStartData);

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                SaveSystem.SavePlayer(startPlayerStats);
                SaveSystem.SaveCamera(startCam.transform.position);
                SaveSystem.SaveScene(startScene);
                SaveSystem.SaveElementData(elementStartData);

                Debug.Log(SaveSystem.LoadScene());
                Debug.Log("Succesfully saved data to " + Application.persistentDataPath);
            }
            catch (System.Exception E)
            {
                Debug.LogError(E);
            }
        }
    }
}
