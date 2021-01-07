using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Transform cam;

    public Enemy enemy;
    public Slider healthSlider;

    void Awake()
    {
        cam = Camera.main.transform;
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
        healthSlider.value = Mathf.Lerp(healthSlider.value, enemy.GetComponent<Enemy>().health, Time.deltaTime * 10f);
        healthSlider.GetComponentsInChildren<Image>()[0].color = Color.Lerp(Color.red, Color.green, enemy.health / enemy.maxHealth);
    }
}
