using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Declaration
    public Sound[] sounds;

    public static AudioManager instance;

    // Reference to the volume slider
    public Slider volumeSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        foreach (Sound s  in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        // Attach the OnVolumeChanged method to the slider's onValueChanged event
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // Load the saved volume value and set it as the initial slider value
        float savedVolume = SaveSystem.LoadVolume();
        volumeSlider.value = savedVolume;
        OnVolumeChanged(savedVolume);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
        return;
    }

    // This method is called whenever the slider value changes
    void OnVolumeChanged(float volume)
    {
        // Update the volume of all audio sources in the sounds array
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }

        // Save the current volume setting
        SaveSystem.SaveVolume(volume);
    }
    // Using below code to play the specific sound you want.
    // FindObjectOfType<AudioManager>().Play("Sound Name");
}
