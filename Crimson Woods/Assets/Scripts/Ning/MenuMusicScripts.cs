using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicScripts : MonoBehaviour
{
    // Reference to the audio source for background music
    public AudioSource backgroundMusic;

    // Build indices of scenes where background music should play
    public int[] scenesWithMusic = { 0, 1 }; // Replace 0 and 1 with the build indices of the scenes where you want the music to play

    // Boolean flag to track whether the music is currently playing
    private bool isMusicPlaying = false;

    void Start()
    {
        // Persist this GameObject across scenes
        DontDestroyOnLoad(gameObject);

        // Initially check the current scene to start/stop music
        UpdateBackgroundMusic();
    }

    void Update()
    {
        // Update the background music based on the current active scene
        UpdateBackgroundMusic();
    }

    // Update the background music based on the current active scene
    private void UpdateBackgroundMusic()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;

        // Check if the current scene index is in the list of scenes with music
        bool shouldPlayMusic = false;
        foreach (int sceneIndex in scenesWithMusic)
        {
            if (currentSceneIndex == sceneIndex)
            {
                shouldPlayMusic = true;
                break;
            }
        }

        // Start or stop the background music based on whether it should play
        if (shouldPlayMusic && !isMusicPlaying)
        {
            // Start the background music
            backgroundMusic.Play();
            isMusicPlaying = true;
        }
        else if (!shouldPlayMusic && isMusicPlaying)
        {
            // Stop the background music
            backgroundMusic.Stop();
            isMusicPlaying = false;
        }
    }
}