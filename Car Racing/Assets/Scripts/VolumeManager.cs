using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI textVolumeButton;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    public void Save()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void Load()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    public void MuteUnmute()
    {
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetFloat("musicVolume", 0);
            textVolumeButton.text = "Mute";
            volumeSlider.value = 0;
            Debug.Log(PlayerPrefs.GetFloat("musicVolume"));
            Debug.Log(textVolumeButton.text);
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetFloat("musicVolume", 1);
            textVolumeButton.text = "Unmute";
            volumeSlider.value = 1;
            Debug.Log(PlayerPrefs.GetFloat("musicVolume"));
            Debug.Log(textVolumeButton.text);
        }
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
