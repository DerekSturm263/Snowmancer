using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum MoveState
    {
        Idle, Walking, Running, Jumping, Falling, Landing
    }
    public static MoveState moveState;

    protected Animator anim;
    protected ParticleSystem ps;
    protected Rigidbody rb;

    public Camera cam;
    public LayerMask ground;

    protected Vector3 movementVector;
    protected Vector3 targetVector;
    protected bool isRunning;

    protected float timeFalling;

    protected int footNum;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Move(Vector2 move, bool useCamera = true)
    {
        if (move == Vector2.zero)
        {
            if (ps)
                if (ps.isPlaying)
                    ps.Stop();

            return;
        }
        if (ps) ps.Play();

        moveState = MoveState.Idle;
        targetVector = Vector3.zero;
        float multiplier = isRunning ? 2f : 1f;

        if (useCamera)
        {
            targetVector += move.x * cam.transform.right * multiplier;
            targetVector += move.y * cam.transform.forward * multiplier;
        }
        else
        {
            targetVector += move.x * Vector3.right * multiplier;
            targetVector += move.y * Vector3.forward * multiplier;
        }
    }

    protected void Run(bool run)
    {
        isRunning = run;
    }

    protected void Jump()
    {
        anim.SetTrigger("Jump");
    }

    protected void LateUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, ground) && anim.velocity.magnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hit.point.y, transform.position.z), Time.deltaTime * 20f);
        }
    }

    protected bool IsGrounded()
    {
        return true; // Will do this later.
    }
}
