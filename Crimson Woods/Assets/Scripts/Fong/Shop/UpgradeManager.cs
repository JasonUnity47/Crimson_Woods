using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpgradeManager : MonoBehaviour
{
    public int userId;// temporary user id

    // URL of the PHP script
    private string phpURL = "http://localhost/cwgd/upgrade.php";

    // Function to upgrade dashCD
    public void UpgradeDashCD()
    {
        // Retrieve userId from PlayerPrefs
        //int userId = PlayerPrefs.GetInt("userId");

        // Send request to upgrade dashCD
        StartCoroutine(Upgrade(userId, "upgradeDashCD"));
    }

    // Function to upgrade fireRate
    public void UpgradeFireRate()
    {
        // Retrieve userId from PlayerPrefs
        //int userId = PlayerPrefs.GetInt("userId");

        // Send request to upgrade fireRate
        StartCoroutine(Upgrade(userId, "upgradeFireRate"));
    }

    // Function to upgrade health
    public void UpgradeHealth()
    {
        // Retrieve userId from PlayerPrefs
        //int userId = PlayerPrefs.GetInt("userId");

        // Send request to upgrade health
        StartCoroutine(Upgrade(userId, "upgradeHealth"));
    }

    // Function to upgrade moveSpeed
    public void UpgradeMoveSpeed()
    {
        // Retrieve userId from PlayerPrefs
        //int userId = PlayerPrefs.GetInt("userId");

        // Send request to upgrade moveSpeed
        StartCoroutine(Upgrade(userId, "upgradeMoveSpeed"));
    }

    // Coroutine to send upgrade request to PHP script
    private IEnumerator Upgrade(int userId, string action)
    {
        // Create form with userId and action
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("action", action);

        // Send request to PHP script
        using (UnityWebRequest www = UnityWebRequest.Post(phpURL, form))
        {
            yield return www.SendWebRequest();

            // Check for errors
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Print response
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}