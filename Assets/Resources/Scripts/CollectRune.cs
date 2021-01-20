using UnityEngine;

public class CollectRune : MonoBehaviour
{
    public static bool[] unlockedSpells = new bool[5];
    private UIController uiCont;

    public enum RuneType
    {
        Ice, Fire, Electric, Wind
    }
    public RuneType runeType;

    private void Awake()
    {
        uiCont = FindObjectOfType<UIController>();

        uiCont.spellSelector.GetComponent<MenuScript>().menuItems[(int) runeType].GetComponent<MenuItemScript>().isLocked = !unlockedSpells[(int) runeType];
        gameObject.SetActive(!unlockedSpells[(int) runeType]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiCont.spellSelector.GetComponent<MenuScript>().menuItems[(int) runeType].GetComponent<MenuItemScript>().isLocked = false;
            unlockedSpells[(int) runeType] = true;
            Destroy(gameObject);
        }
    }
}
