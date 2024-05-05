using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDatas : MonoBehaviour
{
    public int userId;
    private PlayerController playerController;
    private Shooting shooting;
    private PlayerHealth playerHealth;
    private HealthHeartBar healthHeartBar;

    float[] dashCD = { 1f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f };
    float[] fireRate = { 1f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f };
    int[] health = { 0, 1, 2, 3, 4, 5 };
    float[] moveSpeed = { 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

    private void Awake()
    {
        // Retrieve userId from PlayerPrefs
        userId = PlayerPrefs.GetInt("userId", 0);

        playerController = GameObject.FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
        shooting = GameObject.FindAnyObjectByType<Shooting>().GetComponent<Shooting>();
        playerHealth = GameObject.FindAnyObjectByType<PlayerHealth>().GetComponent<PlayerHealth>();
        healthHeartBar = GameObject.FindAnyObjectByType<HealthHeartBar>().GetComponent<HealthHeartBar>();

        StartCoroutine(GetPlayerData());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator GetPlayerData()
    {
        string url = "https://jasonunity47.000webhostapp.com/shop.php?userId=" + userId; // Include userId parameter in the URL
        // Get player data from the server
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to get player data: " + www.error);
            }
            else
            {
                // Parse the JSON data
                PlayerDatasIndexs playerData = JsonUtility.FromJson<PlayerDatasIndexs>(www.downloadHandler.text);

                if (playerData != null)
                {
                    // Assign player data to PlayerController
                    playerController.dashRestoreTime *= dashCD[playerData.dashCD];
                    shooting.timeBetweenFiring *= fireRate[playerData.fireRate];
                    playerHealth.maxHealth += health[playerData.health];
                    playerController.moveSpeed *= moveSpeed[playerData.moveSpeed];
                    playerHealth.health = playerHealth.maxHealth;
                    healthHeartBar.DrawHearts();

                }
                else
                {
                    Debug.LogError("Failed to parse player data.");
                }
            }
        }
    }
}
