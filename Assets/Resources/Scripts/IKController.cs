using UnityEngine;

public class IKController : StateMachineBehaviour
{
    public float distanceToGround;
    public float raycastLength;
    public LayerMask ground;

    private Transform lFootGoal, rFootGoal;

    public float offset;

    private void Awake()
    {
        lFootGoal = new GameObject("Left Foot Goal").transform;
        rFootGoal = new GameObject("Right Foot Goal").transform;
    }

    public override void OnStateIK(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down, out RaycastHit lHit, raycastLength, ground))
        {
            lFootGoal.position = lHit.point + new Vector3(0f, distanceToGround, 0f);
            lFootGoal.transform.up = lHit.normal;

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFootGoal.position);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFootGoal.rotation * anim.transform.rotation);

            Debug.Log(anim.GetFloat("LeftFootWeight"));
        }

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.RightFoot), Vector3.down, out RaycastHit rHit, raycastLength, ground))
        {
            rFootGoal.position = rHit.point + new Vector3(0f, distanceToGround, 0f);
            rFootGoal.transform.up = rHit.normal;

            anim.SetIKPosition(AvatarIKGoal.RightFoot, rFootGoal.position);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rFootGoal.rotation * anim.transform.rotation);

            Debug.Log(anim.GetFloat("RightFootWeight"));
        }



        if (Physics.Raycast(anim.transform.position, Vector3.down, out RaycastHit hit, ground))
        {
            anim.bodyPosition = new Vector3(anim.bodyPosition.x, (lFootGoal.position.y + rFootGoal.position.y) / 2f + offset - hit.distance, anim.bodyPosition.z);
        }
    }
}
