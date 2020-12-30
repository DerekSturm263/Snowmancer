using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private PlayerMovement player;
    private Movement.StatusEffect thisEffect;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
        thisEffect = player.statusEffect;
    }

    private void Update()
    {
        if (player.statusEffect != thisEffect)
        {   
            if (thisEffect == Movement.StatusEffect.Burnt)
            {
                player.timeBurnt = -0.01f;
            }
            else if (thisEffect == Movement.StatusEffect.Frozen)
            {
                player.iceLeft = -0.01f;
            }
            Destroy(gameObject);
        }
    }
}
