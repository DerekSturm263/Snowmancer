using UnityEngine;

public class CollectRune : MonoBehaviour
{
    public enum RuneType
    {
        Ice, Fire, Electric, Wind
    }
    public RuneType runeType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<Player>().AddSpell();
            Destroy(this);
        }
    }
}
