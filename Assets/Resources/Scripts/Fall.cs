using UnityEngine;

public class Fall : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckHit thisHit = animator.GetComponent<CheckHit>();

        animator.GetComponent<Rigidbody>().useGravity = true;

        animator.enabled = false;
        thisHit.isActive = false;
    }
}
