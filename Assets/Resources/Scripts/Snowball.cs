using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public enum CurrentSpell { none, fire, ice, electric, air }
    public CurrentSpell currentSpell;
    public GameObject breakParticles;
    public bool breakable = false;
    public float damage;
    private Rigidbody rb;
    public ParticleSystem trailParticle;

    [Header("Trail Particles")]
    public ParticleSystem defaultTrailParticles;
    public ParticleSystem fireTrailParticles;
    public ParticleSystem iceTrailParticles;
    public ParticleSystem electricTrailParticles;
    public ParticleSystem airTrailParticles;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        switch (currentSpell)
        {
            case CurrentSpell.none:
                trailParticle = Instantiate(defaultTrailParticles, this.transform);
                break;
            case CurrentSpell.fire:
                trailParticle = Instantiate(fireTrailParticles, this.transform);
                break;
            case CurrentSpell.ice:
                trailParticle = Instantiate(iceTrailParticles, this.transform);
                break;
            case CurrentSpell.electric:
                trailParticle = Instantiate(electricTrailParticles, this.transform);
                break;
            case CurrentSpell.air:
                trailParticle = Instantiate(airTrailParticles, this.transform);
                break;
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (rb.isKinematic == false)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("enemy hit");
                EnemyMovement enemyMovement = col.gameObject.GetComponent<EnemyMovement>();
                enemyMovement.TakeDamage(damage);
            }

            trailParticle.transform.parent = null;
            trailParticle.Stop();

            Instantiate(breakParticles, col.contacts[0].point, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
