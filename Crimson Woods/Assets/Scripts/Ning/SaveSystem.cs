using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Function to save the volume setting
    public static void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    // Function to load the volume setting
    public static float LoadVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 1.0f); // Default volume is 1.0f (max volume) if not found
    }

    
}
