﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    private enum State { IDLE, AIMING, CHARGING, SELECTSPELL, COLLECTINGSNOW }
    private State currentState;

    [Header("Player Stats")]
    public float maxHealth;
    public float health;
    public float maxMana;
    public float mana;
    public float manaRegenSpeed;
    public float maxSnow;
    public float snowAmount;
    public bool aiming;
    public bool selectingSpell;

    [Header("Snowball Stuff")]
    public GameObject snowballPrefab;
    public GameObject snowballThrowPoint;
    public Camera cam;
    public float throwBaseDamage;
    public float sizeDamageMultiplier;
    public float throwStrength;
    public float maxThrowRange;
    public float snowballIncreaseSpeed; //how fast snowball increases in size
    public float minSnowballSize;
    public float maxSnowballSize;
    public float snowDecreaseSpeed;
    public float snowCollectSpeed;
    private GameObject currentSnowball;
    private float currentSnowballDamage;
    public bool onSnow;

    public LayerMask snowballCollision;
    [SerializeField]
    public enum CurrentSpell { none, fire, ice, electric, air }
    public CurrentSpell currentSpell;

    [Header("Level Stuff")]
    public float vCurrExp;
    public float vLevel;
    public float vExpLeft;
    public float vExpMod = 1.1f; //modifier that increases needed exp each level
    private float vExpBase; //exp req for level 1

    public float slomoScale;
    public float slomoSmooth;

    //cd's
    private float throwCD;

    private Animator playerAnimator;
    public UIController uiCont;
    public MenuScript menuScript;
    private CameraController camScript;

    [HideInInspector] public List<Material> materials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        //menuScript = FindObjectOfType<MenuScript>().GetComponent<MenuScript>();
        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }
        currentState = State.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiCont.CheckPaused())
        {
            switch (currentState)
            {
                case State.IDLE:
                    if (Input.GetMouseButton(1)) { currentState = State.AIMING; }
                    if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.E)) { currentState = State.SELECTSPELL; }
                    if (Input.GetKey(KeyCode.R)) { currentState = State.COLLECTINGSNOW; }
                        break;

                case State.AIMING:
                    aiming = true;
                    if(Input.GetMouseButtonDown(0) && snowAmount > 0 && currentSnowball == null && throwCD <= 0 && CheckManaCost()) //Prepare to throw snowball
                    {
                        GameObject ball = Instantiate(snowballPrefab, snowballThrowPoint.transform.position, Quaternion.identity);
                        currentSnowball = ball;
                        SetSnowballElement(currentSnowball.GetComponent<Snowball>(), currentSpell);
                        currentSnowball.transform.parent = snowballThrowPoint.transform;
                        currentSnowball.GetComponent<Rigidbody>().isKinematic = true;
                        currentState = State.CHARGING;
                    }
                    if (!Input.GetMouseButton(1)) { aiming = false; currentState = State.IDLE; }
                    break;

                case State.CHARGING:
                    playerAnimator.SetLayerWeight(1, 1f);
                    playerAnimator.SetBool("Release Snowball", false);
                    playerAnimator.SetFloat("Snowball Size", currentSnowball.transform.localScale.x );
                    currentSnowballDamage = throwBaseDamage * currentSnowball.transform.lossyScale.x * sizeDamageMultiplier;
                    Debug.Log(currentSnowballDamage); //Use toCheck damage realtime.
                    if (!Input.GetMouseButton(0) && currentSnowball != null)
                    {
                        playerAnimator.SetBool("Release Snowball", true);
                    }
                    if (currentSnowball == null)
                    {
                        currentState = State.AIMING;
                    }
                    if (snowAmount > 0 && currentSnowball != null && currentSnowball.transform.localScale.x < maxSnowballSize)
                    {
                        currentSnowball.transform.localScale += new Vector3(snowballIncreaseSpeed, snowballIncreaseSpeed, snowballIncreaseSpeed) * (Time.deltaTime);
                        decreaseSnowAmount(snowDecreaseSpeed * Time.deltaTime);
                    }
                    break;

                case State.SELECTSPELL:
                    selectingSpell = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = Mathf.Lerp(Time.timeScale, 0.1f, slomoSmooth * Time.deltaTime);
                    if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.E))
                    {
                        SetCurrentSpell();
                        selectingSpell = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        currentState = State.IDLE;
                    }
                    break;

                case State.COLLECTINGSNOW:
                    if (onSnow && snowAmount < maxSnow && Input.GetKey(KeyCode.R) && playerAnimator.GetBool("Grounded"))
                    {
                        increaseSnowAmount(snowCollectSpeed * Time.deltaTime);
                        playerAnimator.SetBool("Gathering Snow", true);
                    }
                    else
                    {
                        currentState = State.IDLE;
                        playerAnimator.SetBool("Gathering Snow", false);
                    }
                    break;
            }
            if (!selectingSpell)
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slomoSmooth * Time.deltaTime);
                }
            }
            //passive mana regen
            if(mana < maxMana)
            {
                mana += manaRegenSpeed * Time.deltaTime;
            }
            LimitStats();
            updateCD();
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Snow")
        {
            onSnow = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        onSnow = true;
    }
    //Throw
    public void AnimationThrowSnowball()
    {
        ThrowSnowball(currentSnowball.transform.localScale, currentSnowballDamage);
        currentSnowball.GetComponent<Rigidbody>().isKinematic = false;
        currentSnowball.transform.parent = null;
        currentSnowball = null;
        currentState = State.AIMING;
        playerAnimator.SetFloat("Snowball Size", 0f);
    }

    void ThrowSnowball(Vector3 size, float damage)
    {
        RaycastHit hit;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 target;
        float hitDistance;
        if (Physics.Raycast(camRay, out hit, 100f, snowballCollision) && (hit.point - cam.transform.position).magnitude < maxThrowRange)
        {
            target = hit.point;
            hitDistance = (hit.point - cam.transform.position).magnitude;
        }
        else
        {
            target = (cam.transform.position) + (cam.transform.forward * maxThrowRange);
            hitDistance = maxThrowRange;
        }
        Rigidbody rb = currentSnowball.GetComponent<Rigidbody>();
        Snowball snowball = currentSnowball.GetComponent<Snowball>();
        Debug.Log(target);

        float time = (hitDistance / 100) * throwStrength;
        snowball.damage = damage;
        switch (snowball.currentSpell)
        {
            default:
                rb.velocity = CalculateV(target, snowballThrowPoint.transform.position, time);
                break;
            case Snowball.CurrentSpell.air:
                rb.useGravity = false;
                rb.velocity = CalculateAirV(target, snowballThrowPoint.transform.position);
                mana -= 33f;
                break;
        }
        Debug.Log(rb.velocity);
        throwCD = 0.5f;
        // I'm hard setting this right now, but when make the snowballs grow bigger, we will set this to be the size of the current snowball.
        // A quick click will immediately play the throw animation, while a long press will begin to play the building animation.

        playerAnimator.SetLayerWeight(1, 1f); // Later on I'm gonna make this set layer 2 if the player isn't moving.
        playerAnimator.SetFloat("Snowball Size", 1f);
        playerAnimator.speed = 1.1f - playerAnimator.GetFloat("Snowball Size") / 10f; // Slow down the animations based on the size of the snowball.
    }
    Vector3 CalculateV(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distance_x_z = distance;
        distance_x_z.Normalize();
        distance_x_z.y = 0;

        //creating a float that represents our distance 
        float sy = distance.y;
        float sxz = distance.magnitude;

        //calculating initial x velocity
        float Vxz = sxz / time;

        ////calculating initial y velocity
        float Vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z * Vxz;
        result.y = Vy;

        return result;
    }
    Vector3 CalculateAirV(Vector3 target, Vector3 origin)
    {
        Vector3 dir = (target - origin).normalized;
        Vector3 v = dir * 50f;
        return v;
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
    void SetSnowballElement(Snowball snowball, CurrentSpell element)
    {
        switch (element)
        {
            case CurrentSpell.none:
                snowball.currentSpell = Snowball.CurrentSpell.none;
                break;
            case CurrentSpell.ice:
                snowball.currentSpell = Snowball.CurrentSpell.ice;
                break;
            case CurrentSpell.fire:
                snowball.currentSpell = Snowball.CurrentSpell.fire;
                break;
            case CurrentSpell.electric:
                snowball.currentSpell = Snowball.CurrentSpell.electric;
                break;
            case CurrentSpell.air:
                snowball.currentSpell = Snowball.CurrentSpell.air;
                break;
        }
    }
    public void decreaseSnowAmount(float amount)
    {
        snowAmount -= amount;
    }
    public void increaseSnowAmount(float amount)
    {
        snowAmount += amount;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DamageFlash());

        if (health <= 0)
        {
            playerAnimator.SetTrigger("Death");
        }
    }

    private IEnumerator DamageFlash()
    {
        for (float i = 0f; i < 0.15f; i += Time.deltaTime)
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_DamageWeight", i * 6.666666666666667f);
            }

            yield return new WaitForEndOfFrame();
        }

        for (float i = 0.35f; i > 0f; i -= Time.deltaTime)
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_DamageWeight", i * 2.85714286f);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void AddHealth(float health)
    {
        health += health;
    }

    public void AddMana(float mana)
    {
        mana += mana;
    }

    private void LimitStats()
    {
        if(health < 0) { health = 0; }
        else if(health > maxHealth) { health = maxHealth; }
        if(mana < 0) { mana = 0; }
        else if(mana > maxMana) { mana = maxMana; }
        if(snowAmount < 0) { snowAmount = 0; }
        else if (snowAmount > maxSnow) { snowAmount = maxSnow; }
    }

    private void updateCD()
    {
        if(throwCD > 0)
        {
            throwCD -= Time.deltaTime;
        }
    }
    private bool CheckManaCost()
    {
        bool i = true;
        switch (currentSpell)
        {
            case CurrentSpell.none:
                i = true;
                break;
            case CurrentSpell.ice:
                if (mana > 33) { i = true; }
                else i = false;
                break;
            case CurrentSpell.fire:
                if (mana > 33) { i = true; }
                else i = false;
                break;
            case CurrentSpell.electric:
                if (mana > 33) { i = true; }
                else i = false;
                break;
            case CurrentSpell.air:
                if (mana > 33) { i = true; }
                else i = false;
                break;
        }
        return i;
    }

}
