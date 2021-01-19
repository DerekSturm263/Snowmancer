using UnityEngine;

public class ShockwaveHit : MonoBehaviour
{
    private Player player;
    private ParticleSystem ps;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z))
    }
}
