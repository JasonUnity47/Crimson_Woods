using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    // List of audio sources you want to control
    public List<AudioSource> audioSources;

    // References to the Sound On and Sound Off buttons
    public Button soundOnButton;
    public Button soundOffButton;

    // Variable to store the last saved volume
    private float lastVolume;

    void Start()
    {
        if (audioSources.Count > 0)
        {
            volumeSlider.value = audioSources[0].volume;
        }

        // Add a listener for when the slider value changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Load the saved volume value
        float savedVolume = SaveSystem.LoadVolume();
        lastVolume = savedVolume; // Store the saved volume

        // Set the initial value of the slider to match the saved volume
        volumeSlider.value = savedVolume;

        // Set the volume of all audio sources to match the saved volume
        OnVolumeChanged(savedVolume);

        // Load and set the button states
        LoadButtonStates();
    }

    public void OnVolumeChanged(float value)
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = value; // Directly set the volume
        }

        // Update button states
        soundOffButton.gameObject.SetActive(false);
        soundOnButton.gameObject.SetActive(true);

        // Save button states
        SaveButtonStates();

        // Save the current volume setting
        SaveSystem.SaveVolume(value);

        // Update lastVolume variable
        lastVolume = value;
    }

    

    // Method to mute sound
    public void SoundOff()
    {
        volumeSlider.value = 0; // Set slider to 0
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0; // Directly set the volume
        }

        // Update button states
        soundOnButton.gameObject.SetActive(false);
        soundOffButton.gameObject.SetActive(true);

        // Save button states
        SaveButtonStates();
    }

    // Method to unmute sound
    public void SoundOn()
    {
        volumeSlider.value = lastVolume; // Set slider to last saved volume
        OnVolumeChanged(lastVolume); // Update volume of audio sources

        // Update button states
        soundOffButton.gameObject.SetActive(false);
        soundOnButton.gameObject.SetActive(true);

        // Save button states
        SaveButtonStates();
    }

    // Save button states using PlayerPrefs
    void SaveButtonStates()
    {
        PlayerPrefs.SetInt("SoundOnButtonActive", soundOnButton.gameObject.activeSelf ? 1 : 0);
        PlayerPrefs.SetInt("SoundOffButtonActive", soundOffButton.gameObject.activeSelf ? 1 : 0);
    }

    // Load button states using PlayerPrefs
    void LoadButtonStates()
    {
        bool soundOnActive = PlayerPrefs.GetInt("SoundOnButtonActive", 0) == 1;
        bool soundOffActive = PlayerPrefs.GetInt("SoundOffButtonActive", 1) == 1;

        soundOnButton.gameObject.SetActive(soundOnActive);
        soundOffButton.gameObject.SetActive(soundOffActive);
    }

    // Call save on application quit
    void OnApplicationQuit()
    {
        SaveButtonStates();
    }
}