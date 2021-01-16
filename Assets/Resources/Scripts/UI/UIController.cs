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

    public GameObject bossHealth;
    public Slider bossSlider;
    public Boss boss;

    public Canvas mainCanvas;
    public GameObject gameOverCanvas;

    void Start()
    {
        Shader.SetGlobalFloat("_BlacknessLerp", 0f);
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

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverCanvas.activeSelf)
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
            Application.Quit();

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
        snowMeter.fillAmount = fillAmount * 0.383f;
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

    //Boss Health
    public void ShowBossHealth(string name)
    {
        bossHealth.SetActive(true);
        bossHealth.GetComponentInChildren<TMPro.TMP_Text>().text = name;
    }

    public void HideBossHealth()
    {
        bossHealth.SetActive(false);
    }

    public void SetMaxBossHealth()
    {
        bossSlider.maxValue = boss.maxHealth;
        bossSlider.value = bossSlider.maxValue;
    }

    public void SetBossHealth()
    {
        bossSlider.value = Mathf.Lerp(bossSlider.value, boss.health, Time.deltaTime * 10f);
        bossSlider.GetComponentsInChildren<Image>()[1].color = Color.Lerp(Color.red, Color.green, boss.health / boss.maxHealth);
    }

    public void GameOver()
    {
        // Wasn't fading properly so I changed it to just make the alpha 0. Consider using animations for this instead.
        mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        gameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        CameraController camCont = player.cam.GetComponentInParent<CameraController>();
        camCont.CameraFollowObj = camCont.follow2;
        camCont.CamOffset.x = 0f;
        camCont.CamOffset.y = 0f;

        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown()
    {
        for (float i = 0f; i < 1.5f; i += Time.deltaTime)
        {
            CameraController.isActive = false;
            Shader.SetGlobalFloat("_BlacknessLerp", i / 1f);
            Time.timeScale = 1.5f - i;
            yield return new WaitForEndOfFrame();
        }
    }
}
