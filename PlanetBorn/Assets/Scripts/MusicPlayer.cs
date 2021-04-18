using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

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