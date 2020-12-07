using UnityEngine;

public class PlayerMovement : Movement
{
    private void Update()
    {
        #region Player Input

        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        Run(Input.GetButton("Run"));
        if (Input.GetButtonDown("Jump")) Jump();

        #endregion

        #region Aiming

        mouseAim = Input.GetMouseButton(1);

        if (mouseAim)
        {
            anim.SetLayerWeight(1, 1f);

            transform.forward = cam.transform.forward;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            anim.SetLayerWeight(1, 0f);
        }

        #endregion

        anim.SetBool("Grounded", IsGrounded());
    }
}
