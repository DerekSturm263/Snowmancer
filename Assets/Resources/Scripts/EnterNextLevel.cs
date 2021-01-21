using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevel : MonoBehaviour
{
    public Vector3 nextLevelStartPos;
    public Vector3 nextLevelCamStartPos;

    public bool active = false;
    public string nextLevel;
    public GameObject camHolder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            LoadLevel(other.gameObject.GetComponent<Player>());
        }
    }

    public void LoadLevel(Player p)
    {
        p.transform.position = nextLevelStartPos;
        camHolder.transform.position = nextLevelCamStartPos;

        SaveSystem.SavePlayer(p);
        SaveSystem.SaveElementData(CollectRune.unlockedSpells);
        SaveSystem.SaveSettingsData(new bool[5] { UIController.useFullscreen, UIController.useParticles, UIController.usePostProcessing, UIController.useAntiAliasing, UIController.useHints }, new float[2] { UIController.musicVolume, UIController.sfxVolume });
        SaveSystem.SaveCamera(camHolder.transform.position);

        SceneManager.LoadScene(nextLevel);
    }
}
