using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Boss : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    public enum ElementType
    {
        Fire, Electric, Wind, All
    }
    public ElementType type;

    public float maxHealth;
    public float health;
    public float spawnRange;

    public float attackChargeTime;

    public float damage;
    public float magicAttackSpeed;
    public float attackSize;
    public float attackLifetime;

    public Enemy.ElementType attackType;

    public Dictionary<float, Action> phaseFeatures = new Dictionary<float, Action>(); // Keeps track of what phase the boss needs to be on to add a new attack/modify an old one.

    [HideInInspector] public List<Material> materials = new List<Material>();

    [HideInInspector] public bool active = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }
    }

    public void NextPhase()
    {
        try
        {
            phaseFeatures[maxHealth - health].Invoke();
        }
        catch { }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DamageFlash());

        NextPhase();

        if (health <= 0)
        {
            anim.SetTrigger("Death");

            anim.SetLayerWeight(1, 0f);
            anim.SetLayerWeight(2, 0f);

            this.enabled = false;
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
}
