using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public enum CurrentSpell { none, fire, ice, electric, air }
    public CurrentSpell currentSpell;
    public GameObject breakParticles;
    public ParticleSystem trailParticles;

    private void OnCollisionEnter(Collision col)
    {
        trailParticles.transform.parent = null;
        trailParticles.Stop();
        
        Instantiate(breakParticles, col.contacts[0].point, Quaternion.identity);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
