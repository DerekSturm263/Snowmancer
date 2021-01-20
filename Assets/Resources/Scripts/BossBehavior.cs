using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class BossBehavior : MonoBehaviour
{
    public Boss stats;
    public List<Vector3> spots = new List<Vector3>();
    private Vector3 targetSpot = Vector3.zero;

    private float targetSpeed = 0f;
    public float followDist;

    private Quaternion targetRotation;

    private GameObject shockWave;

    private float chargeTime = 4f;

    public GameObject spell;

    public float distToStart = 30f;

    public int bossElementType;

    private Material tattooMaterial;

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
        tattooMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials[3];
        shockWave = Resources.Load<GameObject>("Prefabs/Shockwave");

        switch (stats.type)
        {
            case Boss.ElementType.Fire:

                BossAttacks.fireBoss = stats;
                BossAttacks.Initialize(stats);

                stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                stats.attacks.Add(() => BossAttacks.fireBossSpellBig1.AttackAction.Invoke());
                stats.attacks.Add(() => ChooseSpot());

                stats.phaseFeatures.Add(1, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellBig1.AttackAction.Invoke());
                    stats.attacks.Add(() => ChooseSpot());
                });

                stats.phaseFeatures.Add(2, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSummon2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellBig1.AttackAction.Invoke());
                    stats.attacks.Add(() => ChooseSpot());
                });

                stats.phaseFeatures.Add(3, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellSmall2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSummon1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSummon2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.fireBossSpellBig2.AttackAction.Invoke());
                    stats.attacks.Add(() => ChooseSpot());
                });

                stats.phaseFeatures.Add(4, () =>
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
                break;

            case Boss.ElementType.Electric:

                BossAttacks.electricBoss = stats;
                BossAttacks.Initialize(stats);

                stats.attacks.Add(() => BossAttacks.electricBossSpell1.AttackAction.Invoke());
                stats.attacks.Add(() => BossAttacks.electricBossSpell1.AttackAction.Invoke());
                stats.attacks.Add(() => BossAttacks.electricBossStomp1.AttackAction.Invoke());

                stats.phaseFeatures.Add(1, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSummon1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp2.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(2, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp2.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(3, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSummon1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp3.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(4, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell3.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell3.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp3.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(5, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell3.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell3.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell3.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSummon2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp4.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(6, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.electricBossSpell4.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell4.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSpell4.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossSummon2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.electricBossStomp4.AttackAction.Invoke());
                });

                break;

            case Boss.ElementType.Wind:

                BossAttacks.windBoss = stats;
                BossAttacks.Initialize(stats);

                stats.attacks.Add(() => BossAttacks.windBossSpell1.AttackAction.Invoke());

                stats.phaseFeatures.Add(2, () =>
                {
                    stats.attacks.Clear();
                    
                    stats.attacks.Add(() => BossAttacks.windSummon1_1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon1_1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell1.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(3, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.windSummon2_1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon2_1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell1.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell1.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(4, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.windSummon1_2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon2_2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell2.AttackAction.Invoke());
                });

                stats.phaseFeatures.Add(6, () =>
                {
                    stats.attacks.Clear();

                    stats.attacks.Add(() => BossAttacks.windBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windBossSpell2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon1_2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon2_2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon1_2.AttackAction.Invoke());
                    stats.attacks.Add(() => BossAttacks.windSummon2_2.AttackAction.Invoke());
                });

                AerialWander();
                break;

            case Boss.ElementType.All:

                BossAttacks.finalBoss = stats;
                BossAttacks.Initialize(stats);

                break;
        }
    }

    private void Update()
    {
        float distFromTarget = Vector3.Distance(transform.position, targetSpot);
        float distFromPlayer = Vector3.Distance(stats.player.transform.position, transform.position);

        AnimatorStateInfo anim = stats.anim.GetCurrentAnimatorStateInfo(0);
        float vertical = stats.anim.GetFloat("Vertical");

        if (!stats.active && distFromPlayer < distToStart) // 30f for Fire, Wind, Final. 40f for Electric
        {
            stats.active = true;
            stats.ShowHealth();

            if (stats.type == Boss.ElementType.Wind)
                stats.anim.SetTrigger("Start");
        }

        if (!stats.active)
            return;

        if (stats.type == Boss.ElementType.Fire)
        {
            if (stats.newSpot)
            {
                stats.newSpot = false;
                ChooseSpot();
                stats.attackNum = 0;
            }

            if (distFromPlayer < 7.5f && !(stats.currentAttack == BossAttacks.fireBossSpellBig1 || stats.currentAttack == BossAttacks.fireBossSpellBig2))
            {
                ChooseSpot();
                stats.ResetAttack();
            }

            if (!anim.IsName("Dazed"))
            {
                if (distFromTarget > 2.5f)
                {
                    targetRotation = Quaternion.LookRotation(transform.position - targetSpot, Vector3.up);
                    targetSpeed = 1f;
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(transform.position - stats.player.transform.position, Vector3.up);
                    targetSpeed = 0f;

                    if (vertical < 0.1f && !stats.anim.GetBool("Charging") && stats.timeSinceLastAttack >= stats.timeBetweenAttacks)
                        stats.anim.SetBool("Charging", true);
                }
            }

            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        }
        else if (stats.type == Boss.ElementType.Electric)
        {
            targetRotation = Quaternion.LookRotation(transform.position - stats.player.transform.position, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            if (distFromPlayer > followDist + 5.5f)
            {
                stats.anim.SetFloat("Vertical", 1f);
            }
            else if (distFromPlayer < followDist + 4.5f)
            {
                stats.anim.SetFloat("Vertical", -2.5f + distFromPlayer * 0.2f);
            }
            else
            {
                stats.anim.SetFloat("Vertical", 0f);
            }

            if (!stats.anim.GetBool("Charging Spell") && !stats.anim.GetBool("Charging Stomp") && stats.timeSinceLastAttack >= stats.timeBetweenAttacks && !stats.anim.GetBool("Dazed"))
            {
                string chargeType = "Charging Spell";

                if (stats.attackNum == stats.attacks.Count - 1)
                {
                    chargeType = "Charging Stomp";
                    stats.anim.SetLayerWeight(1, 0f);
                }
                else
                {
                    stats.anim.SetLayerWeight(1, 1f);
                }

                stats.anim.SetBool(chargeType, true);
            }

            if (CheckHit.deadIcicles.Count == 0)
            {
                CheckHit.icicles.ForEach(x => x.Regrow());
            }
        }
        else if (stats.type == Boss.ElementType.Wind)
        {
            if (stats.newSpot)
            {
                stats.newSpot = false;
                AerialWander();
                stats.attackNum = 0;
            }

            if (!anim.IsName("Dazed"))
            {
                if (distFromTarget > 5f)
                {
                    targetRotation = Quaternion.LookRotation(transform.position - targetSpot, Vector3.up);
                    stats.anim.SetFloat("Vertical", 1f);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, targetSpot, Time.deltaTime);

                    targetRotation = Quaternion.LookRotation(transform.position - stats.player.transform.position, Vector3.up);
                    stats.anim.SetFloat("Vertical", 0f);

                    if (vertical < 0.1f && !stats.anim.GetBool("Charging") && stats.timeSinceLastAttack >= stats.timeBetweenAttacks)
                        stats.anim.SetBool("Charging", true);
                }

                transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y + 180f, targetRotation.eulerAngles.z);
            }
        }
        else
        {
            targetRotation = Quaternion.LookRotation(transform.position - stats.player.transform.position, Vector3.up);
            
            if (anim.IsName("Strafing") || anim.IsName("Charging Smash Transition") || anim.IsName("Charging Smash") || anim.IsName("Running") || anim.IsName("Charging Spell Transition") || anim.IsName("Charging Spell"))
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y + 180f, targetRotation.eulerAngles.z);

            if (distFromPlayer > 25f)
            {
                if (!stats.anim.GetBool("Charging Smash") || !stats.anim.GetBool("Charging Spell"))
                    stats.anim.SetBool("Charging Spell", true);
            }
            else if (distFromPlayer > 5f)
            {
                stats.anim.SetFloat("Vertical", 1f);
            }
            else
            {
                stats.anim.SetFloat("Vertical", 0f);

                if (!stats.anim.GetBool("Charging Smash") || !stats.anim.GetBool("Charging Spell"))
                {
                    bossElementType = Random.Range(0, 4);

                    switch (bossElementType)
                    {
                        case 0:
                            tattooMaterial.SetColor("_Tint", Color.red);
                            break;
                        case 1:
                            tattooMaterial.SetColor("_Tint", Color.cyan);
                            break;
                        case 2:
                            tattooMaterial.SetColor("_Tint", Color.yellow);
                            break;
                        case 3:
                            tattooMaterial.SetColor("_Tint", Color.white);
                            break;
                    }

                    stats.anim.SetBool("Charging Smash", true);
                }
            }
        }
        
        if (stats.timeSinceLastAttack > 0f)
            stats.timeSinceLastAttack += Time.deltaTime;
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
    }

    public void AerialWander()
    {
        stats.anim.SetBool("Charging", false);
        targetSpot = spots[spots.IndexOf(targetSpot) + 1];
        stats.timeSinceLastAttack = 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            stats.anim.SetBool("Grounded", true);
    }

    public void Stomp()
    {
        Instantiate(shockWave, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        List<CheckHit> shuffledIcicles = CheckHit.icicles;

        foreach (CheckHit icicle in shuffledIcicles)
        {
            int random = Random.Range(-1, 1);

            if (icicle.isActive == true && random >= 0)
            {
                icicle.anim.SetTrigger("Hit");
            }
        }
    }

    public void Smash()
    {
        Invoke("ReleaseSmash", chargeTime);
    }

    public void ReleaseSmash()
    {
        stats.anim.SetBool("Charging Smash", false);
    }
    
    public void CheckForHit()
    {
        if (Vector3.Distance(stats.player.transform.position, transform.position) < 5f && Vector3.Dot(transform.position, stats.player.transform.position) > 0.5f)
        {
            stats.player.GetComponent<Player>().TakeDamage(75f);
        }
        else
        {
            stats.anim.SetBool("Dazed", true);
            Invoke("UnDaze", 5f);
        }
    }

    public void UnDaze()
    {
        stats.anim.SetBool("Dazed", false);
    }

    public void UseSpellAttack()
    {
        LongRangedAttack currentSpell = Instantiate(spell, transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

        if (stats.health / stats.maxHealth > 0.5f)
            stats.currentAttack = BossAttacks.finalBossSpell1_1;
        else
            stats.currentAttack = BossAttacks.finalBossSpell1_2;

        currentSpell.userBoss = stats;
        currentSpell.target = currentSpell.userBoss.player;
        currentSpell.attackType = (Enemy.ElementType) Random.Range(0, 4);

        switch ((int) currentSpell.attackType)
        {
            case 0:
                tattooMaterial.SetColor("_Tint", Color.red);
                break;
            case 1:
                tattooMaterial.SetColor("_Tint", Color.cyan);
                break;
            case 2:
                tattooMaterial.SetColor("_Tint", Color.yellow);
                break;
            case 3:
                tattooMaterial.SetColor("_Tint", Color.white);
                break;
        }

        currentSpell.Initialize(true);
        Invoke("ReleaseSpellAttack", stats.currentAttack.ChargeTime - 1f);
    }

    public void ReleaseSpellAttack()
    {
        stats.anim.SetBool("Charging Spell", false);
    }

    public void ReleaseStomp()
    {
        stats.anim.SetBool("Charging Stomp", false);
    }

    public void SwatSnowball()
    {
        if (!stats.anim.GetCurrentAnimatorStateInfo(0).IsName("Stuck"))
        {
            stats.anim.SetLayerWeight(1, 1f);
            stats.anim.SetTrigger("Swat Snowball");
        }
    }
}
