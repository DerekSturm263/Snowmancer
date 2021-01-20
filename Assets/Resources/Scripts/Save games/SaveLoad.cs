using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using UnityEngine.Rendering.Universal;

public class SaveLoad : MonoBehaviour
{
    private Camera camera;
    private MenuItemScript spellIcon;
    private Player player;

    public static bool isSave()
    {
        return File.Exists(Application.persistentDataPath + "/player.savedata");
    }
    private void Awake()
    {
        camera = GameObject.FindObjectOfType<Camera>();
        player = GameObject.FindObjectOfType<Player>();
        spellIcon = GameObject.FindObjectOfType<MenuItemScript>();

        if (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 2" || SceneManager.GetActiveScene().name == "Level 3 and 4")
        {
            SaveSystem.LoadPLayer();
           // SaveSystem.LoadCamera();
        }

    }
    //Save/load system stuff
    public void SavePlayer()
    {
        Debug.Log("saved!");
        SaveSystem.SavePlayer(player);
       // SaveSystem.SaveCamera(camera.transform.position);

    }

    public void LoadPlayer()
    {
      //  CamData camData = new CamData();
       // camData.camPos = SaveSystem.LoadCamera();
        PlayerData data = SaveSystem.LoadPLayer();
        Debug.Log("loading...");

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

        ////camera Info
        //Vector3 camPos;
        //camPos.x = camData.camPos[0];
        //camPos.y = camData.camPos[1];
        //camPos.z = camData.camPos[2];

        //Player postition
        Vector3 targetPos;
        targetPos.x = data.postition[0];
        targetPos.y = data.postition[1];
        targetPos.z = data.postition[2];
        player.GetComponent<PlayerMovement>().targetPos = targetPos;

        Vector3 postition;
        postition.x = data.postition[0];
        postition.y = data.postition[1];
        postition.z = data.postition[2];
        transform.position = postition;

       
        //SceneManager.
        //SceneManager.MoveGameObjectToScene(player,//saved scene);


    }
}






//private List <Enemy> enemy = new List<Enemy>();

//enemy = GameObject.FindObjectsOfType<Enemy>().ToList();

//foreach (Enemy e in enemy)
//{
//    SaveSystem.SaveEnemy(e);
//}

//EnemyData eData = SaveSystem.LoadEnemy();

//Enemies Info
//foreach (Enemy e in enemy)
//{
//    e.maxHealth = eData.eMaxHealth;
//    e.health = eData.eHealth;

//    //postition
//    Vector3 ePostition;
//    ePostition.x = eData.ePostition[0];
//    ePostition.y = eData.ePostition[1];
//    ePostition.z = eData.ePostition[2];
//    e.transform.position = ePostition;
//}

//Spell icon change
//spellIcon.nameDisplay.text = spellIcon.runeName;
//spellIcon.background.color = spellIcon.hoverColor;
//spellIcon.currentSpell.sprite = spellIcon.icon;
