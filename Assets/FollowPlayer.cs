using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10f, player.transform.position.z);
    }
}
