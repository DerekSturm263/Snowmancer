using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Enemy enemy;
    public Transform cam;
    public Slider healthSlider;

    void Start()
    {
        SetMaxHealth();
    }

    void FixedUpdate()
    {
        SetHealth();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void SetMaxHealth()
    {
        healthSlider.maxValue = enemy.GetComponent<Enemy>().maxHealth;
        healthSlider.value = healthSlider.maxValue;
    }

    public void SetHealth()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, enemy.GetComponent<Enemy>().health, Time.deltaTime);
    }
}
