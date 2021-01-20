using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLayerWeight2 : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<EnemyMovement>() != null)
            animator.GetComponent<EnemyMovement>().targetLayerWeight = 0f;
        else
            animator.GetComponent<BossBehavior>().stats.anim.SetLayerWeight(layerIndex, 0f);
    }
}
