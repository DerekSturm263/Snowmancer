using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public Boss stats;
    public List<Vector3> spots = new List<Vector3>();
    private Vector3 targetSpot = Vector3.zero;

    private float targetSpeed = 0f;

    private Quaternion targetRotation;

    class Vec3Comparer : Comparer<Vector3>
    {
        Transform transform;

        public Vec3Comparer(Transform transform)
        {
            this.transform = transform;
        }

        public override int Compare(Vector3 x, Vector3 y)
        {
            return (int)Vector3.Distance(x, transform.position) - (int)Vector3.Distance(y, transform.position);
        }
    }

    private void Awake()
    {
        BossAttacks.Initialize(stats);

        stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
        stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
        stats.attacks.Add(() => BossAttacks.fireBossSpellBig1.AttackAction.Invoke());
        stats.attacks.Add(() => ChooseSpot());

        stats.phaseFeatures.Add(75, () =>
        {
            stats.attacks.Clear();

            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellBig1.AttackAction.Invoke());
            stats.attacks.Add(() => ChooseSpot());
        });

        stats.phaseFeatures.Add(50, () =>
        {
            stats.attacks.Clear();

            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSummon2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellBig2.AttackAction.Invoke());
            stats.attacks.Add(() => ChooseSpot());
        });

        stats.phaseFeatures.Add(25, () =>
        {
            stats.attacks.Clear();

            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSummon2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
            stats.attacks.Add(() => BossAttacks.fireBossSpellBig2.AttackAction.Invoke());
            stats.attacks.Add(() => ChooseSpot());
        });
                              
        ChooseSpot();         
    }

    private void Update()
    {
        if (!stats.active && Vector3.Distance(stats.player.transform.position, transform.position) < 30f)
        {
            stats.active = true;
            stats.ShowHealth();
        }

        if (!stats.active)
            return;

        if (Vector3.Distance(stats.player.transform.position, transform.position) < 12.5f && (stats.currentAttack != BossAttacks.fireBossSpellBig1 || stats.currentAttack != BossAttacks.fireBossSpellBig2))
            ChooseSpot();

        if (stats.timeSinceLastAttack > 0f)
            stats.timeSinceLastAttack += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetSpot) > 2.5f)
        {
            targetSpeed = 1f;
        }
        else
        {
            targetRotation = Quaternion.LookRotation(transform.position - stats.player.transform.position, Vector3.up);
            targetSpeed = 0f;

            if (stats.anim.GetFloat("Vertical") < 0.1f && !stats.anim.GetBool("Charging") && stats.timeSinceLastAttack >= stats.timeBetweenAttacks)
                stats.anim.SetBool("Charging", true);
        }

        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        stats.anim.SetFloat("Vertical", targetSpeed);
    }

    public void UseAttack()
    {
        if (stats.attackNum >= stats.attacks.Count)
            stats.attackNum = 0;

        stats.timeSinceLastAttack = 0f;
        stats.attacks[stats.attackNum++].Invoke();
    }

    public void ChooseSpot()
    {
        if (Vector3.Distance(transform.position, targetSpot) > 2.5f && targetSpot != Vector3.zero)
            return;

        stats.anim.SetBool("Charging", false);
        spots.Sort((x, y) =>
        {
            return new Vec3Comparer(transform).Compare(x, y);
        });
        targetSpot = spots[Random.Range(1, 3)];

        stats.timeSinceLastAttack = 0.1f;
        targetRotation = Quaternion.LookRotation(transform.position - targetSpot, Vector3.up);
    }
}
