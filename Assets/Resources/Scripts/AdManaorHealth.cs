using UnityEngine;

public class AdManaorHealth : MonoBehaviour
{
    public float healAmount;

    public enum PotionType
    {
        Health, Mana
    }
    public PotionType potionType;

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("AAAAAAAAA");

        if (col.CompareTag("Player"))
        {
            Debug.Log("BBBBBBBBBBBBBB");

            switch (potionType)
            {
                case PotionType.Health:
                    col.GetComponent<Player>().AddHealth(healAmount);
                    break;
                case PotionType.Mana:
                    col.GetComponent<Player>().AddMana(healAmount);
                    break;
            }
        }
    }
}
