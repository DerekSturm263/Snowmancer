using UnityEngine;

public class SwitchIdle : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.realtimeSinceStartup % 2 == 0f)
            animator.SetFloat("Idle Animation", Random.Range(-7, 4));
    }
}
