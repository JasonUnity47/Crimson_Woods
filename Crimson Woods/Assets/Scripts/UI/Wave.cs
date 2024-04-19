using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public WaveSpawner waveSpawner;
    // References to the wave and panel UI GameObjects
    public GameObject waveUI; // Wave UI GameObject
    public GameObject panelUI; // Panel UI GameObject

    private void Start()
    {
        // Initialize the UI to be inactive
        waveUI.SetActive(false);
        panelUI.SetActive(false);
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



    public IEnumerator ActivateWaveUI()
    {
        // Activate the UI GameObjects
        waveUI.SetActive(true);
        panelUI.SetActive(true);

        // Run fade-in coroutines simultaneously
        StartCoroutine(FadeGameObject(waveUI, 0, 1, 0.5f));
        StartCoroutine(FadeGameObject(panelUI, 0, 1, 0.5f));

        // Wait for the fade-in duration (0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Stay at full opacity for 1 second
        yield return new WaitForSeconds(1.0f);

        // Run fade-out coroutines simultaneously
        StartCoroutine(FadeGameObject(waveUI, 1, 0, 0.5f));
        StartCoroutine(FadeGameObject(panelUI, 1, 0, 0.5f));

        // Wait for the fade-out duration (0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Deactivate the UI GameObjects
        waveUI.SetActive(false);
        panelUI.SetActive(false);
    }


    private IEnumerator FadeGameObject(GameObject gameObject, float startAlpha, float endAlpha, float duration)
    {
        // Get the Renderer, Image, or TextMeshProUGUI component from the GameObject
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Image image = gameObject.GetComponent<Image>();
        TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();

        // Define the color variable to store the current color
        Color color;

        // Determine which component to use (Renderer, Image, or TextMeshProUGUI)
        if (textComponent != null)
        {
            // Use TextMeshProUGUI color
            color = textComponent.color;
        }
        else if (renderer != null)
        {
            // Use Renderer material color
            color = renderer.material.color;
        }
        else if (image != null)
        {
            // Use Image color
            color = image.color;
        }
        else
        {
            // No Renderer, Image, or TextMeshProUGUI found, exit the coroutine
            yield break;
        }

        // Start fading the alpha value
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Interpolate alpha value from startAlpha to endAlpha
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);

            // Update the alpha value in the color
            color.a = newAlpha;

            // Set the new color back to the appropriate component
            if (textComponent != null)
            {
                textComponent.color = color;
            }
            else if (renderer != null)
            {
                renderer.material.color = color;
            }
            else if (image != null)
            {
                image.color = color;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set
        color.a = endAlpha;

        // Set the final color to the appropriate component
        if (textComponent != null)
        {
            textComponent.color = color;
        }
        else if (renderer != null)
        {
            renderer.material.color = color;
        }
        else if (image != null)
        {
            image.color = color;
        }
    }
}