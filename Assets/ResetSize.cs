using UnityEngine;

public class ResetSize : StateMachineBehaviour
{
    // Resetting the snowball size using an animator script to ensure it happens as soon as you release the snowball.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Snowball Size", 0f);
    }
}
