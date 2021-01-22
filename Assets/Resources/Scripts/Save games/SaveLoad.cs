using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public GameObject camera;
    public Player player;
    public GameObject gameoverCanvas;

    public static bool fireAlive, electricAlive, windAlive, finalSpawned, finalAlive;

    public static bool isSave()
    {
        return File.Exists(Application.persistentDataPath + "/player.savedata");
    }

    private void Awake()
    {
        LoadPlayer();
        SavePlayer();
    }

    //Save/load system stuff
    public void SavePlayer()
    {
        Debug.Log("Saved succesfully to " + Application.persistentDataPath);

        SaveSystem.SavePlayer(player);
        SaveSystem.SaveCamera(camera.transform.position);
        SaveSystem.SaveScene(SceneManager.GetActiveScene().name);
        SaveSystem.SaveElementData(CollectRune.unlockedSpells);
        SaveSystem.SaveSettingsData(new bool[5] { UIController.useFullscreen, UIController.useParticles, UIController.usePostProcessing, UIController.useAntiAliasing, UIController.useHints }, UIController.musicVolume );
        SaveSystem.SaveBossData(fireAlive, electricAlive, windAlive, finalSpawned, finalAlive);
    }

    public void Retry()
    {
        CameraController camCont = player.cam.GetComponentInParent<CameraController>();
        FindObjectOfType<UIController>().GetComponent<CanvasGroup>().alpha = 0f;

        camCont.CameraFollowObj = camCont.defaultFollowOb;
        camCont.CamOffset.x = 0.7f;
        camCont.CamOffset.y = 1.4f;

        CameraController.isActive = true;
        Shader.SetGlobalFloat("_BlacknessLerp", 0f);
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadPlayer(bool loadCoords = true)
    {
        PlayerData data = SaveSystem.LoadPLayer();
        CollectRune.unlockedSpells = SaveSystem.LoadElementData();
        BossData bossData = SaveSystem.LoadBoss();

        fireAlive = bossData.isFireAlive;
        electricAlive = bossData.isElectricAlive;
        windAlive = bossData.isWindAlive;
        finalSpawned = bossData.isFinalSpawned;
        finalAlive = bossData.isFireAlive;

        Debug.Log("BEFORE DEATH FIRE: " + fireAlive);
        Debug.Log("BEFORE DEATH ELECTRIC: " + electricAlive);
        Debug.Log("BEFORE DEATH WIND: " + windAlive);
        Debug.Log("BEFORE DEATH FINAL SPAWN: " + finalSpawned);
        Debug.Log("BEFORE DEATH FINAL ALIVE: " + finalAlive);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 1":
                FindObjectOfType<Boss>().gameObject.SetActive(bossData.isFireAlive);
                break;
            case "Level 2":
                FindObjectOfType<Boss>().gameObject.SetActive(bossData.isElectricAlive);
                break;
            case "Level 3 and 4":
                FindObjectsOfType<Boss>()[0].gameObject.SetActive(bossData.isWindAlive);
                FindObjectsOfType<Boss>()[1].gameObject.SetActive(bossData.isFinalAlive && bossData.isFinalSpawned);
                break;
        }

        bool[] boolValues = SaveSystem.SettingsBools();
        float musicValue = SaveSystem.SettingsFloats();

        UIController ui = FindObjectOfType<UIController>();

        ui.toggles[0].isOn = boolValues[0];
        ui.toggles[1].isOn = boolValues[1];
        ui.toggles[2].isOn = boolValues[2];
        ui.toggles[3].isOn = boolValues[3];
        ui.toggles[4].isOn = boolValues[4];

        ui.musicSlider.value = musicValue;

        Debug.Log("Loaded Save Data succesfully!");

        //player Info
        player.maxHealth = data.maxHealth;
        player.health = data.health;
        player.maxMana = data.maxMana;
        player.mana = data.mana;
        player.maxSnow = data.maxSnow;
        player.snowAmount = data.snowAmount;
        player.aiming = data.aiming;
        player.selectingSpell = data.selectingSpell;
        player.currentSpell = data.currentSpell;
        player.vLevel = data.vLevel;
        player.vExpLeft = data.vExpLeft;
        player.vCurrExp = data.vCurrExp;

        if (loadCoords)
        {
            camera.transform.position = SaveSystem.LoadCamera();

            Vector3 targetPos;
            targetPos.x = data.targetPos[0];
            targetPos.y = data.targetPos[1];
            targetPos.z = data.targetPos[2];
            player.GetComponent<PlayerMovement>().targetPos = targetPos;

            Vector3 postition;
            postition.x = data.postition[0];
            postition.y = data.postition[1];
            postition.z = data.postition[2];
            player.transform.position = postition;
        }
    }
}