using UnityEngine;

public class FootSetting : StateMachineBehaviour
{
    private float footNum;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float lFoot = animator.GetFloat("LeftFootWeight");
        float rFoot = animator.GetFloat("RightFootWeight");
        float runMultiplier = animator.GetFloat("Speed");

        if (lFoot > rFoot)
            footNum = -1f;
        else if (rFoot > lFoot)
            footNum = 1f;
        else
            footNum = 0f;

        animator.SetFloat("Last Foot", footNum * runMultiplier);
    }
}
