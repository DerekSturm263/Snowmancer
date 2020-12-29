using UnityEngine;

public class PlayerMovement : Movement
{
    public static bool useHeadIK;

    [Header("Particles")]
    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;
    public ParticleSystem moveParticles;

    private void Update()
    {
        #region Player Input

        Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        Run(Input.GetButton("Run"));
        if (Input.GetButtonDown("Jump")) Jump();

        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), targetSpeed, Time.deltaTime * 10f));
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        #endregion

        #region Aiming

        mouseAim = Input.GetMouseButton(1);
        anim.SetBool("Strafing", mouseAim);

        if (mouseAim && IsGrounded())
        {
            transform.forward = cam.transform.forward;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        useHeadIK = mouseAim;

        #endregion

        #region Moving Particles

        if ((!IsGrounded() || movementVector.magnitude <= 0.025f) && moveParticles.isPlaying)
        {
            Debug.Log("Stop");
            moveParticles.Stop();
        }
        else if (IsGrounded() && movementVector.magnitude > 0f && !moveParticles.isPlaying)
        {
            Debug.Log("Play");
            moveParticles.Play();
        }

        #endregion

        anim.SetBool("Grounded", IsGrounded());
    }
}
