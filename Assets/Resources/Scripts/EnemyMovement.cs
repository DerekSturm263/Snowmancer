using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : Movement
{
    private GameObject player;
    private Enemy enemy;

    public GameObject spell;

    public static Vector3 playerHeadPosition;

    private Vector2 moveDir;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>().gameObject;

        if (enemy.enemyAttackType == Enemy.AttackType.Magic) StartCoroutine("ChargeAttack");
        anim.SetBool("Move While Charging", enemy.moveWhileAttacking);
    }

    private void Update()
    {
        Vector3 targetVector;
        mouseAim = false;

        if (Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;

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
        LongRangedAttack newSpell = Instantiate(spell, enemy.hand.transform.position, Quaternion.identity).GetComponent<LongRangedAttack>();

        newSpell.target = player;
        newSpell.attackType = (LongRangedAttack.AttackType) (int) enemy.enemyType;
        newSpell.damage = enemy.damage;
        newSpell.speed = enemy.magicAttackSpeed;
        newSpell.size = enemy.attackSize;
        newSpell.lifeTime = enemy.magicAttackLifeTime;

        newSpell.SeekTarget();
    }

    private IEnumerator ChargeAttack()
    {
        yield return new WaitForSeconds(enemy.intervalBetweenLongRangeAttacks);

        if (Vector3.Distance(transform.position, player.transform.position) < 10.5f)
        {
            anim.SetLayerWeight(enemy.moveWhileAttacking ? 1 : 2, 1f);

            anim.SetBool("Charging", true);
            yield return new WaitForSeconds(enemy.chargeTime);
            anim.SetBool("Charging", false);
        }

        StartCoroutine(ChargeAttack());
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
