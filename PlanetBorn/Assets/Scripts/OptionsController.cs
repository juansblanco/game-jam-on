using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Image icon;

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
            if(volumeSlider.value == 0)
            {
                icon.sprite = sprites[0];
            }
            else
            {
                icon.sprite = sprites[1];
            }
        }
        else
        {
            Debug.LogWarning("No music player found");
        }
    }
}