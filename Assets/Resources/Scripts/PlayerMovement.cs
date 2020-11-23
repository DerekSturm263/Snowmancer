using UnityEngine;

public class PlayerMovement : Movement
{
    private void Update()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        Run(Input.GetButton("Run"));
        if (Input.GetButtonDown("Jump")) Jump();

        movementVector = targetVector;

        if (targetVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        anim.SetFloat("Vertical", movementVector.magnitude);

        if (Input.GetButton("Run")) moveState = MoveState.Running;


        if (rb.velocity.y < -1f)
            timeFalling += Time.deltaTime;
        else
            timeFalling = 0f;

        anim.SetFloat("TimeFalling", timeFalling);
        


        float runMultiplier = isRunning ? 1f : 0.5f;

        float lFoot = anim.GetFloat("LeftFootWeight");
        float rFoot = anim.GetFloat("RightFootWeight");

        float foot;

        if (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude != 0f)
            foot = lFoot > rFoot ? -1f : 1f;
        else
            foot = 0f;

        anim.SetFloat("LastFoot", foot * runMultiplier);
    }
}
