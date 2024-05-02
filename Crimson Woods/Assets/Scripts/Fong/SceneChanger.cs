using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to change scene after 32 seconds
        StartCoroutine(ChangeSceneAfterDelay(32f));
    }

    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Change the scene
        SceneManager.LoadScene(3);
    }
}