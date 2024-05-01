using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LeaderBoard : MonoBehaviour
{
    public TMP_Text[] userName;
    public TMP_Text[] timeSpent;

    // URL of the PHP script to retrieve leaderboard data
    private string phpURL = "http://localhost/cwgd/leaderboard.php";

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve leaderboard data from the database
        StartCoroutine(GetLeaderboardData());
    }

    // Coroutine to fetch leaderboard data from the database
    IEnumerator GetLeaderboardData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(phpURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch leaderboard data: " + www.error);
            }
            else
            {
                // Parse the JSON response
                string jsonResponse = www.downloadHandler.text;
                LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(jsonResponse);
                if (leaderboardData != null)
                {
                    // Update user names and time spent
                    for (int i = 0; i < leaderboardData.userNames.Length; i++)
                    {
                        userName[i].text = leaderboardData.userNames[i];
                        timeSpent[i].text = ConvertSecondsToFormattedTime(leaderboardData.timeSpent[i]);
                    }
                    Debug.Log(leaderboardData);
                }
                else
                {
                    Debug.LogError("Failed to parse leaderboard data.");
                }
            }
        }
    }

    // Convert seconds to "mm:ss" format
    string ConvertSecondsToFormattedTime(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }
}
