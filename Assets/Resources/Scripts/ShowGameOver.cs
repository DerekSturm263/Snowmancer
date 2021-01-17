using UnityEngine;

public class ShowGameOver : StateMachineBehaviour
{
    private int layerMask = 1 << 9;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //layerMask = ~layerMask;
        Player p = animator.GetComponent<Player>();

        p.uiCont.GameOver();
        p.cam.cullingMask = layerMask;
    }
}
