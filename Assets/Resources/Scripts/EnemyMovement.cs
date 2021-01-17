using UnityEngine;
using System.Collections;

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

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>().gameObject;

        anim.SetBool("Move While Charging", enemy.moveWhileAttacking);
        spawnPosition = transform.position;
    }

    private void Update()
    {
        Vector3 targetVector;
        mouseAim = false;

        #region Moving

        if (Vector3.Distance(player.transform.position, transform.position) < followDist)
            isTargeting = true;

        if (Vector3.Distance(player.transform.position, transform.position) > followDist * 1.5f)
            isTargeting = false;

        if (isTargeting)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > distFromPlayer || targetLayerWeight == 1f)
                targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;
            else
                targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized * -1f;
        }
        else
        {
            if (Vector3.Distance(transform.position, spawnPosition) < 5f)
            {
                moveDir.x += Time.deltaTime * Random.Range(-5f, 5f);
                moveDir.y += Time.deltaTime * Random.Range(-5f, 5f);

                moveDir = moveDir.normalized;
            }
            else
            {
                moveDir = new Vector2(spawnPosition.x - transform.position.x, spawnPosition.z - transform.position.z).normalized;
            }

            targetVector = moveDir;
        }

        #endregion

        anim.SetFloat("Speed", 1f);
        Move(targetVector, false);

        // Attacking
        if (Vector3.Distance(player.transform.position, transform.position) < distFromPlayer + 0.5f && Vector3.Distance(player.transform.position, transform.position) > distFromPlayer - 0.5f)
            Attack();

        anim.SetBool("Grounded", true);
        anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, Mathf.Lerp(anim.GetLayerWeight(enemy.moveWhileAttacking ? 1 : 2), targetLayerWeight, Time.deltaTime * 10f));
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
            player.GetComponent<Player>().TakeDamage(enemy.damage);

        Instantiate(hitEffect, Vector3.Lerp(player.transform.position, transform.position, 0.3f) + new Vector3(0f, 2.125f, 0f), Quaternion.identity);
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
        if (currentSpawningParticles != null)
            Destroy(currentSpawningParticles);
    }
}
