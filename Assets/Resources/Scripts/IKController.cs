using UnityEngine;

public class IKController : StateMachineBehaviour
{
    public LayerMask ground;

    public float distanceToGround;
    public float raycastLength;
    public float offset;

    public static Transform lFootGoal, rFootGoal, bodyGoal;

    private void Awake()
    {
        if (GameObject.Find("Left Foot Goal") == null)
        {
            lFootGoal = new GameObject("Left Foot Goal").transform;
            rFootGoal = new GameObject("Right Foot Goal").transform;
            bodyGoal = new GameObject("Body Goal").transform;
        }
    }

    public override void OnStateIK(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, anim.GetFloat("LeftHandWeight"));
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, anim.GetFloat("RightHandWeight"));

        // Left Foot.
        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down, out RaycastHit lHit, raycastLength, ground))
        {
            lFootGoal.position = lHit.point + new Vector3(0f, distanceToGround, 0f);
            lFootGoal.transform.up = lHit.normal;

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFootGoal.position);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFootGoal.rotation * anim.transform.rotation);
        }

        // Right Foot.
        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.RightFoot), Vector3.down, out RaycastHit rHit, raycastLength, ground))
        {
            rFootGoal.position = rHit.point + new Vector3(0f, distanceToGround, 0f);
            rFootGoal.transform.up = rHit.normal;

            anim.SetIKPosition(AvatarIKGoal.RightFoot, rFootGoal.position);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rFootGoal.rotation * anim.transform.rotation);
        }

        // Left Hand.

        // Right Hand.

        if (anim.velocity.magnitude < 0.2f)
        {
            Vector3 targetPos = new Vector3(anim.bodyPosition.x, (lFootGoal.position.y + rFootGoal.position.y) / 2f + offset, anim.bodyPosition.z);
            bodyGoal.position = Vector3.Lerp(targetPos, anim.bodyPosition, anim.velocity.magnitude * 5f);
            anim.bodyPosition = bodyGoal.position;
        }
    }
}
