using UnityEngine;

public class EnemyHeadIK : StateMachineBehaviour
{
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLookAtPosition(PlayerMovement.playerHeadPos);
        animator.SetLookAtWeight(1f);
    }
}
