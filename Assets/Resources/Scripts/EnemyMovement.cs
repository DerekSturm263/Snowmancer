using UnityEngine;

public class EnemyMovement : Movement
{
    public GameObject player;

    private void Update()
    {
        Vector3 targetVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z).normalized;
        Move(targetVector, false);
    }
}
