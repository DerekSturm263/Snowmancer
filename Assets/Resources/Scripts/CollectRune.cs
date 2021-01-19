using UnityEngine;

public class CollectRune : MonoBehaviour
{
    private UIController uiCont;

    public enum RuneType
    {
        Ice, Fire, Electric, Wind
    }
    public RuneType runeType;

    private void Awake()
    {
        uiCont = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiCont.spellSelector.GetComponent<MenuScript>().menuItems[(int) runeType].GetComponent<MenuItemScript>().isLocked = false;
            Destroy(this);
        }
    }
}
