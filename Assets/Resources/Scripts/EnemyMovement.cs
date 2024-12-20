﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyMovement : Movement
{
    public static Vector3 playerHeadPosition;

    private GameObject player;
    private Enemy enemy;
    public GameObject spell;

    private Vector2 moveDir;

    public float followDist; // Enemy will not notice the player until they are this far away.
    public float distFromPlayer; // Enemy will move towards player until they are this far away.
    private bool isTargeting = false; // Enemy will move towards player and attack if true.

    public float spawnRange;

    public GameObject spawnParticles;
    public GameObject enemyToSummon;

    [HideInInspector] public float targetLayerWeight;

    private Vector3 spawnPosition;

    private GameObject currentSpawningParticles;

    public GameObject hitEffect;

    private GameObject healthPotion;

    public float timeBurnt;
    public float burnDamage;
    public float shockTime;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>().gameObject;
        healthPotion = Resources.Load<GameObject>("Prefabs/HealthP");

        anim.SetBool("Move While Charging", enemy.moveWhileAttacking);
        spawnPosition = transform.position;
    }

    private void Update()
    {
        Quaternion targetRotation = Quaternion.identity;
        mouseAim = false;

        #region Moving

        if (Vector3.Distance(player.transform.position, transform.position) < followDist)
            isTargeting = true;

        if (Vector3.Distance(player.transform.position, transform.position) > followDist * 1.5f)
            isTargeting = false;

        if (isTargeting)
        {
            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

            if (Vector3.Distance(player.transform.position, transform.position) > distFromPlayer + 0.5f)
            {
                anim.SetFloat("Speed", 1f);
            }
            else if (Vector3.Distance(player.transform.position, transform.position) < distFromPlayer - 0.5f)
            {
                anim.SetFloat("Speed", -1f);
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, spawnPosition) < 5f)
            {
                moveDir.x += Time.deltaTime * Random.Range(-15f, 15f);
                moveDir.y += Time.deltaTime * Random.Range(-15f, 15f);

                moveDir = moveDir.normalized;
                targetRotation = Quaternion.LookRotation(new Vector3(moveDir.x, 0f, moveDir.y), Vector3.up);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(new Vector3(spawnPosition.x - transform.position.x, 0f, spawnPosition.z - transform.position.z), Vector3.up);
            }

            anim.SetFloat("Speed", 1f);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z), Time.deltaTime * 2.5f);

        #endregion

        // Attacking
        if (Vector3.Distance(player.transform.position, transform.position) < distFromPlayer + 0.5f && Vector3.Distance(player.transform.position, transform.position) > distFromPlayer - 0.5f)
            Attack();

        anim.SetBool("Grounded", true);
        anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, Mathf.Lerp(anim.GetLayerWeight(enemy.moveWhileAttacking ? 1 : 2), targetLayerWeight, Time.deltaTime * 10f));

        #region Fire Damage
        if (statusEffect == StatusEffect.Burnt)
        {
            enemy.health -= Time.deltaTime * 2.5f;
            timeBurnt -= Time.deltaTime;

            if (enemy.health <= 0f)
                anim.SetTrigger("Dead");
        }

        if (timeBurnt < 0f)
        {
            timeBurnt = 0f;
            statusEffect = StatusEffect.None;

            materials.ToList().ForEach(x =>
            {
                x.SetFloat("_DamageWeight", 0f);
            });
        }
        #endregion

        #region Freeze Time

        if (statusEffect == StatusEffect.Frozen)
        {
            iceLeft -= Time.deltaTime;
            anim.speed = 0.01f;
        }
        else
        {
            anim.speed = 1;
        }

        if (iceLeft < 0f)
        {
            anim.speed = 0.25f;
            iceLeft = 0f;
            statusEffect = StatusEffect.None;

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetColor("_Tint", materialColors[i]);
                materials[i].SetFloat("_Smoothness", materialFloats[i]);
            }
        }

        #endregion


        #region Shock Time
        if (statusEffect == StatusEffect.Shocked)
        {
            shockTime -= Time.deltaTime;
            anim.SetBool("Shocked", true);
        }

        if (shockTime < 0f)
        {
            anim.SetBool("Shocked", false);
            shockTime = 0f;
            statusEffect = StatusEffect.None;
        }

        #endregion
        if (enemy.health <= 0)
        {
            iceLeft = 0;
            statusEffect = StatusEffect.None;
            anim.SetTrigger("Death");

            anim.SetLayerWeight(1, 0f);
            anim.SetLayerWeight(2, 0f);

            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShockBall"))
        {
            shockTime = 2;
            statusEffect = StatusEffect.Shocked;
            TakeDamage(20);
        }
    }
    #region Attacking

    private void Attack()
    {
        if (anim.GetLayerWeight(enemy.moveWhileAttacking ? 1 : 2) == 1 || anim.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
            return;

        targetLayerWeight = 1f;
        switch (enemy.enemyAttackType)
        {
            case Enemy.AttackType.Melee:
                MeleeAttack();
                break;
            case Enemy.AttackType.Magic:
                MagicAttack();
                break;
            case Enemy.AttackType.Summoner:
                SummonAttack();
                break;
        }
    }

    private void MeleeAttack()
    {
        anim.SetTrigger("Short Range Attack");
    }

    private void MagicAttack()
    {
        anim.SetBool("Charging", true);
    }

    private void SummonAttack()
    {
        anim.SetBool("Charging Summon", true);
    }

    #endregion

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distFromPlayer + 1f)
        {
            player.GetComponent<Player>().TakeDamage(enemy.damage);
            Instantiate(hitEffect, Vector3.Lerp(player.transform.position, transform.position, 0.3f) + new Vector3(0f, 2.125f, 0f), Quaternion.identity);
        }

        targetLayerWeight = 0f;
    }

    public void LongRangeAttack()
    {
        switch (enemy.enemyAttackType)
        {
            case Enemy.AttackType.Magic:
                ShootSpell();
                break;
            case Enemy.AttackType.Summoner:
                StartCoroutine(SummonEnemy());
                break;
        }
    }

    private void ShootSpell()
    {
        LongRangedAttack currentSpell = Instantiate(spell, enemy.wandTip.transform.position, Quaternion.identity).GetComponent<LongRangedAttack>();

        currentSpell.user = enemy;
        currentSpell.target = player;

        currentSpell.Initialize();
    }

    private IEnumerator SummonEnemy()
    {
        Vector3 spawnPos = GetSummonSpot(transform.position, spawnRange);

        GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);
        currentSpawningParticles = summonParticles;

        yield return new WaitForSeconds(enemy.chargeTime);

        GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true);
        Destroy(summonParticles);

        anim.SetBool("Charging Summon", false);
        targetLayerWeight = 0f;
    }

    private Vector3 GetSummonSpot(Vector3 startPos, float range)
    {
        Vector3 newPos = startPos + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
        
        if (Physics.Linecast(newPos, newPos + Vector3.down * 10f, out RaycastHit hit, ground))
        {
            return hit.point;
        }

        return newPos;
    }

    #region Taking Damage

    public void TakeDamage(float damage)
    {
        enemy.health -= damage;
        StartCoroutine(DamageFlash());

        isTargeting = true;

        if (enemy.health <= 0)
        {
            iceLeft = 0;
            anim.speed = 1; 
            statusEffect = StatusEffect.None;
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

    #endregion

    private void OnDestroy()
    {
        if (player != null)
        {
            if (player.GetComponent<Player>().health / player.GetComponent<Player>().maxHealth < 0.25f)
            {
                if (Random.Range(0, 6) > 3)
                {
                    Instantiate(healthPotion, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                }
            }
        }

        if (currentSpawningParticles != null)
            Destroy(currentSpawningParticles);
    }
}
