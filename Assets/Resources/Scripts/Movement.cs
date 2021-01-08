using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Movement : MonoBehaviour
{
    public enum StatusEffect
    {
        None, Burnt, Frozen, Shocked
    }
    public StatusEffect statusEffect = StatusEffect.None;

    protected Animator anim;
    protected Rigidbody rb;

    protected Camera cam;
    public LayerMask ground;

    [HideInInspector] public Vector3 movementVector;
    protected bool isRunning;

    protected float timeFalling;

    protected bool mouseAim;

    [SerializeField] private Vector3 targetPos;

    public float groundedDistance;
    public float groundedVelocity;

    private float runMultiplier;
    private float slopeMultiplier;

    protected float targetSpeed;

    protected Vector3 minLoc, maxLoc;
    [HideInInspector] public float iceLeft = 0f;

    public List<Material> materials = new List<Material>();
    public List<Color> materialColors = new List<Color>();
    public List<float> materialFloats = new List<float>();

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        targetPos = transform.position;

        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material m in mr.materials)
            {
                materials.Add(m);
                materialColors.Add(m.GetColor("_Tint"));
                materialFloats.Add(m.GetFloat("_Smoothness"));
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Move(Vector2 move, bool useCamera = true)
    {
        if (anim.speed == 0f)
            return;

        movementVector = Vector3.zero;
        minLoc = transform.position - new Vector3(0.5f, 0f, 0.5f);
        maxLoc = transform.position + new Vector3(0.5f, 0f, 0.5f);

        if (move == Vector2.zero)
        {
            targetSpeed = 0f;
            return;
        }

        #region Camera Direction Setting

        if (useCamera)
        {
            movementVector += move.x * cam.transform.right;
            movementVector += move.y * cam.transform.forward;
        }
        else
        {
            movementVector += move.x * Vector3.right;
            movementVector += move.y * Vector3.forward;
        }

        #endregion

        float speed = movementVector.normalized.magnitude * runMultiplier * slopeMultiplier;

        if (!mouseAim && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementVector, Vector3.up), 15f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        if (!mouseAim)
            targetSpeed = speed;
        else
            targetSpeed = Input.GetAxis("Vertical");
    }

    protected void Shake(Vector2 move)
    {
        if (anim.speed != 0f)
            return;

        float x = Mathf.Lerp(minLoc.x, maxLoc.x, ((move.x + 1f) / 2f) * Time.deltaTime * 10f);
        float z = Mathf.Lerp(minLoc.z, maxLoc.z, ((move.y + 1f) / 2f) * Time.deltaTime * 10f);

        transform.position = new Vector3(x, transform.position.y, z);
    }

    protected void Run(bool run)
    {
        runMultiplier = run ? 2f : 1f;
    }

    protected void Jump()
    {
        if (anim.speed == 0f)
            return;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            return;

        anim.SetTrigger("Jump");
    }

    // Stick the player to the ground to avoid "floating".
    protected void LateUpdate()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, ground))
        {
            float targetSlopeMultiplier = Mathf.Abs(Vector3.Angle(hit.normal, transform.forward) - 90f) / 35f + 1f;
            slopeMultiplier = Mathf.Lerp(slopeMultiplier, targetSlopeMultiplier, Time.deltaTime);
        }

        if ((Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit2, ground) && movementVector != Vector3.zero) || animState.IsName("Landing"))
            targetPos = new Vector3(transform.position.x, hit2.point.y, transform.position.z);

        if (hit.distance > 1f)
        {
            if (!IsGrounded() && Mathf.Abs(rb.velocity.y) > 0.1f && animState.IsName("Grounded"))
                anim.SetTrigger("Fall");
        }
        else
        {
            if (animState.IsName("Grounded") || animState.IsName("Strafing") && rb.velocity.y > 0.1f && !animState.IsName("Landing"))
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * (hit.distance * 20f) * (hit.distance * 20f));
        }
    }

    protected bool IsGrounded()
    {
        bool grounded = Physics.BoxCast(transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, Quaternion.identity, 0.7f, ground);
        
        if (grounded)
            anim.ResetTrigger("Hit Wall");

        return grounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !IsGrounded())
            anim.SetTrigger("Hit Wall");
    }
}
