using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject newGame;
    public GameObject continueGame;

    private void Awake()
    {
        continueGame.SetActive(SaveLoad.isSave());
    }

    public void LoadSceneString(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(SaveSystem.LoadScene());
    }

    public void NewGame()
    {
        FindObjectOfType<SavingLoadingTitle>().NewSave();
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
