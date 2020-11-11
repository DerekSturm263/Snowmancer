using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem ps;

    public Camera cam;
    public LayerMask ground;

    private Vector3 movementVector;
    private Vector3 targetVector;
    private bool isRunning;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
    }

    public void Move(Vector2 move)
    {
        targetVector = Vector3.zero;
        float multiplier = isRunning ? 2f : 1f;

        targetVector += move.x * cam.transform.right * multiplier;
        targetVector += move.y * cam.transform.forward * multiplier;

        ps.Play();
    }

    public void Run(bool run)
    {
        isRunning = run;
    }

    public void Jump()
    {
        anim.SetTrigger("Jump");
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private void Update()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        Run(Input.GetButton("Run"));
        Cursor.lockState = CursorLockMode.Locked;

        movementVector = Vector3.Lerp(movementVector, targetVector, Time.deltaTime * 10f);

        if (targetVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);
        else
            ps.Stop();

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        anim.SetFloat("Vertical", movementVector.magnitude);
    }

    private void LateUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, ground) && anim.velocity.magnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hit.point.y, transform.position.z), Time.deltaTime * 20f);
        }
    }
}
