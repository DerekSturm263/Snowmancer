﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : StateMachineBehaviour
{

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }

}
