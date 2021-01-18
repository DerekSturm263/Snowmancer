using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckHit : MonoBehaviour
{
    public static List<CheckHit> icicles = new List<CheckHit>();
    public static List<CheckHit> deadIcicles = new List<CheckHit>();

    [HideInInspector] public Animator anim;

    private Vector3 startPos;
    public bool isActive = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        startPos = transform.position;
        icicles.Add(this);
        deadIcicles.Add(this);
    }

    private void Update()
    {
        if (transform.localScale.magnitude < 4.2f)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
        }
    }

    public void Regrow()
    {
        deadIcicles.Add(this);
        isActive = true;

        transform.position = startPos;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball") && isActive)
        {
            GetComponent<Animator>().SetTrigger("Hit");
            return;
        }
        else
        {
            if (deadIcicles.Contains(this))
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    deadIcicles.Remove(this);
                }
                else if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Player>().TakeDamage(25f);
                    deadIcicles.Remove(this);
                }
                else if (collision.gameObject.CompareTag("Boss"))
                {
                    collision.gameObject.GetComponent<Boss>().TakeIcicleDamage(200f);
                    deadIcicles.Remove(this);
                }

                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}
