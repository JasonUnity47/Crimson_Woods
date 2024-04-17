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

    // Function to save the bloodSave value
    public static void SaveBloodSave(int bloodSave)
    {
        PlayerPrefs.SetInt("BloodSave", bloodSave);
        PlayerPrefs.Save();
    }

    // Function to load the bloodSave value
    public static int LoadBloodSave()
    {
        return PlayerPrefs.GetInt("BloodSave", 0); // Default bloodSave is 0 if not found
    }
}
