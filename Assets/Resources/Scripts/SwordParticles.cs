using UnityEngine;

public class SwordParticles :  MonoBehaviour
{
    private ParticleSystem swordParticles;

    private void Start()
    {
        swordParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void StartParticles()
    {
        swordParticles.Play();
    }

    public void StopParticles()
    {
        swordParticles.Stop();
    }
}
