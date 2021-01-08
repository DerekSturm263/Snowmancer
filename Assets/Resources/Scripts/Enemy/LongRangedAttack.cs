using UnityEngine;
using System.Linq;

public class LongRangedAttack : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector] public Enemy user;

    public GameObject target;
    public Vector3 targetOffset;

    public Material ice;

    public ParticleSystem shockedParticles;
    public ParticleSystem fireParticles;
    public ParticleSystem frozenParticles;

    public Enemy.ElementType attackType;

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

        Invoke("Despawn", lifeTime);
    }

    private void Update()
    {
        if (origin == null)
            Destroy(gameObject);

        if (!active)
            transform.position = origin.position;

        if (attackType != Enemy.ElementType.Wind)
            return;

        rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    public void Initialize()
    {
        attackType = user.enemyType;
        damage = user.damage;
        speed = user.magicAttackSpeed;
        size = user.attackSize;
        lifeTime = user.magicAttackLifeTime;
        origin = user.wandTip;

        transform.localScale = new Vector3(size / 2f, size / 2f, size / 2f);
        chargeEffects[(int)attackType].SetActive(true);
    }

    public void SeekTarget()
    {
        active = true;

        if (attackType != Enemy.ElementType.Electric)
        {
            chargeEffects[(int)attackType].SetActive(false);
            trailEffects[(int)attackType].SetActive(true);
        }

        switch (attackType)
        {
            case Enemy.ElementType.Ice:
                rb.velocity = TargetVector(target.transform).normalized * speed * 2.5f;
                break;
            case Enemy.ElementType.Fire:
                rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
                break;
            case Enemy.ElementType.Electric:
                if (Physics.Linecast(target.transform.position + Vector3.up, target.transform.position + Vector3.down * 10f, out RaycastHit hit, ground))
                {
                    transform.position = hit.point;
                    transform.up = hit.normal;
                    chargeEffects[2].SetActive(false);
                    trailEffects[2].SetActive(true);
                }
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

            if (attackType != Enemy.ElementType.Electric)
            {
                hitEffects[(int) attackType].SetActive(true);
                trailEffects[(int)attackType].SetActive(false);

                Invoke("Despawn", 0.5f);
            }

            rb.velocity = Vector3.zero;

            switch (attackType)
            {
                case Enemy.ElementType.Ice:
                    Freeze(other.gameObject);
                    break;
                case Enemy.ElementType.Fire:
                    Burn(other.gameObject);
                    break;
                case Enemy.ElementType.Electric:
                    lightningTarget = other.gameObject;
                    Invoke("SetShockTimer", 1.9f);
                    Invoke("Despawn", 4f);
                    break;
                case Enemy.ElementType.Wind:
                    Push(other.gameObject);
                    break;
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            if (hit || !active) return;
            hit = true;

            if (attackType != Enemy.ElementType.Electric)
            {
                hitEffects[(int)attackType].SetActive(true);
                trailEffects[(int)attackType].SetActive(false);
            }

            rb.velocity = Vector3.zero;
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
