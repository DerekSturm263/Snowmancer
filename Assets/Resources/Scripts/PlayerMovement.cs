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

    [HideInInspector] public float timeBurnt;

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
            && statusEffect == StatusEffect.Frozen)
        {
            iceLeft -= 0.1f;
        }

        if (iceLeft < 0f)
        {
            anim.speed = 1f;
            iceLeft = 0f;
            statusEffect = StatusEffect.None;

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetColor("_Tint", materialColors[i]);
                materials[i].SetFloat("_Smoothness", materialFloats[i]);
            }
        }

        #endregion

        #region Fire Damage

        if (statusEffect == StatusEffect.Burnt)
        {
            player.health -= Time.deltaTime * 2.5f;
            timeBurnt -= Time.deltaTime;
        }

        if (timeBurnt < 0f)
        {
            timeBurnt = 0f;
            statusEffect = StatusEffect.None;

            materials.ToList().ForEach(x =>
            {
                x.SetFloat("_DamageWeight", 0f);
            });
        }

        #endregion

        anim.SetBool("Grounded", IsGrounded());
    }
}
