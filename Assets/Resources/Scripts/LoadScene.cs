using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneString(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(SaveSystem.LoadScene());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
