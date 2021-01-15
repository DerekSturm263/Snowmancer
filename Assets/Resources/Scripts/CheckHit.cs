using UnityEngine;

public class CheckHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball"))
        {
            GetComponent<Animator>().SetBool("Hit", true);
        }
    }
}
