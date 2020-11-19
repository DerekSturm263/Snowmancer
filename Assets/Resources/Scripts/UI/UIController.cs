using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //public Slider healthSlider;
    //public Slider manaSlider;
    public GameObject spellSelector;
    private int spellID;
    private GameObject[] menuItems;
    private MenuItemScript menuItemSc;

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.E))
        {
            spellSelector.SetActive(true);
            menuItems = spellSelector.GetComponent<MenuScript>().menuItems;
            spellID = spellSelector.GetComponent<MenuScript>().spellID;
            menuItemSc = menuItems[spellID].GetComponent<MenuItemScript>();

        }
        else if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.E))
        {
            menuItemSc.SetCurrentSpell();
            spellSelector.SetActive(false);
        }

    }

    ////Health
    //public void SetMaxHealth(int maxHealth)
    //{
    //    healthSlider.maxValue = maxHealth;
    //    healthSlider.value = maxHealth;
    //}

    //public void SetHealth(int health)
    //{
    //    healthSlider.value = health;
    //}

    ////Mana
    //public void SetMaxMana(int maxMana)
    //{
    //    manaSlider.maxValue = maxMana;
    //    manaSlider.value = maxMana;
    //}

    //public void SetMana(int mana)
    //{
    //    manaSlider.value = mana;
    //}
}
