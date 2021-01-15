using UnityEngine;

public class CollectRune : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<Player>().AddSpell();
            Destroy(this);
        }
    }
}
