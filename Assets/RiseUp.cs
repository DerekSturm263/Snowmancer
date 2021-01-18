using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseUp : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody>().useGravity = false;
        animator.SetBool("Grounded", false);
    }
}
