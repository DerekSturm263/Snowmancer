using UnityEngine;
using System.Collections.Generic;

public class BossAttacks : MonoBehaviour
{
    public static Boss fireBoss;
    public static Boss electricBoss;
    public static Boss windBoss;
    public static Boss finalBoss;

    public static List<BossAttack> fireBossAttacks = new List<BossAttack>();
    public static List<BossAttack> electricBossAttacks = new List<BossAttack>();
    public static List<BossAttack> windBossAttacks = new List<BossAttack>();
    public static List<BossAttack> finalBossAttacks = new List<BossAttack>();

    public static GameObject spell = Resources.Load<GameObject>("Prefabs/Long Ranged Attack");
    public static GameObject spawnParticles = Resources.Load<GameObject>("Prefabs/Spawning Particles");

    #region Fire Boss Attacks

    // Element Type, Charge Time, Damage, Size, Speed, Lifetime
    public static BossAttack fireBossSpellSmall1 = new BossAttack(Enemy.ElementType.Fire, 2.5f, 10f, 0.5f, 2f, 30f, BossAttack.BossUser.Fire_Boss);
    public static BossAttack fireBossSpellSmall2 = new BossAttack(Enemy.ElementType.Fire, 1.5f, 15f, 0.75f, 3f, 25f, BossAttack.BossUser.Fire_Boss);

    public static BossAttack fireBossSpellBig1 = new BossAttack(Enemy.ElementType.Fire, 5f, 25f, 3f, 1f, 50f, BossAttack.BossUser.Fire_Boss);
    public static BossAttack fireBossSpellBig2 = new BossAttack(Enemy.ElementType.Fire, 3.5f, 35f, 4f, 2f, 40f, BossAttack.BossUser.Fire_Boss);

    public static BossAttack fireBossSummon1 = new BossAttack(Enemy.ElementType.Fire, 2f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Fire_Boss);
    public static BossAttack fireBossSummon2 = new BossAttack(Enemy.ElementType.Fire, 1.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Fire_Boss);

    #endregion

    #region Electric Boss Attacks

