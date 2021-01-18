using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetVelocity : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
    }
}
