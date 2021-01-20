using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using TMPro;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public static bool useFullscreen = true;
    public static bool useParticles = true;
    public static bool usePostProcessing = true;
    public static bool useAntiAliasing = true;
    public static bool useHints = true;

    public static float musicVolume = 1f;
    public static float sfxVolume = 1f;

    private DepthOfField dof;

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

    public float defaultFocusLength;
    public float pausedFocusLength;

    public GameObject racoonTip;
    public TMP_Text tipText;

    public GameObject settingsMenu;
    public GameObject graphicSettingsMenu;
    public GameObject soundSettingsMenu;
    public GameObject gameplaySettingsMenu;

    public Toggle[] toggles = new Toggle[5];
    public Slider[] sliders = new Slider[2];

    private void Awake()
    {
        float[] sliderValues = SaveSystem.SettingsFloats();
        bool[] boolValues = SaveSystem.SettingsBools();
        toggles[0].isOn = boolValues[0];
        toggles[1].isOn = boolValues[1];
        toggles[2].isOn = boolValues[2];
        toggles[3].isOn = boolValues[3];
        toggles[4].isOn = boolValues[4];

        sliders[0].value = sliderValues[0];
        sliders[1].value = sliderValues[1];


        FindObjectOfType<VisualEffect>().enabled = useParticles;
        if (useAntiAliasing)
        {
            QualitySettings.antiAliasing = 4;
        }
        else
        {
            QualitySettings.antiAliasing = 0;
        }
        Screen.fullScreen = useFullscreen;
        Volume v = FindObjectOfType<Camera>().GetComponent<Volume>();
        v.enabled = usePostProcessing;
    }

    void Start()
    {
        isPaused = false;
        Shader.SetGlobalFloat("_BlacknessLerp", 0f);
        SetMaxHealth();
        SetMaxMana();
       Camera.main.GetComponent<Volume>().profile.TryGet(out dof);
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

                dof.focusDistance.value = pausedFocusLength;

            }
            else if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.E))
            {
                menuItemSc.SetCurrentSpell();
                spellSelector.GetComponent<Animator>().SetTrigger("CloseSelect");
                Cursor.lockState = CursorLockMode.Locked;

                dof.focusDistance.value = defaultFocusLength;
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
        dof.focusDistance.value = defaultFocusLength;

        if (settingsMenu.activeSelf)
            CloseSettings();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        spellSelector.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        dof.focusDistance.value = pausedFocusLength;
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
        settingsMenu.SetActive(true);
        graphicsMenu();
        //Debug.Log("Settings open");
    }

    public void graphicsMenu()
    {
        soundSettingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(false);
        graphicSettingsMenu.SetActive(true);
    }

    public void audioMenu()
    {
        graphicSettingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(false);
        soundSettingsMenu.SetActive(true);
    }

    public void gameplayMenu()
    {
        graphicSettingsMenu.SetActive(false);
        soundSettingsMenu.SetActive(false);
        gameplaySettingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        // Something to apply settings here
        settingsMenu.SetActive(false);
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
            // CameraController.isActive = false;
            Shader.SetGlobalFloat("_BlacknessLerp", i / 1f);
            Time.timeScale = 1.5f - i;
            yield return new WaitForEndOfFrame();
        }
    }

    public void GiveTip(string setTipText, float displayTime)
    {
        
        racoonTip.SetActive(true);
        tipText.text = setTipText;
        Invoke("ExitTip", displayTime);
    }

    private void ExitTip()
    {
        racoonTip.GetComponent<Animator>().SetTrigger("Exit");
    }

    public void ToggleFullscreen()
    {
        useFullscreen = !useFullscreen;
        Screen.fullScreen = useFullscreen;
    }

    public void ToggleParticles()
    {
        useParticles = !useParticles;
        FindObjectOfType<VisualEffect>().enabled = useParticles;
    }

    public void TogglePostProcessing()
    {
        usePostProcessing = !usePostProcessing;

        Volume v = FindObjectOfType<Camera>().GetComponent<Volume>();
        v.enabled = usePostProcessing;
    }

    public void ToggleFancySky()
    {
        useAntiAliasing = !useAntiAliasing;

        if (useAntiAliasing)
        {
            QualitySettings.antiAliasing = 4;
        }
        else
        {
            QualitySettings.antiAliasing = 0;
        }
    }

    public void AdjustMusicVolume()
    {
        //musicVolume = sliders[0].value;
        //MusicPlayer.ChangeVolume(musicVolume);
    }

    public void AdjustSFXVolume()
    {
        //sfxVolume = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>().value;
        //SoundPlayer.ChangeVolume(sfxVolume);
    }
    
    public void ToggleTips()
    {
        useHints = !useHints;
    }

    public void ResetSaveData()
    {
        SavingLoadingTitle.resetSave = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}
