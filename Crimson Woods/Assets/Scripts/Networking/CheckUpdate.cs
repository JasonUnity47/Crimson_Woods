using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class CheckUpdate : MonoBehaviour
{
    // Declaration
    // Input Field
    [Header("Input Field")]
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;

    // Message Text
    [Header("Message Text")]
    [SerializeField] private TMP_Text errorSystem;

    // Store Variable
    [Header("Store Variable")]
    public string userId;
    public string username;
    public string email;
    public string password;

    // Check Variable
    [Header("Check")]
    public bool isExisted = false;

    // URL
    [Header("URL")]
    [SerializeField] private string url = "http://localhost/cwgd/selection.php";

    // Object Reference
    [Header("Object Reference")]
    public GameObject updatePanel;

    private void Update()
    {
        if (emailField.text == "" && passwordField.text == "")
        {
            errorSystem.text = "";
        }
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
                        // A variable to Filter the value messages and error messages.
                        bool playerInfo = response.Contains("playerName") || response.Contains("playerEmail") || response.Contains("playerPassword") || response.Contains("playerId");

                        // If the text has playerName which means this text is a value message.
                        // Get value from PHP files.
                        if (response.Contains("playerName"))
                        {
                            string[] names = response.Split(':', System.StringSplitOptions.RemoveEmptyEntries);

                            foreach (string n in names)
                            {
                                username = n;
                            }
                        }

                        // If the text has playerId which means this text is a value message.
                        // Get value from PHP files.
                        if (response.Contains("playerId"))
                        {
                            string[] ids = response.Split(':', System.StringSplitOptions.RemoveEmptyEntries);

                            foreach (string i in ids)
                            {
                                userId = i;
                            }
                        }

                        // General error messages.
                        if (!playerInfo)
                        {
                            switch (response)
                            {
                                case "true":
                                    {
                                        isExisted = true;
                                        ShowPanel();
                                        break;
                                    }

                                case "false":
                                    {
                                        isExisted = false;
                                        break;
                                    }

                                default:
                                    {
                                        // Handle system errors.
                                        Debug.Log(response);
                                        errorSystem.text = response;
                                        break;
                                    }
                            }
                        }
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

    public void CheckButton()
    {
        // If input fields are exist.
        if (emailField != null && passwordField != null)
        {
            // If the user click the button then store the input texts.
            email = emailField.text;
            password = passwordField.text;

            // If the user has input something.
            if (email != "" && password != "")
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

    public void ClearText()
    {
        emailField.text = "";
        passwordField.text = "";
        errorSystem.text = "";

        username = "";
        email = "";
        password = "";

        return;
    }

    // Show Panel
    public void ShowPanel()
    {
        if (!updatePanel.activeSelf)
        {
            updatePanel.SetActive(true);
        }

        return;
    }
}
