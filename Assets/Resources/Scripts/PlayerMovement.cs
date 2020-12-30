using UnityEngine;
using System.Linq;

public class PlayerMovement : Movement
{
    public static bool useHeadIK;
    public Player player;

    public static Vector3 playerHeadPos;

    [Header("Particles")]
    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;
    public ParticleSystem moveParticles;

    private void Update()
    {
        playerHeadPos = transform.position + new Vector3(0f, 2.5f, 0f);

        #region Player Input

        Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        Shake(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        Run(Input.GetButton("Run"));
        if (Input.GetButtonDown("Jump")) Jump();

        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), targetSpeed, Time.deltaTime * 10f));
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        #endregion

        #region Aiming

        mouseAim = Input.GetMouseButton(1);
        anim.SetBool("Strafing", mouseAim);

        if (mouseAim && IsGrounded() && anim.speed != 0f)
        {
            transform.forward = cam.transform.forward;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        useHeadIK = mouseAim;

        #endregion

        #region Moving Particles

        if (!IsGrounded() || movementVector.magnitude <= 0.025f)
        {
            if (moveParticles.isPlaying)
            {
                Debug.Log("Stop");
                moveParticles.Stop();
            }
        }
        else if (IsGrounded() && movementVector.magnitude > 0f && !moveParticles.isPlaying)
        {
            Debug.Log("Play");
            moveParticles.Play();
        }

        #endregion

        #region Ice Breaking

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            && iceLeft > 0f)
        {
            iceLeft -= 0.1f;
        }

        if (iceLeft < 0f)
        {
            anim.speed = 1f;
            iceLeft = 0f;

            foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                r.materials.ToList().ForEach(x =>
                {
                    // Change to be back to the original.
                    x.SetColor("_Tint", Color.yellow);
                    x.SetFloat("_Smoothness", 1000f);
                });
            }
        }

        #endregion

        #region Fire Damage

        if (statusEffect == StatusEffect.Burnt)
        {
            player.health -= Time.deltaTime * 2.5f;
        }

        #endregion

        anim.SetBool("Grounded", IsGrounded());
    }
}
