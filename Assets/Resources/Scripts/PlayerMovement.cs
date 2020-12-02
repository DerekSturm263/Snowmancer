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
        


        float runMultiplier = 0.5f;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") && targetVector != Vector3.zero)
        {
            float lFoot = anim.GetFloat("LeftFootWeight");
            float rFoot = anim.GetFloat("RightFootWeight");

            footNum = lFoot > rFoot ? 1 : -1;
            runMultiplier = isRunning ? 1f : 0.5f;
        }

        anim.SetFloat("LastFoot", footNum * runMultiplier);
    }
}
