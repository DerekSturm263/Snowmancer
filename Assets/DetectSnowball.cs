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
        if (other.CompareTag("Player") || other.CompareTag("Snowball"))
        {
            ai.stats.active = true;
            ai.stats.anim.SetBool("Charging", false);
            ai.ChooseSpot();
        }
    }
}
