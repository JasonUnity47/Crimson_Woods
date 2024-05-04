using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class RecordTime : MonoBehaviour
{
    // Store Variable
    [Header("Store Variable")]
    public int userId;
    public int timeSpent;

    // Script Reference
    private Timer timer;
    private WaveSpawner waveSpawner;

    // URL
    [Header("URL")]
    [SerializeField] private string url = "https://jasonunity47.000webhostapp.com/insert_time.php";

    private void Start()
    {
        timer = GameObject.FindWithTag("Game Manager").GetComponent<Timer>();
        waveSpawner = GameObject.FindWithTag("Game Manager").GetComponent<WaveSpawner>();
    }

    private void Update()
    {
        timeSpent = Mathf.FloorToInt(timer.rawTime);
    }

    IEnumerator SendFrom(int userId, int timeSpent)
    {
        // Create a form object (?) for sending data to the server.
        WWWForm form = new WWWForm();

        // The form is empty which means no fields.

        // AddField(string fieldName (PHP), string value);
        form.AddField("userId", userId);
        form.AddField("timeSpent", timeSpent);

        // Currently, there are 3 fields in the form.

        // Create an object called UnityWebRequest to handle the HTTP stuff.
        // Before creating the UnityWebRequest, using Post function to confrim the UnityWebRequest is configured to transmit form data to a server.
        // Post function will take an url link and a form to send the form data to the server.
        // Using Post function to create a UnityWebRequest will automatically attach a DownloadHandler to it for receiving or checking replies from the server.
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // Send the form data and wait the server reply.
            yield return www.SendWebRequest();

            // Check the result of the request which is transmitting the form data to the server.
            if (www.result != UnityWebRequest.Result.Success)
            {
                // Failed to send data to the server.
                Debug.Log("Form submission failed: " + www.error);
                yield break;
            }

            else
            {
                // Successfully sent the form data to the server.
                if (!string.IsNullOrEmpty(www.downloadHandler.text))
                {
                    // Split the response into separate error messages.
                    string[] responses = www.downloadHandler.text.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                    // Update UI with error messages.
                    foreach (string response in responses)
                    {
                        Debug.Log(response);
                    }
                }
            }
        }
    }

    public void SubmitForm(int userId, int timeSpent)
    {
        StartCoroutine(SendFrom(userId, timeSpent));
    }

    public void UpdateTime()
    {
        if (waveSpawner.isEnd)
        {
            userId = PlayerPrefs.GetInt("userId");

            SubmitForm(userId, timeSpent);
        }
    }
}
