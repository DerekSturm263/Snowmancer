using UnityEngine;

public class PlayerMovement : Movement
{
    private void Update()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        Run(Input.GetButton("Run"));

        movementVector = Vector3.Lerp(movementVector, targetVector, Time.deltaTime * 10f);

        if (targetVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        anim.SetFloat("Vertical", movementVector.magnitude);
    }
}