    // Element Type, Charge Time, Damage, Size, Speed, Lifetime
    public static BossAttack electricBossSpell1 = new BossAttack(Enemy.ElementType.Electric, 2f, 10f, 1f, 0f, 10f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossSpell2 = new BossAttack(Enemy.ElementType.Electric, 1.5f, 20f, 1f, 0f, 10f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossSpell3 = new BossAttack(Enemy.ElementType.Electric, 0.75f, 20f, 1f, 0f, 10f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossSpell4 = new BossAttack(Enemy.ElementType.Electric, 0.25f, 20f, 1f, 0f, 10f, BossAttack.BossUser.Electric_Boss);

    public static BossAttack electricBossStomp1 = new BossAttack(Enemy.ElementType.Electric, 2f, 20f, 1f, 5f, 1f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossStomp2 = new BossAttack(Enemy.ElementType.Electric, 1f, 20f, 1f, 5f, 1f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossStomp3 = new BossAttack(Enemy.ElementType.Electric, 0.5f, 25f, 1f, 7.5f, 1f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossStomp4 = new BossAttack(Enemy.ElementType.Electric, 0.25f, 25f, 1f, 7.5f, 1f, BossAttack.BossUser.Electric_Boss);

    public static BossAttack electricBossSummon1 = new BossAttack(Enemy.ElementType.Electric, 2.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Electric_Boss);
    public static BossAttack electricBossSummon2 = new BossAttack(Enemy.ElementType.Electric, 1.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Electric_Boss);

    #endregion

    #region Wind Boss Attacks

    // Element Type, Charge Time, Damage, Size, Speed, Lifetime
    public static BossAttack windBossSpell1 = new BossAttack(Enemy.ElementType.Wind, 3f, 15f, 0.5f, 1.5f, 5f, BossAttack.BossUser.Wind_Boss);
    public static BossAttack windBossSpell2 = new BossAttack(Enemy.ElementType.Wind, 2f, 20f, 0.75f, 2f, 7.5f, BossAttack.BossUser.Wind_Boss);

    public static BossAttack windSummon1_1 = new BossAttack(Enemy.ElementType.Wind, 2.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Wind_Boss);
    public static BossAttack windSummon1_2 = new BossAttack(Enemy.ElementType.Wind, 1.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Wind_Boss);
    public static BossAttack windSummon2_1 = new BossAttack(Enemy.ElementType.Wind, 2.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Wind_Boss);
    public static BossAttack windSummon2_2 = new BossAttack(Enemy.ElementType.Wind, 1.5f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Wind_Boss);

    #endregion

    #region Final Boss Attacks

    // Element Type, Charge Time, Damage, Size, Speed, Lifetime
    public static BossAttack finalBossSpell1_1 = new BossAttack(Enemy.ElementType.Ice, 2.5f, 20f, 0.75f, 2f, 10f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell1_2 = new BossAttack(Enemy.ElementType.Ice, 1.5f, 25f, 1.5f, 2.5f, 7.5f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell2_1 = new BossAttack(Enemy.ElementType.Fire, 2.5f, 20f, 0.75f, 2f, 10f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell2_2 = new BossAttack(Enemy.ElementType.Fire, 1.5f, 25f, 1.5f, 2.5f, 7.5f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell3_1 = new BossAttack(Enemy.ElementType.Electric, 2.5f, 20f, 0.75f, 2f, 10f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell3_2 = new BossAttack(Enemy.ElementType.Electric, 1.5f, 25f, 1.5f, 2.5f, 7.5f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell4_1 = new BossAttack(Enemy.ElementType.Wind, 2.5f, 20f, 0.75f, 2f, 10f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSpell4_2 = new BossAttack(Enemy.ElementType.Wind, 1.5f, 25f, 1.5f, 2.5f, 7.5f, BossAttack.BossUser.Final_Boss);

    public static BossAttack finalBossSummon1 = new BossAttack(Enemy.ElementType.Ice, 2f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSummon2 = new BossAttack(Enemy.ElementType.Fire, 2f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSummon3 = new BossAttack(Enemy.ElementType.Electric, 2f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSummon4 = new BossAttack(Enemy.ElementType.Wind, 2f, 0f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);

    public static BossAttack finalBossSmash1_1 = new BossAttack(Enemy.ElementType.Ice, 3f, 40f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash1_2 = new BossAttack(Enemy.ElementType.Ice, 2f, 60f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash2_1 = new BossAttack(Enemy.ElementType.Fire, 3f, 40f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash2_2 = new BossAttack(Enemy.ElementType.Fire, 2f, 60f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash3_1 = new BossAttack(Enemy.ElementType.Electric, 3f, 40f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash3_2 = new BossAttack(Enemy.ElementType.Electric, 2f, 60f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash4_1 = new BossAttack(Enemy.ElementType.Wind, 3f, 40f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);
    public static BossAttack finalBossSmash4_2 = new BossAttack(Enemy.ElementType.Wind, 2f, 60f, 0f, 0f, 0f, BossAttack.BossUser.Final_Boss);

    #endregion

    public static void Initialize(Boss boss)
    {
        if (boss == fireBoss)
        {
            fireBossAttacks.ForEach(x =>
            {
                x.SetUser(boss);
            });

            #region Attacks

            fireBossSpellSmall1.SetAction(() =>
            {
                boss.currentAttack = fireBossSpellSmall1;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                fireBossSpellSmall1.Spell = currentSpell;

                currentSpell.Initialize();
            });
            fireBossSpellSmall2.SetAction(() =>
            {
                boss.currentAttack = fireBossSpellSmall2;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                fireBossSpellSmall2.Spell = currentSpell;

                currentSpell.Initialize();
            });

            fireBossSpellBig1.SetAction(() =>
            {
                boss.currentAttack = fireBossSpellBig1;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                fireBossSpellBig1.Spell = currentSpell;

                currentSpell.Initialize();
            });
            fireBossSpellBig2.SetAction(() =>
            {
                boss.currentAttack = fireBossSpellBig2;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                fireBossSpellBig2.Spell = currentSpell;

                currentSpell.Initialize();
            });

            fireBossSummon1.SetAction(() =>
            {
                boss.currentAttack = fireBossSummon1;
                Vector3 spawnPos = fireBossSummon1.UserBoss.GetSummonSpot(boss.gameObject.transform.position, fireBossSpellSmall1.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Fire Melee");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                fireBossSummon1.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });
            fireBossSummon2.SetAction(() =>
            {
                boss.currentAttack = fireBossSummon2;
                Vector3 spawnPos = fireBossSummon2.UserBoss.GetSummonSpot(boss.gameObject.transform.position, fireBossSpellSmall2.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Fire Magic");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                fireBossSummon1.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });

            #endregion
        }
        else if (boss == electricBoss)
        {
            electricBossAttacks.ForEach(x =>
            {
                x.SetUser(boss);
            });

            #region Attacks

            electricBossSpell1.SetAction(() =>
            {
                boss.currentAttack = electricBossSpell1;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = electricBossSpell1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                electricBossSpell1.Spell = currentSpell;

                currentSpell.Initialize();
            });
            electricBossSpell2.SetAction(() =>
            {
                boss.currentAttack = electricBossSpell2;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = electricBossSpell2.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                electricBossSpell2.Spell = currentSpell;

                currentSpell.Initialize();
            });
            electricBossSpell3.SetAction(() =>
            {
                boss.currentAttack = electricBossSpell3;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = electricBossSpell3.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                electricBossSpell3.Spell = currentSpell;

                currentSpell.Initialize();
            });
            electricBossSpell4.SetAction(() =>
            {
                boss.currentAttack = electricBossSpell4;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = electricBossSpell4.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                electricBossSpell4.Spell = currentSpell;

                currentSpell.Initialize();
            });

            electricBossStomp1.SetAction(() =>
            {
                boss.currentAttack = electricBossStomp1;
                boss.GetComponent<BossBehavior>().Invoke("ReleaseStomp", electricBossStomp1.ChargeTime);
            });
            electricBossStomp2.SetAction(() =>
            {
                boss.currentAttack = electricBossStomp2;
                boss.GetComponent<BossBehavior>().Invoke("ReleaseStomp", electricBossStomp2.ChargeTime);
            });
            electricBossStomp3.SetAction(() =>
            {
                boss.currentAttack = electricBossStomp3;
                boss.GetComponent<BossBehavior>().Invoke("ReleaseStomp", electricBossStomp3.ChargeTime);
            });
            electricBossStomp4.SetAction(() =>
            {
                boss.currentAttack = electricBossStomp4;
                boss.GetComponent<BossBehavior>().Invoke("ReleaseStomp", electricBossStomp4.ChargeTime);
            });

            electricBossSummon1.SetAction(() =>
            {
                boss.currentAttack = electricBossSummon1;
                Vector3 spawnPos = electricBossSummon1.UserBoss.GetSummonSpot(boss.gameObject.transform.position, electricBossSummon1.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Electric Magic");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                electricBossSummon1.UserBoss.anim.SetBool("Charging Spell", false);
                boss.timeSinceLastAttack = 0.1f;
            });
            electricBossSummon2.SetAction(() =>
            {
                boss.currentAttack = electricBossSummon2;
                Vector3 spawnPos = electricBossSummon2.UserBoss.GetSummonSpot(boss.gameObject.transform.position, electricBossSummon2.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Electric Melee");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                electricBossSummon2.UserBoss.anim.SetBool("Charging Spell", false);
                boss.timeSinceLastAttack = 0.1f;
            });

            #endregion
        }
        else if (boss == windBoss)
        {
            windBossAttacks.ForEach(x =>
            {
                x.SetUser(boss);
            });

            #region Attacks

            windBossSpell1.SetAction(() =>
            {
                boss.currentAttack = windBossSpell1;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = windBossSpell1.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                windBossSpell1.Spell = currentSpell;

                currentSpell.Initialize();
            });
            windBossSpell2.SetAction(() =>
            {
                boss.currentAttack = windBossSpell2;
                LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

                currentSpell.userBoss = windBossSpell2.UserBoss;
                currentSpell.target = currentSpell.userBoss.player;
                windBossSpell2.Spell = currentSpell;

                currentSpell.Initialize();
            });
            windSummon1_1.SetAction(() =>
            {
                boss.currentAttack = windSummon1_1;
                Vector3 spawnPos = windSummon1_1.UserBoss.GetSummonSpot(boss.gameObject.transform.position, windSummon1_1.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Wind Melee");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                windSummon1_1.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });
            windSummon1_2.SetAction(() =>
            {
                boss.currentAttack = windSummon1_2;
                Vector3 spawnPos = windSummon1_2.UserBoss.GetSummonSpot(boss.gameObject.transform.position, windSummon1_2.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Wind Melee");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                windSummon1_2.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });
            windSummon2_1.SetAction(() =>
            {
                boss.currentAttack = windSummon2_1;
                Vector3 spawnPos = windSummon2_1.UserBoss.GetSummonSpot(boss.gameObject.transform.position, windSummon2_1.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Wind Magic");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                windSummon2_1.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });
            windSummon2_2.SetAction(() =>
            {
                boss.currentAttack = windSummon2_2;
                Vector3 spawnPos = windSummon2_2.UserBoss.GetSummonSpot(boss.gameObject.transform.position, windSummon2_2.UserBoss.spawnRange);

                GameObject summonParticles = Instantiate(spawnParticles, spawnPos, Quaternion.identity);

                GameObject enemyToSummon = Resources.Load<GameObject>("Prefabs/Enemy/Wind Magic");

                GameObject newEnemy = Instantiate(enemyToSummon, spawnPos, Quaternion.identity);
                newEnemy.SetActive(true);
                Destroy(summonParticles);

                windSummon2_2.UserBoss.anim.SetBool("Charging", false);
                boss.timeSinceLastAttack = 0.1f;
            });


            #endregion
        }
        else
        {
            finalBossAttacks.ForEach(x =>
            {
                x.SetUser(boss);
            });
        }
    }
}
