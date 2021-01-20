using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraController : MonoBehaviour
{
    public float CamMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 FollowPOS;
    public float clampAngle = 80.0f;
    public float minDist = -8;
    public float maxDist = -4;
    public float inputSensitivity = 150.0f;
    public float scrollSensitivity = 1f;
    public float scrollSmooth = 7f;
    public float collideSmooth = 15f;
    public float collideDistance = 0.1f;
    public LayerMask camCollideWith;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public GameObject OffsetCenter;
    public Vector3 CamOffset;
    public float camDistance;
    public float mouseX;
    public float mouseY;
    public float aimFOV;
    public float aimSens;  //Leave as zero if you want it to be automatically set to half of normal
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    private Camera CameraComponent;
    private float tempSens;
    private float savedCamDist;
    private Player playerScript;
    public GameObject follow2;

    public static bool isActive = true;

    [HideInInspector] public GameObject defaultFollowOb;

    void Start()
    {
        defaultFollowOb = CameraFollowObj;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        CameraObj.transform.localPosition = CamOffset;
        OffsetCenter.transform.localPosition = CamOffset;
        CameraComponent = CameraObj.GetComponent<Camera>();
        tempSens = inputSensitivity;
        playerScript = PlayerObj.GetComponent<Player>();
        if (aimSens == 0)
        {
            aimSens = inputSensitivity * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        //rotation
        if (!playerScript.selectingSpell) //if selecting spell, don't rotate
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            rotY += mouseX * inputSensitivity * Time.deltaTime;
            rotX += -mouseY * inputSensitivity * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
        }

        //Collision & Cam distance scrolling
        camDistance = Mathf.Clamp(camDistance, 4f, maxDist);
        savedCamDist += scrollSensitivity * Input.mouseScrollDelta.y;
        savedCamDist = Mathf.Clamp(savedCamDist, minDist, maxDist);

        Vector3 camDir = CameraObj.transform.position - OffsetCenter.transform.position;
        camDir = camDir.normalized;
        RaycastHit camHit;
        if (Physics.Linecast(transform.position, OffsetCenter.transform.position + (camDir * -savedCamDist) , out camHit, camCollideWith) && -camHit.distance > (savedCamDist - collideDistance) )
        {
            CameraObj.transform.localPosition = Vector3.Lerp(CameraObj.transform.localPosition, new Vector3(CamOffset.x, CamOffset.y, -camHit.distance + collideDistance), collideSmooth * Time.deltaTime);
        }
        else
        {
            CameraObj.transform.localPosition = Vector3.Lerp(CameraObj.transform.localPosition, new Vector3(CamOffset.x, CamOffset.y, savedCamDist), scrollSmooth * Time.deltaTime);
        }

        //Aim
        if (playerScript.aiming)
        {
            CameraComponent.fieldOfView = Mathf.Lerp(CameraComponent.fieldOfView, aimFOV, 10 * Time.deltaTime);
            inputSensitivity = aimSens;
        }
        else
        {
            CameraComponent.fieldOfView = Mathf.Lerp(CameraComponent.fieldOfView, 60, 10 * Time.deltaTime);
            inputSensitivity = tempSens;
        }
    }
    private void LateUpdate()
    {
        CameraUpdater();
    }
    void CameraUpdater()
    {
        //set the target object to follow
        Transform target = CameraFollowObj.transform;

        //move towards the game obj thats the target
        float step = CamMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
