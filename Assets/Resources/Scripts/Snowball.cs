using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snowball : MonoBehaviour
{
    public enum CurrentSpell { none, fire, ice, electric, air }
    public CurrentSpell currentSpell;
    public GameObject breakParticles;
    public bool breakable = false;
    public float damage;
    public float burnDamage;
    private Rigidbody rb;
    public ParticleSystem trailParticle;

    [Header("Trail Particles")]
    public ParticleSystem defaultTrailParticles;
    public ParticleSystem fireTrailParticles;
    public ParticleSystem iceTrailParticles;
    public ParticleSystem electricTrailParticles;
    public ParticleSystem airTrailParticles;

    [Header("Enemy Debuff Particles")]
    public ParticleSystem shockedParticles;
    public ParticleSystem fireParticles;
    public ParticleSystem frozenParticles;
    public Vector3 targetOffset;
    public GameObject shockBall;
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
            EnemyMovement enemy = col.gameObject.GetComponent<EnemyMovement>();
            switch (currentSpell)
            {
                case CurrentSpell.fire:
                    CompareEnemyTag(col);

                    if (enemy.statusEffect == Movement.StatusEffect.None)
                      {
                        enemy.statusEffect = Movement.StatusEffect.Burnt;
                        enemy.timeBurnt = 7.5f;
                        enemy.burnDamage = burnDamage;

                        ParticleSystem newParticles3 = Instantiate(fireParticles, col.transform);
                        newParticles3.transform.localPosition += targetOffset;

                        enemy.materials.ToList().ForEach(x =>
                        {
                              x.SetFloat("_DamageWeight", 0.75f);
                        });
                      }
                    break;
                case CurrentSpell.ice:
                    CompareEnemyTag(col);

                    if (enemy.statusEffect == Movement.StatusEffect.None)
                    {
                        enemy.iceLeft = 4.5f;
                        enemy.statusEffect = Movement.StatusEffect.Frozen;

                        ParticleSystem newParticles2 = Instantiate(frozenParticles, col.transform);
                        newParticles2.transform.localPosition += targetOffset;

                        enemy.materials.ToList().ForEach(x =>
                        {
                            x.SetColor("_Tint", Color.cyan);
                            x.SetFloat("_Smoothness", 0.1f);
                        });
                    }
                    break;

                case CurrentSpell.electric:
                    CompareEnemyTag(col);
                    if(enemy != null)
                    {
                        Instantiate(shockBall, enemy.transform.position, Quaternion.identity);
                    }
                    ParticleSystem newParticles = Instantiate(shockedParticles, col.transform);
                    newParticles.transform.localPosition += targetOffset;
                    break;

                default:
                    CompareEnemyTag(col);
                    break;
            }


            trailParticle.transform.parent = null;
            trailParticle.Stop();

            Instantiate(breakParticles, col.contacts[0].point, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private Vector3 TargetVector(Transform targetTransform)
    {
        return (targetTransform.position + targetOffset) - transform.position;
    }

    void CompareEnemyTag(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyMovement enemyMovement = col.gameObject.GetComponent<EnemyMovement>();
            enemyMovement.TakeDamage(damage);
        }
        else if (col.gameObject.CompareTag("Boss"))
        {
            Boss boss = col.gameObject.GetComponent<Boss>();
            boss.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
