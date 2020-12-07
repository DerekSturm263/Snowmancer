using UnityEngine;

public class FootSetting : StateMachineBehaviour
{
    private int footNum;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float lFoot = animator.GetFloat("LeftFootWeight");
        float rFoot = animator.GetFloat("RightFootWeight");
        float runMultiplier = animator.GetFloat("Speed");

        Debug.Log(lFoot + ", " + rFoot);

        if (lFoot > rFoot)
            footNum = -1;
        else if (rFoot > lFoot)
            footNum = 1;
        else
            footNum = 0;

        animator.SetFloat("Last Foot", footNum * runMultiplier);
    }
}
