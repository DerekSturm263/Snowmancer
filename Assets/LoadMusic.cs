using UnityEngine;

public class LoadMusic : MonoBehaviour
{
    public AudioClip mainTheme;

    private void Awake()
    {
        MusicPlayer.Initialize();
        MusicPlayer.Play(mainTheme);
        MusicPlayer.ChangeVolume(0.25f);
    }
}
