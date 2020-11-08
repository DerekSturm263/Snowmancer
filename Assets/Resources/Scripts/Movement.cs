using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem ps;

    public Camera cam;

    private Vector3 movementVector;
    private Vector3 targetVector;
    private bool isRunning;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Move(Vector2 amount)
    {
        targetVector = Vector3.zero;
        float multiplier = isRunning ? 2f : 1f;

        targetVector += amount.x * cam.transform.right * multiplier;
        targetVector += amount.y * cam.transform.forward * multiplier;

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

        movementVector = Vector3.Lerp(movementVector, targetVector, Time.deltaTime * 10f);

        if (targetVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);
        else
            ps.Stop();

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        anim.SetFloat("Vertical", movementVector.magnitude);
    }
}
