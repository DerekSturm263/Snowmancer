using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevel : MonoBehaviour
{
    public bool active = false;
    public string nextLevel;
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            SaveSystem.SavePlayer(player);
            SceneManager.LoadScene(nextLevel);
        }
    }
}
