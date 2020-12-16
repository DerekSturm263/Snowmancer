using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : Movement
{
    private GameObject player;
    private Enemy enemy;

    public Mesh debugMesh;
    public Material debugMaterial;

    private List<Material> materials = new List<Material>();

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>().gameObject;

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }

        if (enemy.enemyAttackType == Enemy.AttackType.Magic) StartCoroutine("ChargeAttack");
        anim.SetBool("Move While Charging", enemy.moveWhileLongRangeAttacking);
    }

    private void Update()
    {
        Vector3 targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            Move(targetVector, false);
        else
            Move(-targetVector, false);

        mouseAim = false;

        if (Vector3.Distance(transform.position, player.transform.position) < transform.localScale.x * 1.5f && enemy.enemyAttackType == Enemy.AttackType.Melee)
            ShortRangeAttack();

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
        anim.SetLayerWeight(0, 1f);
        anim.SetTrigger("Short Range Attack");
    }

    private void LongRangeAttack()
    {
        anim.SetLayerWeight(1, 1f);
        ChargeAttack();
    }

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x)
            player.GetComponent<Player>().TakeDamage(enemy.damage);
    }

    public void TakeDamage(float damage)
    {
        enemy.health -= damage;
        StartCoroutine(DamageFlash());

        if (enemy.health <= 0)
        {
            anim.SetTrigger("Death");
        }
    }

    public void  ShootSpell()
    {
        GameObject spell = new GameObject("Spell");
        spell.transform.position = transform.position;
        Rigidbody spellRB = spell.AddComponent<Rigidbody>();
        spell.AddComponent<MeshFilter>().mesh = debugMesh;
        spell.AddComponent<MeshRenderer>().material = debugMaterial;
        spellRB.useGravity = false;
        spellRB.velocity = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, player.transform.position.z - transform.position.z).normalized * enemy.magicAttackSpeed;
    }

    private IEnumerator ChargeAttack()
    {
        yield return new WaitForSeconds(enemy.intervalBetweenLongRangeAttacks);
        anim.SetLayerWeight(1, 1f);
        anim.SetBool("Charging", true);
        yield return new WaitForSeconds(enemy.chargeTime);
        anim.SetBool("Charging", false);
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
