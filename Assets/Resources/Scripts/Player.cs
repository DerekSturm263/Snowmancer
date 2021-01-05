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
    public float maxSnow;
    public float snowAmount;
    public bool aiming;
    public bool selectingSpell;

    [Header("Snowball Stuff")]
    public GameObject snowballPrefab;
    public GameObject snowballThrowPoint;
    public Camera cam;
    public float throwStrength;
    public float maxThrowRange;
    public float minSnowballSize;
    public float maxSnowballSize;
    public float chargeTime;
    public float snowDecreaseSpeed;
    public float snowCollectSpeed;
    private GameObject currentSnowball;
    private bool onSnow;

    public LayerMask snowballCollision;
    [SerializeField]
    private enum CurrentSpell { none, fire, ice, electric, air }
    private CurrentSpell currentSpell;

    [Header("Level Stuff")]
    public float vCurrExp;
    public float vLevel;
    public float vExpLeft;
    public float vExpMod = 1.1f; //modifier that increases needed exp each level
    private float vExpBase; //exp req for level 1

    public float slomoScale;
    public float slomoSmooth;

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
                    if (Input.GetKey(KeyCode.Tab)) { currentState = State.SELECTSPELL; }
                    if (Input.GetKey(KeyCode.R)) { currentState = State.COLLECTINGSNOW; }
                        break;

                case State.AIMING:
                    aiming = true;
                    if(Input.GetMouseButtonDown(0) && snowAmount > 0 && currentSnowball == null) //Left Down
                    {
                        GameObject ball = Instantiate(snowballPrefab, snowballThrowPoint.transform.position, Quaternion.identity);
                        currentSnowball = ball;
                        currentSnowball.transform.parent = snowballThrowPoint.transform;
                        currentSnowball.GetComponent<Rigidbody>().isKinematic = true;
                        currentState = State.CHARGING;
                    }
                    if (!Input.GetMouseButton(1)) { aiming = false; currentState = State.IDLE; }
                    break;

                case State.CHARGING:
                    if (!Input.GetMouseButton(0) && currentSnowball != null)
                    {
                        currentSnowball.GetComponent<Rigidbody>().isKinematic = false;
                        currentSnowball.transform.parent = null;
                        ThrowSnowball(new Vector3(1, 1, 1), currentSpell);
                        currentSnowball = null;
                        currentState = State.AIMING;
                    }
                    if (currentSnowball == null)
                    {
                        currentState = State.AIMING;
                    }
                    if (snowAmount > 0 && currentSnowball != null)
                    {
                        currentSnowball.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f) * (Time.deltaTime);
                        decreaseSnowAmount(snowDecreaseSpeed * Time.deltaTime);
                    }
                    break;

                case State.SELECTSPELL:
                    selectingSpell = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = Mathf.Lerp(Time.timeScale, 0.1f, slomoSmooth * Time.deltaTime);
                    if (Input.GetKeyUp(KeyCode.Tab))
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
            //Debug.Log(currentState);
        }

        // When the player releases the throw button, it will set a bool in the animator to play the throw animation.
        if (Input.GetMouseButtonUp(1))
            playerAnimator.SetBool("Release Snowball", true);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Snow")
        {
            onSnow = true;
        }
        else onSnow = false;
    }
    //Throw
    void ThrowSnowball(Vector3 size, CurrentSpell element)
    {
        //if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Strafing") || !playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
        //    return; // Right now I'm making it so that you can't throw another snowball if you're still in the animation but that's more for testing purposes and we can tweak the cooldown time later.

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
        SetSnowballElement(snowball, element);
        rb.velocity = CalculateV(target, snowballThrowPoint.transform.position, time);
        Debug.Log(rb.velocity);
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
        mana -= mana;
    }
}
