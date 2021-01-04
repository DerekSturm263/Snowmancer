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

    public GameObject[] chargeEffects = new GameObject[4];
    public GameObject[] trailEffects = new GameObject[4];
    public GameObject[] hitEffects = new GameObject[4];

    public LayerMask ground;

    [HideInInspector] public GameObject lightningTarget;

    private bool hit = false;
    private bool active = false;

    [HideInInspector] public Transform origin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        transform.localScale = new Vector3(size / 2f, size / 2f, size / 2f);

        Invoke("Despawn", lifeTime);
    }

    private void Update()
    {
        if (!active)
            transform.position = origin.position;

        if (attackType != AttackType.Wind)
            return;

        rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    public void Activate()
    {
        switch (attackType)
        {
            case AttackType.Ice:
                chargeEffects[0].SetActive(true);
                break;
            case AttackType.Fire:
                chargeEffects[1].SetActive(true);
                break;
            case AttackType.Electric:
                chargeEffects[2].SetActive(true);
                break;
            case AttackType.Wind:
                chargeEffects[3].SetActive(true);
                break;
        }
    }

    public void SeekTarget()
    {
        active = true;

        switch (attackType)
        {
            case AttackType.Ice:
                rb.velocity = TargetVector(target.transform).normalized * speed * 2.5f;
                chargeEffects[0].SetActive(false);
                trailEffects[0].SetActive(true);
                break;
            case AttackType.Fire:
                rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
                chargeEffects[1].SetActive(false);
                trailEffects[1].SetActive(true);
                break;
            case AttackType.Electric:
                if (Physics.Linecast(target.transform.position + Vector3.up, target.transform.position + Vector3.down * 10f, out RaycastHit hit, ground))
                {
                    transform.position = hit.point;
                    chargeEffects[2].SetActive(false);
                    trailEffects[2].SetActive(true);
                }
                break;
            case AttackType.Wind:
                chargeEffects[3].SetActive(false);
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
            if (hit || !active) return;
            hit = true;

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
        
        if (Vector3.Distance(lightningTarget.transform.position, transform.position) < 2f)
        {
            Shock(lightningTarget);
        }
    }

    public void Freeze(GameObject hit)
    {
        PlayerMovement player = hit.GetComponent<PlayerMovement>();

        if (player.statusEffect == Movement.StatusEffect.None)
        {
            hit.GetComponent<Animator>().speed = 0f;
            player.iceLeft = 2f;
            player.statusEffect = Movement.StatusEffect.Frozen;
            hit.GetComponent<Player>().health -= damage;
        }

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
        PlayerMovement player = hit.GetComponent<PlayerMovement>();

        if (player.statusEffect == Movement.StatusEffect.None)
        {
            player.statusEffect = Movement.StatusEffect.Shocked;
            hit.GetComponent<Player>().health -= damage;
            hit.GetComponent<Animator>().SetTrigger("Shocked");
        }

        ParticleSystem newParticles = Instantiate(shockedParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;
    }

    public void Burn(GameObject hit)
    {
        PlayerMovement player = hit.GetComponent<PlayerMovement>();

        if (player.statusEffect == Movement.StatusEffect.None)
        {
            player.statusEffect = Movement.StatusEffect.Burnt;
            player.timeBurnt = 7.5f;
            hit.GetComponent<Player>().health -= damage;
        }

        ParticleSystem newParticles = Instantiate(fireParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;

        hit.GetComponent<PlayerMovement>().materials.ToList().ForEach(x =>
        {
            x.SetFloat("_DamageWeight", 0.75f);
        });
    }

    public void Push(GameObject hit)
    {
        Vector3 pushVector = (hit.transform.position - transform.position);

        pushVector.y = 0f;
        pushVector = pushVector.normalized;

        hit.GetComponent<Rigidbody>().AddForce(pushVector * 100f, ForceMode.Impulse); // Not working because of snapping script.
        hit.GetComponent<Player>().health -= damage;
    }
}
