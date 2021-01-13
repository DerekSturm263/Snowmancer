using UnityEngine;

public class DetectSnowball : MonoBehaviour
{
    private BossBehavior ai;

    private void Awake()
    {
        ai = transform.parent.GetComponent<BossBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ai.stats.active)
        {
            ai.ChooseSpot();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (ai.stats.currentAttack == BossAttacks.fireBossSpellBig1 || ai.stats.currentAttack == BossAttacks.fireBossSpellBig1)
            return;

        if (other.CompareTag("Snowball"))
        {
            ai.stats.attackNum = 0;

            if (ai.stats.currentAttack != null)
                ai.stats.currentAttack.Spell.Despawn();

            ai.stats.currentAttack = null;
            ai.stats.active = true;
            ai.stats.anim.SetBool("Charging", false);

            ai.ChooseSpot();
        }
    }
}
