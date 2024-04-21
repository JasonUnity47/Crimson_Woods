using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DeleteAccount : MonoBehaviour
{
    // Declaration
    // Message Text
    [Header("Message Text")]
    [SerializeField] private TMP_Text errorSystem;

    // Store Variable
    [Header("Store Variable")]
    private string email;
    private string password;

    // Script Reference
    private CheckUpdate checkAccount;

    // URL
    [Header("URL")]
    [SerializeField] private string url = "http://localhost/codeweb/deletion.php";

    private void Start()
    {
        checkAccount = GetComponent<CheckUpdate>();
    }

    IEnumerator SendFrom(string email, string password)
    {
        // Create a form object (?) for sending data to the server.
        WWWForm form = new WWWForm();

        // The form is empty which means no fields.

        // AddField(string fieldName (PHP), string value);
        form.AddField("email", email);
        form.AddField("password", password);

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
                    string[] responses = www.downloadHandler.text.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (string response in responses)
                    {
                        Debug.Log(response);
                        errorSystem.text = response;
                    }
                }

                else
                {
                    // If response is empty, reset error message.
                    errorSystem.text = "";
                }
            }
        }
    }

    public void SubmitForm(string email, string password)
    {
        StartCoroutine(SendFrom(email, password));
    }

    public void DeleteButton()
    {
        // If the user click the button then store the input texts.
        email = checkAccount.email;
        password = checkAccount.password;

        // If the user has input something and already checked.
        if (email != "" && password != "" && checkAccount.isExisted)
        {
            SubmitForm(email, password);
        }

        else
        {
            string errorInput = "No inputs detected / Invalid inputs.";
            Debug.Log(errorInput);
            errorSystem.text = errorInput;
        }
    }
}
