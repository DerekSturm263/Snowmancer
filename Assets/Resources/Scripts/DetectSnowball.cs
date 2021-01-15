using UnityEngine;

public class DetectSnowball : MonoBehaviour
{
    private BossBehavior ai;

    private void Awake()
    {
        ai = transform.parent.GetComponent<BossBehavior>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (ai.stats.currentAttack == BossAttacks.fireBossSpellBig1 || ai.stats.currentAttack == BossAttacks.fireBossSpellBig2)
            return;

        if (other.CompareTag("Snowball") && other.attachedRigidbody.velocity.magnitude > 0f)
        {
            ai.stats.attackNum = 0;

            if (ai.stats.currentAttack != null)
                ai.stats.currentAttack.Spell.Despawn();

            if (!ai.stats.active)
            {
                ai.stats.active = true;
                ai.stats.ShowHealth();
            }

            ai.stats.currentAttack = null;
            ai.stats.anim.SetBool("Charging", false);

            ai.ChooseSpot();
        }
    }
}
