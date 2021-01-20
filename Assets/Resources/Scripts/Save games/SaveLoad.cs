using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public GameObject camera;
    public Player player;

    public static bool isSave()
    {
        return File.Exists(Application.persistentDataPath + "/player.savedata");
    }

    private void Awake()
    {
        LoadPlayer();
    }

    //Save/load system stuff
    public void SavePlayer()
    {
        Debug.Log("Saved succesfully to " + Application.persistentDataPath);

        SaveSystem.SavePlayer(player);
        SaveSystem.SaveCamera(camera.transform.position);
    }

    public void LoadPlayer()
    {
        camera.transform.position = SaveSystem.LoadCamera();
        PlayerData data = SaveSystem.LoadPLayer();

        Debug.Log("Loading...");

        //player Info
        player.maxHealth = data.maxHealth;
        player.health = data.health;
        player.maxMana = data.maxMana;
        player.mana = data.mana;
        player.maxSnow = data.maxSnow;
        player.snowAmount = data.snowAmount;
        player.aiming = data.aiming;
        player.selectingSpell = data.selectingSpell;
        player.currentSpell = data.currentSpell;
        player.vLevel = data.vLevel;
        player.vExpLeft = data.vExpLeft;
        player.vCurrExp = data.vCurrExp;

        Vector3 targetPos;
        targetPos.x = data.targetPos[0];
        targetPos.y = data.targetPos[1];
        targetPos.z = data.targetPos[2];
        player.GetComponent<PlayerMovement>().targetPos = targetPos;

        Vector3 postition;
        postition.x = data.postition[0];
        postition.y = data.postition[1];
        postition.z = data.postition[2];
        player.transform.position = postition;
    }
}