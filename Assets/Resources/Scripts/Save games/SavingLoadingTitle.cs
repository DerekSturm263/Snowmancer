using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingLoadingTitle : MonoBehaviour
{
    public string scene;
    public GameObject camera;
    private Player player;


    private void Awake()
    {
        player = GetComponent<Player>();
        try
        {

            Debug.Log(Application.persistentDataPath);

            if (!SaveLoad.isSave())
            {
                SaveSystem.SavePlayer(player);
                SaveSystem.SaveCamera(camera.transform.position);
                SaveSystem.SaveScene(scene);
                Debug.Log("Saved Data");
            }
        }catch(System.Exception E)
        {
            Debug.LogError(E);
        }

       


    }


}
