using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Boss : MonoBehaviour
{
    public LayerMask ground;

    public GameObject player;
    [HideInInspector] public Animator anim;

    public enum ElementType
    {
        Fire, Electric, Wind, All
    }
    public ElementType type;

    public float maxHealth;
    public float health;
    public float spawnRange;

    [HideInInspector] public int attackNum = 0;
    public List<Action> attacks = new List<Action>();
    public BossAttack currentAttack;

    public float timeSinceLastAttack = 0.1f;
    public float timeBetweenAttacks;

    public Dictionary<int, Action> phaseFeatures = new Dictionary<int, Action>(); // Keeps track of what phase the boss needs to be on to add a new attack/modify an old one.

    [HideInInspector] public List<Material> materials = new List<Material>();

    [HideInInspector] public bool active = false;

    [HideInInspector] public Transform wandTip;

    public GameObject runeDrop;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        wandTip = GetComponentInChildren<WandTip>().transform;

        anim = GetComponent<Animator>();

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }
    }

    public void TryNewFeature(int key)
    {
        try
        {
            phaseFeatures[key].Invoke();
        }
        catch { }
    }

    public void TakeDamage(float damage)
    {
        for (int i = 0; i < damage; i++)
        {
            health--;
            TryNewFeature((int)((maxHealth - health) / maxHealth));
        }

        health -= damage;
        StartCoroutine(DamageFlash());
        
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            FindObjectOfType<EnterNextLevel>().active = true;
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

    public Vector3 GetSummonSpot(Vector3 startPos, float range)
    {
        Vector3 newPos = startPos + new Vector3(UnityEngine.Random.Range(-range, range), 0f, UnityEngine.Random.Range(-range, range));

        if (Physics.Linecast(newPos, newPos + Vector3.down * 10f, out RaycastHit hit, ground))
        {
            return hit.point;
        }

        return newPos;
    }

    public void OnDestroy()
    {
        Instantiate(runeDrop);
    }
}
