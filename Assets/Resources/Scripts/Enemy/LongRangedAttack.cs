using UnityEngine;
using System.Linq;

public class LongRangedAttack : MonoBehaviour
{
    private Rigidbody rb;
    private ParticleSystem ps;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();

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
        ParticleSystem.MainModule main = ps.main;

        switch (attackType)
        {
            case AttackType.Ice:
                rb.velocity = TargetVector(target.transform).normalized * speed * 2.5f;
                main.startColor = Color.cyan;
                break;
            case AttackType.Fire:
                rb.velocity = TargetVector(target.transform).normalized * speed * 5f;
                main.startColor = Color.red;
                break;
            case AttackType.Electric:
                transform.position = target.transform.position;
                main.startColor = Color.yellow;
                break;
            case AttackType.Wind:
                main.startColor = Color.white;
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
            Despawn();

            switch (attackType)
            {
                case AttackType.Ice:
                    Freeze(other.gameObject);
                    break;
                case AttackType.Fire:
                    Burn(other.gameObject);
                    break;
                case AttackType.Electric:
                    Shock(other.gameObject);
                    break;
                case AttackType.Wind:
                    Push(other.gameObject);
                    break;
            }
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

        foreach (SkinnedMeshRenderer r in hit.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            r.materials.ToList().ForEach(x =>
            {
                x.SetColor("_Tint", Color.cyan);
                x.SetFloat("_Smoothness", 0.1f);
            });
        }
    }

    public void Shock(GameObject hit)
    {
        hit.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.Shocked;
        hit.GetComponent<Player>().health -= 10f;
        hit.GetComponent<Animator>().SetTrigger("Shocked");

        Instantiate(shockedParticles, hit.transform.position + targetOffset, Quaternion.Euler(-90, 0, 0));
    }

    public void Burn(GameObject hit)
    {
        hit.GetComponent<PlayerMovement>().statusEffect = Movement.StatusEffect.Burnt;
        hit.GetComponent<Player>().health -= 5f;

        ParticleSystem newParticles = Instantiate(fireParticles, hit.transform);
        newParticles.transform.localPosition += targetOffset;

        foreach (SkinnedMeshRenderer r in hit.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            r.materials.ToList().ForEach(x =>
            {
                x.SetFloat("_DamageWeight", 0.75f);
            });
        }
    }

    public void Push(GameObject hit)
    {
        hit.GetComponent<Rigidbody>().AddForce((hit.transform.position - transform.position).normalized * 100f, ForceMode.Acceleration);
        hit.GetComponent<Player>().health -= 15f;
    }
}
