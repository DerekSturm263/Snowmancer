using UnityEngine;
using System.Linq;

public class LongRangedAttack : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject target;
    public Vector3 targetOffset;

    public Material ice;

    public ParticleSystem shockedParticles;
    public ParticleSystem fireParticles;
    public ParticleSystem frozenParticles;

    public enum AttackType
    {
        Fire, Ice, Electric, Wind
    }
    public AttackType attackType;

    public float size = 1f;
    public float speed = 5f;
    public float damage = 5f;
    public float lifeTime = 10f;

    public GameObject[] trailEffects = new GameObject[4];
    public GameObject[] hitEffects = new GameObject[4];

    [HideInInspector] public GameObject lightningTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        transform.localScale = new Vector3(size / 2f, size / 2f, size / 2f);

        Invoke("Despawn", lifeTime);
    }

    private void Update()
    {
        if (attackType != AttackType.Wind)
            return;

        rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    public void SeekTarget()
    {

        switch (attackType)
        {
            case AttackType.Ice:
                rb.velocity = TargetVector(target.transform).normalized * speed * 2.5f;
                trailEffects[0].SetActive(true);
                break;
            case AttackType.Fire:
                rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
                trailEffects[1].SetActive(true);
                break;
            case AttackType.Electric:
                transform.position = target.transform.position + targetOffset / 2.75f;
                trailEffects[2].SetActive(true);
                break;
            case AttackType.Wind:
                trailEffects[3].SetActive(true);
                break;
        }
    }

    private Vector3 TargetVector(Transform targetTransform)
    {
        return (targetTransform.position + targetOffset) - transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Despawn();

            switch (attackType)
            {
                case AttackType.Ice:
                    hitEffects[0].SetActive(true);
                    trailEffects[0].SetActive(false);
                    rb.velocity = Vector3.zero;
                    Freeze(other.gameObject);
                    break;
                case AttackType.Fire:
                    hitEffects[1].SetActive(true);
                    trailEffects[1].SetActive(false);
                    rb.velocity = Vector3.zero;
                    Burn(other.gameObject);
                    break;
                case AttackType.Electric:
                    lightningTarget = other.gameObject;
                    rb.velocity = Vector3.zero;
                    Invoke("SetShockTimer", 1.9f);
                    break;
                case AttackType.Wind:
                    hitEffects[3].SetActive(true);
                    trailEffects[3].SetActive(false);
                    rb.velocity = Vector3.zero;
                    Push(other.gameObject);
                    break;
            }
        }
    }

    public void SetShockTimer()
    {
        hitEffects[2].SetActive(true);
        trailEffects[2].SetActive(false);
        
        if (Vector3.Distance(lightningTarget.transform.position, transform.position) < 2.5f)
        {
            Shock(lightningTarget);
        }
    }

    public void Freeze(GameObject hit)
    {
        hit.GetComponent<Animator>().speed = 0f;
        hit.GetComponent<PlayerMovement>().iceLeft = 2f;
        hit.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.Frozen;
        hit.GetComponent<Player>().health -= 5f;

        ParticleSystem newParticles = Instantiate(frozenParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;

        hit.GetComponent<PlayerMovement>().materials.ToList().ForEach(x =>
        {
            x.SetColor("_Tint", Color.cyan);
            x.SetFloat("_Smoothness", 0.1f);
        });
    }

    public void Shock(GameObject hit)
    {
        hit.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.Shocked;
        hit.GetComponent<Player>().health -= 10f;
        hit.GetComponent<Animator>().SetTrigger("Shocked");

        ParticleSystem newParticles = Instantiate(shockedParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;
    }

    public void Burn(GameObject hit)
    {
        hit.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.Burnt;
        hit.GetComponent<PlayerMovement>().timeBurnt = 7.5f;
        hit.GetComponent<Player>().health -= 5f;

        ParticleSystem newParticles = Instantiate(fireParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;

        hit.GetComponent<PlayerMovement>().materials.ToList().ForEach(x =>
        {
            x.SetFloat("_DamageWeight", 0.75f);
        });
    }

    public void Push(GameObject hit)
    {
        hit.GetComponent<Rigidbody>().AddForce((hit.transform.position - transform.position).normalized * 100f, ForceMode.Acceleration);
        hit.GetComponent<Player>().health -= 15f;
    }
}
