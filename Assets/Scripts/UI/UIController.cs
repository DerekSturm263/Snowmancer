using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //public Slider healthSlider;
    //public Slider manaSlider;
    public GameObject spellSelector;

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.E))
        {
            spellSelector.SetActive(true);
        }
        else
        {
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
