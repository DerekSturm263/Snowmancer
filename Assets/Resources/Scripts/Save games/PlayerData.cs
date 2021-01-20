using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    //player
    public float maxHealth;
    public float health;
    public float maxMana;
    public float mana;
    public float manaRegenSpeed;
    public float maxSnow;
    public float snowAmount;
    public bool aiming;
    public bool selectingSpell;
    public float vCurrExp;
    public float vLevel;
    public float vExpLeft;
    public Player.CurrentSpell currentSpell;
    public float[] targetPos;
    public float[] postition;




    public PlayerData(Player player)
    {
        //player
        maxHealth = player.maxHealth;
        health = player.health;
        maxMana = player.maxMana;
        mana = player.mana;
        maxSnow = player.maxSnow;
        snowAmount = player.snowAmount;
        aiming = player.aiming;
        selectingSpell = player.selectingSpell;
        currentSpell = player.currentSpell;
        vCurrExp = player.vCurrExp;
        vExpLeft = player.vExpLeft;
        vLevel = player.vLevel;

        //player Postition
        targetPos = new float[3];
        targetPos[0] = player.transform.position.x;
        targetPos[1] = player.transform.position.y;
        targetPos[2] = player.transform.position.z;

        postition = new float[3];
        postition[0] = player.transform.position.x;
        postition[1] = player.transform.position.y;
        postition[2] = player.transform.position.z;


    }

}