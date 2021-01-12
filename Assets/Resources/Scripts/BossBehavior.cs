using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public LayerMask ground;

    public Boss stats;
    public List<Vector3> spots = new List<Vector3>();
    private Vector3 targetSpot = Vector3.zero;

    private Transform player;

    private float targetSpeed = 0f;

    public GameObject spawnParticles;
    public GameObject enemyToSummon;

    private Vector3 spawnPosition;

    public GameObject spell;

    public enum Attack
    {
        Small_Attack, Long_Attack, Summon
    }
    public Attack currentAttack;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        stats.phaseFeatures.Add(10f, () =>
        {

        });

        ChooseSpot();
    }

    private void Update()
    {
        if (!stats.active)
            return;

        if (Vector3.Distance(transform.position, targetSpot) > 2.5f)
        {
            transform.forward = (transform.position - targetSpot).normalized * -1f;
            targetSpeed = 1f;
        }
        else
        {
            transform.LookAt(player);
            targetSpeed = 0f;

            if (stats.anim.GetCurrentAnimatorStateInfo(0).IsName("Default"))
                stats.anim.SetBool("Charging", true);
        }
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, transform.rotation.eulerAngles.z);

        stats.anim.SetFloat("Vertical", Mathf.Lerp(stats.anim.GetFloat("Vertical"), targetSpeed, Time.deltaTime * 20f));
    }

    class Vec3Comparer : Comparer<Vector3>
    {
        Transform transform;

        public Vec3Comparer(Transform transform)
        {
            this.transform = transform;
        }

        public override int Compare(Vector3 x, Vector3 y)
        {
            return (int) Vector3.Distance(x, transform.position) - (int) Vector3.Distance(y, transform.position);
        }
    }

    public void ChooseSpot()
    {
        if (Vector3.Distance(transform.position, targetSpot) > 2.5f && targetSpot != Vector3.zero)
            return;

        spots.Sort((x, y) =>
        {
            return new Vec3Comparer(transform).Compare(x, y);
        });
        targetSpot = spots[Random.Range(1, 3)];
    }

    public void UseAttack()
    {
        if (currentAttack == Attack.Small_Attack)
        {
            StartCoroutine(SmallAttack());
        }
        else if (currentAttack == Attack.Long_Attack)
        {
            StartCoroutine(BigAttack());
        }
        else
        {
            StartCoroutine(Summon());
        }
    }

    private IEnumerator SmallAttack()
    {
        LongRangedAttack currentSpell = Instantiate(spell, transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

        currentSpell.userBoss = stats;
        currentSpell.target = player.gameObject;

        currentSpell.Initialize();

        yield return new WaitForSeconds(stats.attackChargeTime);

        currentSpell.SeekTarget();
        stats.anim.SetBool("Charging", false);
    }
    
    private IEnumerator BigAttack()
    {
        LongRangedAttack currentSpell = Instantiate(spell, transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

        currentSpell.userBoss = stats;
        currentSpell.target = player.gameObject;

        currentSpell.Initialize();
        currentSpell.transform.localScale *= 2f;
        currentSpell.speed /= 2f;
        currentSpell.damage *= 2f;

        yield return new WaitForSeconds(stats.attackChargeTime * 3f);

        currentSpell.SeekTarget();
        stats.anim.SetBool("Charging", false);
    }

    private IEnumerator Summon()
    {
        Vector3 spawnPos = GetSummonSpot(transform.position, stats.spawnRange);

        GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(stats.attackChargeTime);

        GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true);
        Destroy(summonParticles);

        stats.anim.SetBool("Charging", false);
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
}
