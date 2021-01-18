using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall2 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody>().useGravity = true;
    }
}
