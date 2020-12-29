using UnityEngine;

public class SetLandSpeed : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = CalculateSpeed(animator.GetComponent<Rigidbody>());
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }

    private float CalculateSpeed(Rigidbody rb)
    {
        float x = rb.velocity.y;

        return Mathf.Clamp(1.5f + 0.1f * x, 0.5f, 1f);
    }
}
