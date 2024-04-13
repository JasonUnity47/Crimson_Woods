using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wave : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public WaveSpawner waveSpawner;

    private void Start()
    {
 
    }

    private void Update()
    {
        // Update the wave information in the UI
        if (waveSpawner != null)
        {
            // Display the current wave
            waveText.text = "Wave " + (waveSpawner.nextWave + 1);
        }
    }
}

