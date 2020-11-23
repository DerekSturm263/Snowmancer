using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float maxMana;
    public float mana;
    public float maxSnow;
    public float snowAmount;

    private enum CurrentSpell { none, fire, ice, electric, air}
    [SerializeField]
    private CurrentSpell currentSpell;
    public GameObject snowball;
    public float minThrowStrength;
    public float maxThrowStrength;

    public float vCurrExp;
    public float vLevel;
    public float vExpLeft;
    public float vExpMod = 1.1f; //modifier that increases needed exp each level
    private float vExpBase; //exp req for level 1

    public bool aiming;
    public bool selectingSpell;

    public float slomoScale;
    public float slomoSmooth;

    private Animator playerAnimator;
    public MenuScript menuScript;
    private CameraController camScript;


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        //menuScript = FindObjectOfType<MenuScript>().GetComponent<MenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) { aiming = true; } else { aiming = false; }
        if (Input.GetKey(KeyCode.Tab))
        {
            selectingSpell = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.1f, slomoSmooth * Time.deltaTime);
        }
        else
        {
            SetCurrentSpell();
            selectingSpell = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slomoSmooth * Time.deltaTime);
        }

    }

    //Set current spell based on menuscript spell ID;
    void SetCurrentSpell()
    {
        switch (menuScript.spellID)
        {
            case 0:
                currentSpell = CurrentSpell.none;
                break;
            case 1:
                currentSpell = CurrentSpell.ice;
                break;
            case 2:
                currentSpell = CurrentSpell.fire;
                break;
            case 3:
                currentSpell = CurrentSpell.electric;
                break;
            case 4:
                currentSpell = CurrentSpell.air;
                break;
        }
    }
}
