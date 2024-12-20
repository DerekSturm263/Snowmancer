﻿using UnityEngine;
using System.Collections.Generic;

public class SwitchIdle : MonoBehaviour // Really sorry for calling it SwitchIdle despite the fact that it also controls the footprints.
{
    private Animator anim;
    public Mesh footprintMesh;
    public Material footprintMaterial;

    private int footprintNum;
    private GameObject[] footprints = new GameObject[8];

    public bool isEnemy;

    private void Awake()
    {
        if (isEnemy) return;

        anim = GetComponent<Animator>();

        // Create footprints.
        for (int i = 0; i < 8; i++)
        {
            GameObject footprint = new GameObject("Footprint " + i);
            MeshFilter filter = footprint.AddComponent<MeshFilter>();
            MeshRenderer renderer = footprint.AddComponent<MeshRenderer>();

            filter.mesh = footprintMesh;
            renderer.material = footprintMaterial;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            footprint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            footprints[i] = footprint;
        }
    }

    public void SwitchIdlePose()
    {
        if (isEnemy)
            return;

        if (anim.GetLayerWeight(1) == 1f)
            return;

        anim.SetFloat("Idle Animation", Random.Range(-5, 4));
    }

    public void SetFootPrint(int footNum)
    {
        if (++footprintNum > 7)
            footprintNum = 0;

        Transform referenceTransform = footNum == 0 ? IKController.lFootGoal : IKController.rFootGoal;

        footprints[footprintNum].transform.position = referenceTransform.position - new Vector3(0f, 0.1f, 0f);
        footprints[footprintNum].transform.rotation = Quaternion.LookRotation(-referenceTransform.up, transform.forward);
    }

    private void Update()
    {
        if (isEnemy)
            return;

        if (anim.GetLayerWeight(1) == 1f)
            anim.SetFloat("Idle Animation", 0f);

        for (int i = 0; i < 8; i++)
        {
            footprints[i].GetComponent<MeshRenderer>().material.SetFloat("_Alpha", GetAlpha(i));
        }
    }

    private float GetAlpha(int posInArray)
    {
        return 1f;
        //return 1f - posInArray * 0.125f;
    }
}
