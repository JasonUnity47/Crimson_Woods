using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    // This script is only for each new round.

    // Declaration
    public int bloodCount;
    private int bloodSave;

    private void Start()
    {
        bloodCount = 0;
        LoadBloodSave();
    }

    private void OnApplicationQuit()
    {
        // Save fodSave when application quits
        SaveBloodSave();
    }

    // Function to save fodSave
    private void SaveBloodSave()
    {
        SaveSystem.SaveBloodSave(bloodSave);
    }

    // Function to load fodSave
    private void LoadBloodSave()
    {
        bloodSave = SaveSystem.LoadBloodSave();
    }

    public void bloodCurrency()
    {
        bloodSave += bloodCount;

        // Saving bloodSave
        SaveSystem.SaveBloodSave(bloodSave);
    }


}
