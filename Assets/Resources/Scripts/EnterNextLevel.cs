using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNextLevel : MonoBehaviour
{
    public bool active = false;
    public string nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
