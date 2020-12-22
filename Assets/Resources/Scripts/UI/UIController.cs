using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Player player;

    public Slider healthSlider;
    public Slider manaSlider;

    public GameObject spellSelector;
    private int spellID;
    private GameObject[] menuItems;
    private MenuItemScript menuItemSc;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public Image snowMeter;

    public Image expBar;
    public float scaler = 1f;

    void Start()
    {
        SetMaxHealth();
        SetMaxMana();
    }

    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.E))
            {
                spellSelector.SetActive(true);
                menuItems = spellSelector.GetComponent<MenuScript>().menuItems;
                spellID = spellSelector.GetComponent<MenuScript>().spellID;
                menuItemSc = menuItems[spellID].GetComponent<MenuItemScript>();
                Cursor.lockState = CursorLockMode.Confined;

            }
            else if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.E))
            {
                menuItemSc.SetCurrentSpell();
                spellSelector.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        // Update HUD meters
        SetSnowFill();
        SetHealth();
        SetMana();
        FillExp();

    }

    //Health
    public void SetMaxHealth()
    {
        healthSlider.maxValue = player.maxHealth;
        healthSlider.value = player.maxHealth;
    }

    public void SetHealth()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, player.health, Time.deltaTime * scaler);
    }

    //Mana
    public void SetMaxMana()
    {
        manaSlider.maxValue = player.maxMana;
        manaSlider.value = player.maxMana;
    }

    public void SetMana()
    {
        manaSlider.value = Mathf.Lerp(manaSlider.value, player.mana, Time.deltaTime * scaler);
    }

    //Snow
    public void SetSnowFill()
    {
        float fillAmount = (float)player.snowAmount / (float)player.maxSnow;
        snowMeter.fillAmount = fillAmount;
    }

    //EXP
    public void FillExp()
    {
        float fillAmount = (float)player.vCurrExp / (float)player.vExpLeft;
        expBar.fillAmount = fillAmount;
    }

    public void SetMaxExp(int maxExp)
    {
        player.vExpLeft = maxExp;
        player.vCurrExp = 0;
    }

    //Pause Menu
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        spellSelector.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(string sceneToLoad)
    {
        //Debug.Log("Load menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void OpenSettings()
    {
        Debug.Log("Settings open");
    }

    public bool CheckPaused()
    {
        return isPaused;
    }
}
