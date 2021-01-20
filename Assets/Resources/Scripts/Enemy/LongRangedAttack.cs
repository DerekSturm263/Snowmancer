using UnityEngine;
using System.Linq;

public class LongRangedAttack : MonoBehaviour
{
    private GameObject player;

    private Rigidbody rb;
    [HideInInspector] public Enemy user;
    [HideInInspector] public Boss userBoss;
    private MeshRenderer mr;

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
    public float chargeTime = 2f;

    public GameObject[] chargeEffects = new GameObject[4];
    public GameObject[] trailEffects = new GameObject[4];
    public GameObject[] hitEffects = new GameObject[4];

    public LayerMask ground;

    private bool hit = false;
    private bool active = false;

    [HideInInspector] public Transform origin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        Invoke("Despawn", lifeTime);
    }

    private void Update()
    {
        if (transform.localScale.x < size)
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / 2f;

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

    public void Initialize(bool overrideAttackType = false)
    {
        if (user != null)
        {
            attackType = user.enemyType;
            damage = user.damage;
            speed = user.magicAttackSpeed;
            size = user.attackSize;
            lifeTime = user.magicAttackLifeTime;
            chargeTime = user.chargeTime;
            origin = user.wandTip;
        }
        else
        {
            if (!overrideAttackType) attackType = userBoss.currentAttack.Type;
            damage = userBoss.currentAttack.Damage;
            speed = userBoss.currentAttack.Speed;
            size = userBoss.currentAttack.Size;
            lifeTime = userBoss.currentAttack.LifeTime;
            chargeTime = userBoss.currentAttack.ChargeTime;
            origin = userBoss.wandTip;
        }

        switch (attackType)
        {
            case Enemy.ElementType.Fire:
                mr.material.SetColor("_BaseColor", new Color(1f, 0.5f, 0f, 0.5f));
                break;

            case Enemy.ElementType.Electric:
                mr.material.SetColor("_BaseColor", new Color(1f, 1f, 0f, 0.5f));
                break;

            case Enemy.ElementType.Ice:
                mr.material.SetColor("_BaseColor", new Color(0f, 0.5f, 1f, 0.5f));
                break;

            case Enemy.ElementType.Wind:
                mr.material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.5f));
                break;
        }

        chargeEffects[(int) attackType].SetActive(true);

        Invoke("SeekTarget", chargeTime);
    }

    public void SeekTarget()
    {
        mr.enabled = false;

        if (user != null)
        {
            user.GetComponent<Animator>().SetBool("Charging", false);
        }
        else
        {
            if (userBoss.type == Boss.ElementType.All)
            {
                userBoss.GetComponent<Animator>().SetBool("Charging Spell", false);
            }
            else if (userBoss.type != Boss.ElementType.Electric)
            {
                userBoss.GetComponent<Animator>().SetBool("Charging", false);
            }
            else if (userBoss.type == Boss.ElementType.Electric)
            {
                if (userBoss.currentAttack.LifeTime == 1f)
                {
                    userBoss.GetComponent<Animator>().SetBool("Charging Stomp", false);
                }
                else
                {
                    userBoss.GetComponent<Animator>().SetBool("Charging Spell", false);
                }
            }
            userBoss.timeSinceLastAttack = 0.1f;
        }

        active = true;

        if (attackType != Enemy.ElementType.Electric)
        {
            chargeEffects[(int)attackType].SetActive(false);
            trailEffects[(int)attackType].SetActive(true);
        }
        var main = trailEffects[(int)attackType].GetComponent<ParticleSystem>().main;
        main.startSize = size * 2f;


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
                    transform.position = hit.point - new Vector3(0f, 0.4f, 0f);
                    transform.up = hit.normal;
                    chargeEffects[2].SetActive(false);
                    trailEffects[2].SetActive(true);
                }
                Invoke("SetShockTimer", 1.9f);
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
            mr.enabled = false;

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
                case Enemy.ElementType.Wind:
                    Push(other.gameObject);
                    break;
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            if (hit || !active) return;
            hit = true;
            mr.enabled = false;

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
        
        if (Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            Shock(player);
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
            hit.GetComponent<Player>().TakeDamage(damage);

            ParticleSystem newParticles = Instantiate(frozenParticles, hit.transform);
            newParticles.transform.localPosition += targetOffset;

            hit.GetComponent<PlayerMovement>().materials.ToList().ForEach(x =>
            {
                x.SetColor("_Tint", Color.cyan);
                x.SetFloat("_Smoothness", 0.1f);
            });
        }
    }

    public void Shock(GameObject hit)
    {
        PlayerMovement player = hit.GetComponent<PlayerMovement>();

        if (player.statusEffect == Movement.StatusEffect.None)
        {
            player.statusEffect = Movement.StatusEffect.Shocked;
            hit.GetComponent<Player>().TakeDamage(damage);
            hit.GetComponent<Animator>().SetTrigger("Shocked");

            ParticleSystem newParticles = Instantiate(shockedParticles, hit.transform);
            newParticles.transform.localPosition += targetOffset;
        }
    }

    public void Burn(GameObject hit)
    {
        PlayerMovement player = hit.GetComponent<PlayerMovement>();

        if (player.statusEffect == Movement.StatusEffect.None)
        {
            player.statusEffect = Movement.StatusEffect.Burnt;
            player.timeBurnt = 7.5f;
            hit.GetComponent<Player>().TakeDamage(damage);

            ParticleSystem newParticles = Instantiate(fireParticles, hit.transform);
            newParticles.transform.localPosition += targetOffset;

            hit.GetComponent<PlayerMovement>().materials.ToList().ForEach(x =>
            {
                x.SetFloat("_DamageWeight", 0.75f);
            });
        }
    }

    public void Push(GameObject hit)
    {
        hit.GetComponent<Player>().TakeDamage(damage);
    }
}
