using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;
    [SerializeField] MusicPlayer musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefsController.CheckIfPrefsExist())
        {
            volumeSlider.value = PlayerPrefsController.GetMasterVolume();
        }
        else
        {
            volumeSlider.value = defaultVolume;
        }
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicPlayer)
        {
            musicPlayer.SetVolume(volumeSlider.value);
            PlayerPrefsController.SetMasterVolume(volumeSlider.value);
        }
        else
        {
            Debug.LogWarning("No music player found");
        }
    }
}