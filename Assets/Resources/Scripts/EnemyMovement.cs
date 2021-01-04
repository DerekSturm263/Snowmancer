using UnityEngine;
using System.Collections;

public class EnemyMovement : Movement
{
    private GameObject player;
    private Enemy enemy;

    public GameObject spell;

    public static Vector3 playerHeadPosition;

    private Vector2 moveDir;

    private Vector3 enemySpawnPos;

    private GameObject particles;

    private LongRangedAttack currentSpell;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>().gameObject;

        if (enemy.enemyAttackType == Enemy.AttackType.Magic) StartCoroutine("ChargeAttack");
        else if (enemy.enemyAttackType == Enemy.AttackType.Summoner) StartCoroutine("ChargeSummon");

        anim.SetBool("Move While Charging", enemy.moveWhileAttacking);
    }

    private void Update()
    {
        Vector3 targetVector;
        mouseAim = false;

        if (Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            switch (enemy.enemyAttackType)
            {
                case Enemy.AttackType.Melee:

                    targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;

                    Move(targetVector, false);
                    anim.SetFloat("Speed", 1f);

                    if (Vector3.Distance(transform.position, player.transform.position) < transform.localScale.x * 1.5f)
                        ShortRangeAttack();

                    break;
                case Enemy.AttackType.Magic:

                    targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;

                    transform.forward = new Vector3(targetVector.x, 0f, targetVector.y);
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                    anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), -Mathf.Abs(targetVector.x), Time.deltaTime * 10f));
                    anim.SetFloat("Horizontal", Mathf.Lerp(anim.GetFloat("Horizontal"), -Mathf.Abs(targetVector.y), Time.deltaTime * 10f));

                    break;
                case Enemy.AttackType.Summoner:

                    targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;
                    
                    transform.forward = new Vector3(targetVector.x, 0f, targetVector.y);
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                    anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), -Mathf.Abs(targetVector.x), Time.deltaTime * 10f));
                    anim.SetFloat("Horizontal", Mathf.Lerp(anim.GetFloat("Horizontal"), -Mathf.Abs(targetVector.y), Time.deltaTime * 10f));

                    break;
            }
        }
        else
        {
            moveDir = moveDir.normalized;
            moveDir.x += Time.deltaTime * Random.Range(-0.1f, 0.1f);
            moveDir.y += Time.deltaTime * Random.Range(-0.1f, 0.1f);

            targetVector = moveDir;
            Move(targetVector, false);
        }

        anim.SetBool("Grounded", true);
    }

    // Will be changed to be on the snowball script instead once I can make changes to that file.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball"))
        {
            TakeDamage(5f);
        }
    }

    private void ShortRangeAttack()
    {
        anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, 1f);

        anim.SetTrigger("Short Range Attack");
    }

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x * 1.5f)
            player.GetComponent<Player>().TakeDamage(enemy.damage);
    }

    public void TakeDamage(float damage)
    {
        enemy.health -= damage;
        StartCoroutine(DamageFlash());

        if (enemy.health <= 0)
        {
            anim.SetTrigger("Death");

            anim.SetLayerWeight(1, 0f);
            anim.SetLayerWeight(2, 0f);
        }
    }

    public void ShootSpell()
    {
        if (enemy.enemyAttackType == Enemy.AttackType.Magic)
        {
            currentSpell.SeekTarget();
        }
        else
        {
            FinishSummon(enemy.summonerEnemy, enemySpawnPos);
        }
    }

    private void FinishSummon(GameObject enemy, Vector3 pos)
    {
        Instantiate(enemy, pos, Quaternion.identity);
    }

    private Vector3 GetNearbySpot(Vector3 startPos, float range)
    {
        Vector3 newPos = startPos + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));

        if (Physics.Linecast(newPos, newPos + Vector3.down * 10f, out RaycastHit hit, ground))
        {
            return hit.point;
        }

        return newPos;
    }

    private IEnumerator ChargeAttack()
    {
        yield return new WaitForSeconds(enemy.intervalBetweenLongRangeAttacks);

        if (Vector3.Distance(transform.position, player.transform.position) < 10.5f)
        {
            anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, 1f);

            anim.SetBool("Charging", true);

            currentSpell = Instantiate(spell, enemy.hand.transform.position, Quaternion.identity).GetComponent<LongRangedAttack>();

            currentSpell.target = player;
            currentSpell.attackType = (LongRangedAttack.AttackType)(int)enemy.enemyType;
            currentSpell.damage = enemy.damage;
            currentSpell.speed = enemy.magicAttackSpeed;
            currentSpell.size = enemy.attackSize;
            currentSpell.lifeTime = enemy.magicAttackLifeTime;
            currentSpell.origin = enemy.hand;

            currentSpell.Activate();

            yield return new WaitForSeconds(enemy.chargeTime);
            anim.SetBool("Charging", false);
        }

        StartCoroutine(ChargeAttack());
    }

    private IEnumerator ChargeSummon()
    {
        yield return new WaitForSeconds(enemy.intervalBetweenLongRangeAttacks);

        if (Vector3.Distance(transform.position, player.transform.position) < 10.5f)
        {
            anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, 1f);

            anim.SetBool("Charging Summon", true);

            enemySpawnPos = GetNearbySpot(transform.position, enemy.spawnRange);
            particles = Instantiate(enemy.spawnParticles, enemySpawnPos, Quaternion.identity);
            Invoke("DestroyParticles", enemy.chargeTime);

            yield return new WaitForSeconds(enemy.chargeTime);
            anim.SetBool("Charging Summon", false);
        }

        StartCoroutine(ChargeSummon());
    }

    private void DestroyParticles()
    {
        Destroy(particles);
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
