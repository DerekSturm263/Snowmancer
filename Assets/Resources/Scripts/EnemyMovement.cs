using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    public GameObject player;
    public float speed;

    private void Update()
    {
        Move(new Vector2(transform.position.x - player.transform.position.x, transform.position.z - player.transform.position.z).normalized * speed);
        Run(false);

        movementVector = Vector3.Lerp(movementVector, targetVector, Time.deltaTime * 10f);

        if (targetVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVector, Vector3.up);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        anim.SetFloat("Vertical", movementVector.magnitude);
    }
}
