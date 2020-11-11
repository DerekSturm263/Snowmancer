using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Animator anim;
    protected ParticleSystem ps;

    public Camera cam;
    public LayerMask ground;

    protected Vector3 movementVector;
    protected Vector3 targetVector;
    protected bool isRunning;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Move(Vector2 move)
    {
        if (move == Vector2.zero)
        {
            if (ps.isPlaying) ps.Stop();
            return;
        }
        ps.Play();

        targetVector = Vector3.zero;
        float multiplier = isRunning ? 2f : 1f;

        targetVector += move.x * cam.transform.right * multiplier;
        targetVector += move.y * cam.transform.forward * multiplier;
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
}
