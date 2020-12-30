using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetShocked : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.None;
    }
}
