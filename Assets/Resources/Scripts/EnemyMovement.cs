using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : Movement
{
    public GameObject player;
    private Enemy enemy;

    public Mesh debugMesh;
    public Material debugMaterial;

    private List<Material> materials = new List<Material>();

    private void Start()
    {
        enemy = GetComponent<Enemy>();

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
            }
        }

        if (enemy.enemyAttackType == Enemy.AttackType.Magic) StartCoroutine("Charge");
    }

    private void Update()
    {
        Vector3 targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;
        Move(targetVector, false);
        mouseAim = false;

        if (Vector3.Distance(transform.position, player.transform.position) < 2f && enemy.enemyAttackType == Enemy.AttackType.Melee)
            anim.SetTrigger("Short Range Attack");

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

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2f)
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

    private void Charge()
    {
        StartCoroutine(ChargeAttack());
    }

    private IEnumerator ChargeAttack()
    {
        yield return new WaitForSeconds(enemy.intervalBetweenLongRangeAttacks);
        anim.SetBool("Charging", true);
        yield return new WaitForSeconds(enemy.chargeTime);
        anim.SetBool("Charging", false);
        StartCoroutine(ChargeAttack());
    }

    private IEnumerator DamageFlash()
    {
        for (float i = 0f; i < 0.25f; i += Time.deltaTime)
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_DamageWeight", i * 4f);
            }

            yield return new WaitForEndOfFrame();
        }

        for (float i = 0.25f; i > 0f; i -= Time.deltaTime)
        {
            foreach (Material m in materials)
            {
                m.SetFloat("_DamageWeight", i * 4f);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
