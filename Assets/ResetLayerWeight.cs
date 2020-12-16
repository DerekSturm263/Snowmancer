using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLayerWeight : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 0f);
    }
}
