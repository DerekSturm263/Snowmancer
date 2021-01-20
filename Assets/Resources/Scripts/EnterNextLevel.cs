using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevel : MonoBehaviour
{
    public Vector3 nextLevelStartPos;
    public Vector3 nextLevelCamStartPos;

    public bool active = false;
    public string nextLevel;
    private Player player;
    public GameObject camHolder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            player = other.gameObject.GetComponent<Player>();

            player.transform.position = nextLevelStartPos;
            camHolder.transform.position = nextLevelCamStartPos;

            SaveSystem.SavePlayer(player);
            SaveSystem.SaveElementData(CollectRune.unlockedSpells);
            SaveSystem.SaveSettingsData(new bool[5] { UIController.useFullscreen, UIController.useParticles, UIController.usePostProcessing, UIController.useAntiAliasing, UIController.useHints}, new float[2] { UIController.musicVolume, UIController.sfxVolume })

            SceneManager.LoadScene(nextLevel);
        }
    }
}
