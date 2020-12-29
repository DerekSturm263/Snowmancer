using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticles : StateMachineBehaviour
{
    public enum ParticleType
    {
        Landing, Jumping, Moving
    }
    public ParticleType pt;

    private PlayerMovement player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerMovement>();

        switch (pt)
        {
            case ParticleType.Jumping:
                player.jumpParticles.Play();
                break;

            case ParticleType.Landing:
                player.landParticles.Play();
                break;
        }
    }
}
