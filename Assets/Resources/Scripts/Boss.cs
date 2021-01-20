using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Boss : MonoBehaviour
{
    public LayerMask ground;
    private UIController ui;

    public string name;
    public GameObject player;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rb;

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

    private int phaseIndex = 0;
    public Dictionary<int, Action> phaseFeatures = new Dictionary<int, Action>(); // Keeps track of what phase the boss needs to be on to add a new attack/modify an old one.

    [HideInInspector] public List<Material> materials = new List<Material>();

    [HideInInspector] public bool active = false;

    [HideInInspector] public Transform wandTip;

    public GameObject runeDrop;

    public bool newSpot = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        wandTip = GetComponentInChildren<WandTip>().transform;
        ui = FindObjectOfType<UIController>();
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }
    }

    private void Update()
    {
        ui.SetBossHealth();
    }

    public void TryNewFeature(int key)
    {
        try
        {
            phaseFeatures[key].Invoke();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void TakeDamage(float damage)
    {
        if (type == ElementType.Fire)
        {
            if (currentAttack != BossAttacks.fireBossSpellBig1 && currentAttack != BossAttacks.fireBossSpellBig2 && !anim.GetBool("Dazed"))
                return;
        }

        if (!active)
            return;

        if (type == ElementType.Fire)
        {
            if (damage > 25f)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dazed"))
                {
                    Debug.Log("Dazed");
                    anim.SetBool("Dazed", true);
                    Invoke("UnDaze", 4f);
                    ResetAttack();

                    TryNewFeature(++phaseIndex);
                }
            }
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dazed"))
                    damage /= 2f;
                else
                    damage = 0f;
            }
        }
        else if (type == ElementType.Wind)
        {
            if (damage > 25f && anim.GetBool("Charging")) // Make bigger once we add leveling up.
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
                {
                    damage *= 2f;
                    anim.SetBool("Dazed", true);
                    Invoke("UnDaze", 6f);
                    ResetAttack();

                    TryNewFeature(++phaseIndex);
                }
            }
        }
        else
        {
            if (anim.GetLayerWeight(1) == 1f)
            {
                damage = 0f;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Stuck"))
            {
                damage *= 1.5f;
            }
        }

        if (damage > 0f)
        {
            health -= damage;
            StartCoroutine(DamageFlash());
        }

        if (health <= 0f)
        {
            try
            {
                anim.SetBool("Dead", true);
            } catch { }

            anim.SetTrigger("Death");
            ui.HideBossHealth();
            FindObjectOfType<EnterNextLevel>().active = true;
            this.enabled = false;
        }
    }

    public void TakeIcicleDamage(float damage)
    {
        if (!active)
            return;

        anim.SetBool("Dazed", true);
        anim.SetBool("Charging Spell", false);
        anim.SetBool("Charging Stomp", false);
        anim.SetLayerWeight(1, 0f);
        Invoke("UnDaze", 6f);
        ResetAttack();

        TryNewFeature(++phaseIndex);

        if (damage > 0f)
        {
            health -= damage;
            StartCoroutine(DamageFlash());
        }

        if (health <= 0f)
        {
            try
            {
                anim.SetBool("Dead", true);
            }
            catch { }

            anim.SetTrigger("Death");
            ui.HideBossHealth();
            FindObjectOfType<EnterNextLevel>().active = true;
            this.enabled = false;
        }
    }

    private void UnDaze()
    {
        anim.SetBool("Dazed", false);
        newSpot = true;

        if (type == ElementType.Electric)
            timeSinceLastAttack = 0.1f;
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
        if (runeDrop != null)
            Instantiate(runeDrop, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);

        if (type == ElementType.Wind)
            GameObject.FindGameObjectsWithTag("Boss")[0].SetActive(true);
    }

    public void ShowHealth()
    {
        ui.ShowBossHealth(name);
        ui.SetMaxBossHealth();
    }

    public void ResetAttack()
    {
        if (currentAttack != null)
        {
            try
            {
                currentAttack.Spell.Despawn();
            } catch { }

            currentAttack = null;
        }
    }
}
