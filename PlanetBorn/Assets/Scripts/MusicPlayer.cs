using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    [Header("Audio clips")]
    public AudioClip main;
    public AudioClip game;

    // Start is called before the first frame update
    void Start()
    {
        SetUpSingleton();
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefsController.CheckIfPrefsExist())
        {
            audioSource.volume = PlayerPrefsController.GetMasterVolume();
        }
        else
        {
            audioSource.volume = 0.8f;
            PlayerPrefsController.SetMasterVolume(0.8f);
        }

        audioSource.clip = main;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        audioSource.Stop();
        if(level == 1)
        {
            audioSource.clip = game;
            audioSource.pitch = 1.0f;
        }
        else if (level == 0)
        {
            audioSource.clip = main;
            audioSource.pitch = 1.0f;
        }
        audioSource.Play();
    }

    private void SetUpSingleton()
    {
        int numberMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numberMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}