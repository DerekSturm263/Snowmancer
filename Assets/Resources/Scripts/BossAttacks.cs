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

    public static void Initialize(Boss boss)
    {
        fireBossAttacks.ForEach(x =>
        {
            x.SetUser(boss);
        });



        fireBossSpellSmall1.SetAction(() =>
        {
            boss.currentAttack = fireBossSpellSmall1;
            LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

            currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
            currentSpell.target = FindObjectOfType<Player>().gameObject;
            fireBossSpellSmall1.Spell = currentSpell;

            currentSpell.Initialize();
        });
        fireBossSpellSmall2.SetAction(() =>
        {
            boss.currentAttack = fireBossSpellSmall2;
            LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

            currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
            currentSpell.target = FindObjectOfType<Player>().gameObject;
            fireBossSpellSmall2.Spell = currentSpell;

            currentSpell.Initialize();
        });


        fireBossSpellBig1.SetAction(() =>
        {
            boss.currentAttack = fireBossSpellBig1;
            LongRangedAttack currentSpell = Instantiate(spell, boss.gameObject.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity).GetComponent<LongRangedAttack>();

            currentSpell.userBoss = fireBossSpellSmall1.UserBoss;
            currentSpell.target = FindObjectOfType<Player>().gameObject;
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
    }
}
