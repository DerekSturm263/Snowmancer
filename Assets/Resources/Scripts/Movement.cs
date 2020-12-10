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

    protected bool mouseAim;

    protected bool isGrounded;

    private Vector3 targetPos;

    public float groundedDistance;
    public float groundedVelocity;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        targetPos = transform.position;

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

        if (!mouseAim)
            anim.SetFloat("Speed", movementVector.magnitude);
        else
            anim.SetFloat("Speed", Input.GetAxis("Vertical"));

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
    }

    protected void Run(bool run)
    {
        isRunning = run;
    }

    protected void Jump()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            return;

        anim.SetTrigger("Jump");
    }

    // Stick the player to the ground to avoid "floating".
    protected void LateUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, ground))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
                isGrounded = ((hit.distance < groundedDistance || Mathf.Abs(rb.velocity.normalized.y) < groundedVelocity) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) ? true : false;
            else
                isGrounded = hit.distance < 0.2f;
        }



        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit2, ground) && movementVector != Vector3.zero)
            targetPos = new Vector3(transform.position.x, hit2.point.y, transform.position.z);

        if (rb.velocity.magnitude > 0.25f && isGrounded) transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 20f);
    }
}
