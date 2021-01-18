using UnityEngine;
using System.Collections;

public class CheckHit : MonoBehaviour
{
    private Vector3 startPos;
    public bool isActive = true;

    private void Awake()
    {
        startPos = transform.position;
    }

    public void Regrow()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Animator>().enabled = true;
        transform.position = startPos;
        StartCoroutine(RegrowSize());
    }

    private IEnumerator RegrowSize()
    {
        for (float i = 1f; i > 0f; i -= Time.deltaTime)
        {
            transform.localScale = new Vector3(1f - i, 1f - i, 1f - i);
            yield return new WaitForEndOfFrame();
        }

        isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball") && isActive)
        {
            GetComponent<Animator>().SetBool("Hit", true);
        }
    }
}
