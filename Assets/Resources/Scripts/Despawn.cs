using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : StateMachineBehaviour
{
    public GameObject deathParticles;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(deathParticles, animator.gameObject.transform.position, Quaternion.identity);
        Destroy(animator.gameObject);
    }
}
