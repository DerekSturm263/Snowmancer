using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveLoad : MonoBehaviour
{
    private List <Enemy> enemy = new List<Enemy>();
    private MenuItemScript spellIcon;
    private Player player;
    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        spellIcon = GameObject.FindObjectOfType<MenuItemScript>();
        enemy = GameObject.FindObjectsOfType<Enemy>().ToList();
    }
    //Save/load system stuff
    public void SavePlayer()
    {
        Debug.Log("saved!");
        SaveSystem.SavePlayer(player);
        foreach (Enemy e in enemy)
        {
            SaveSystem.SaveEnemy(e);
        }
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPLayer();
        EnemyData eData = SaveSystem.LoadEnemy();
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
        //Enemies Info
        foreach (Enemy e in enemy)
        {
            e.maxHealth = eData.eMaxHealth;
            e.health = eData.eHealth;

            //postition
            Vector3 ePostition;
            ePostition.x = eData.ePostition[0];
            ePostition.y = eData.ePostition[1];
            ePostition.z = eData.ePostition[2];
            e.transform.position = ePostition;
        }

        //Player postition
        //change to teliport to postition not glide 
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


        //Spell icon change
        spellIcon.nameDisplay.text = spellIcon.runeName;
        spellIcon.background.color = spellIcon.hoverColor;
        spellIcon.currentSpell.sprite = spellIcon.icon;

    }
}
