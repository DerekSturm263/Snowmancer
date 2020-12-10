using UnityEngine;
using System.Collections;

public class EnemyMovement : Movement
{
    public GameObject player;
    private Enemy enemy;

    public Mesh debugMesh;
    public Material debugMaterial;

    private void Start()
    {
        enemy = GetComponent<Enemy>();

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

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2f)
            player.GetComponent<Player>().TakeDamage(enemy.damage);
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
}
