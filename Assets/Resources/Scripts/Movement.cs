using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Animator anim;
    protected ParticleSystem ps;
    protected Rigidbody rb;

    public Camera cam;
    public LayerMask ground;

    protected Vector3 movementVector;
    protected bool isRunning;

    protected float timeFalling;

    protected int footNum;

    protected bool mouseAim;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Move(Vector2 move, bool useCamera = true)
    {
        movementVector = Vector3.zero;

        if (move == Vector2.zero)
        {
            if (ps)
                if (ps.isPlaying)
                    ps.Stop();

            anim.SetFloat("Speed", 0f);
            return;
        }
        if (ps) ps.Play();

        float multiplier = isRunning ? 2f : 1f;

        #region Camera Direction Setting

        if (useCamera)
        {
            movementVector += move.x * cam.transform.right * multiplier;
            movementVector += move.y * cam.transform.forward * multiplier;
        }
        else
        {
            movementVector += move.x * Vector3.right * multiplier;
            movementVector += move.y * Vector3.forward * multiplier;
        }

        #endregion

        if (!mouseAim && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        anim.SetFloat("Speed", movementVector.magnitude);
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
    }

    protected void Run(bool run)
    {
        isRunning = run;
    }

    protected void Jump()
    {
        #region Last Foot Setting

        float lFoot = anim.GetFloat("LeftFootWeight");
        float rFoot = anim.GetFloat("RightFootWeight");
        float runMultiplier = isRunning ? 1f : 0.5f;

        if (lFoot > rFoot)
            footNum = -1;
        else if (rFoot > lFoot)
            footNum = 1;
        else
            footNum = 0;

        anim.SetFloat("Last Foot", footNum * runMultiplier);

        #endregion

        anim.SetTrigger("Jump");
    }

    // Stick the player to the ground to avoid "floating".
    protected void LateUpdate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            return;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, ground) && anim.velocity.magnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hit.point.y, transform.position.z), Time.deltaTime * 20f);
        }
    }

    protected bool IsGrounded()
    {
        Vector3 boxOffset = new Vector3(0f, 0.75f, 0f);
        Vector3 boxSize = new Vector3(1f, 1f, 1f);
        float distance = 0.5f;

        return Physics.BoxCast(transform.position + boxOffset, boxSize / 2f, Vector3.down, Quaternion.identity, distance, ground, QueryTriggerInteraction.UseGlobal);
    }
}
