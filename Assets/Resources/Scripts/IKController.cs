using UnityEngine;

public class IKController : MonoBehaviour
{
    private Animator anim;

    [Header("Foot IK Settings")]

    public GameObject lFoot;
    public GameObject rFoot;
    public GameObject lKnee;
    public GameObject rKnee;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private float slopeRaycastLength;

    public float lFootWeight;
    public float rFootWeight;

    private Transform lFootTarget, lKneeTarget;
    private Transform rFootTarget, rKneeTarget;

    [Header("Debug Mesh Settings")]
    public Mesh renderMesh;
    public Material renderMat;
    public bool visualize;

    public float kneeValueL, kneeValueR;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        lFootTarget = BuildTarget("Left Foot Target");
        rKneeTarget = BuildTarget("Left Knee Target");

        rFootTarget = BuildTarget("Right Foot Target");
        lKneeTarget = BuildTarget("Right Knee Target");
    }

    /// <summary>
    /// Returns the transform of a new GameObject with the given name.
    /// </summary>
    /// <param name="name">Name of the new IK target.</param>
    /// <returns></returns>
    private Transform BuildTarget(string name, bool finalTarget = false)
    {
        GameObject g = new GameObject(name);

        if (visualize) g.AddComponent<MeshFilter>().mesh = renderMesh;
        if (visualize) g.AddComponent<MeshRenderer>().material = renderMat;
        g.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        return g.transform;
    }

    /// <summary>
    /// Sends out a raycast from each foot with information about position and normals.
    /// </summary>
    private void SetFootRaycasts()
    {
        // Left Foot Transform.
        if (Physics.Raycast(lFoot.transform.position, Vector3.down, out RaycastHit lFootHit, groundRaycastLength, ground, QueryTriggerInteraction.UseGlobal)
            && lFootTarget.gameObject.activeSelf)
        {
            lFootTarget.position = lFootHit.point;
            lFootTarget.transform.up = lFootHit.normal;
        }
 
        // Right Foot Transform.
        if (Physics.Raycast(rFoot.transform.position, Vector3.down, out RaycastHit rFootHit, groundRaycastLength, ground, QueryTriggerInteraction.UseGlobal)
            && rFootTarget.gameObject.activeSelf)
        {
            rFootTarget.position = rFootHit.point;
            rFootTarget.transform.up = rFootHit.normal;
        }
    }

    private void SetKneeRaycasts()
    {
        // Left Knee Transform.
        if (Physics.Raycast(lKnee.transform.position, Vector3.down + transform.forward, out RaycastHit lKneeHit, slopeRaycastLength, ground, QueryTriggerInteraction.UseGlobal)
            && lKneeTarget.gameObject.activeSelf && anim.GetFloat("LFootLerp") != 0f)
        {
            lKneeTarget.position = lKneeHit.point + lKneeHit.normal * 0.8f;
            lKneeTarget.transform.up = lKneeHit.normal;
        }

        // Right Knee Transform.
        if (Physics.Raycast(rKnee.transform.position, Vector3.down + transform.forward, out RaycastHit rKneeHit, slopeRaycastLength, ground, QueryTriggerInteraction.UseGlobal)
            && rKneeTarget.gameObject.activeSelf && anim.GetFloat("RFootLerp") != 0f)
        {
            rKneeTarget.position = rKneeHit.point + rKneeHit.normal * 0.8f;
            rKneeTarget.transform.up = rKneeHit.normal;
        }
    }

    /// <summary>
    /// Adjusts the animation IK position and rotation, as well as their respective weights.
    /// </summary>
    private void SetIKs()
    {
        // Set weights.
        if (anim.GetFloat("LFootLerp") > 0.01f)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LFootLerp"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LFootLerp"));
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
        }
        if (anim.GetFloat("RFootLerp") > 0.01f)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RFootLerp"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RFootLerp"));
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
        }

        //if (anim.GetFloat("LKneeLerp") > 0.01f)
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1f);
        //else
            //anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 0f);

        //if (anim.GetFloat("RKneeLerp") > 0.01f)
            anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1f);
        //else
            //anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 0f);



        // Set positions.
        anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFootTarget.position);
        anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFootTarget.rotation * transform.rotation);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rFootTarget.position);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rFootTarget.rotation * transform.rotation);

        anim.SetIKHintPosition(AvatarIKHint.LeftKnee, lKneeTarget.position);
        anim.SetIKHintPosition(AvatarIKHint.RightKnee, rKneeTarget.position);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        SetFootRaycasts();
        SetKneeRaycasts();
        SetIKs();

        Debug.DrawRay(lFoot.transform.position, Vector3.down * groundRaycastLength, Color.red, 0);
        Debug.DrawRay(rFoot.transform.position, Vector3.down * groundRaycastLength, Color.blue, 0);
        Debug.DrawRay(lKnee.transform.position, (Vector3.down + transform.forward) * slopeRaycastLength, Color.red, 0);
        Debug.DrawRay(rKnee.transform.position, (Vector3.down + transform.forward) * slopeRaycastLength, Color.blue, 0);
    }
}
