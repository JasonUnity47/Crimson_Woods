using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UpdateAccount : MonoBehaviour
{
    // Declaration
    // Input Field
    [Header("Input Field")]
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;

    // Message Text
    [Header("Message Text")]
    [SerializeField] private TMP_Text errorSystem;
    [SerializeField] private TMP_Text errorName;
    [SerializeField] private TMP_Text errorEmail;

    // Store Variable
    [Header("Store Variable")]
    private int userId;
    private string username;
    private string email;
    private string password;

    // Script Reference
    private CheckUpdate checkUpdate;

    // URL
    [Header("URL")]
    [SerializeField] private string url = "https://jasonunity47.000webhostapp.com/update.php";

    private void Start()
    {
        checkUpdate = GetComponent<CheckUpdate>();
    }

    private void Update()
    {
        // If no texts then clear the selected messages.
        if (nameField.text == "")
        {
            errorName.text = "";
        }

        if (emailField.text == "")
        {
            errorEmail.text = "";
        }
    }

    IEnumerator SendFrom(string username, string email, string password)
    {
        // Create a form object (?) for sending data to the server.
        WWWForm form = new WWWForm();

        // The form is empty which means no fields.

        // AddField(string fieldName (PHP), string value);
        form.AddField("userId", userId);
        form.AddField("name", username);
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
                    // Split the response into separate error messages.
                    string[] responses = www.downloadHandler.text.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                    // Update UI with error messages.
                    foreach (string response in responses)
                    {
                        if (!response.Contains("name") && !response.Contains("email"))
                        {
                            // Update UI with system error message.
                            errorSystem.text = response;
                        }

                        if (response.Contains("name"))
                        {
                            // Update UI with name error message.
                            errorName.text = response;
                        }

                        if (response.Contains("email"))
                        {
                            // Update UI with email error message.
                            errorEmail.text = response;
                        }
                    }
                }
            }
        }
    }

    public void SubmitForm(string username, string email, string password)
    {
        StartCoroutine(SendFrom(username, email, password));
    }

    public void GetValue()
    {
        if (checkUpdate.isExisted)
        {
            userId = int.Parse(checkUpdate.userId);
            username = checkUpdate.username;
            email = checkUpdate.email;
            password = checkUpdate.password;

            nameField.text = username;
            emailField.text = email;
            passwordField.text = password;
        }

        return;
    }

    public void SubmitButton()
    {
        // If input fields are exist.
        if (nameField != null && emailField != null && passwordField != null)
        {
            // If the user click the button then store the input texts.
            username = nameField.text;
            email = emailField.text;
            password = passwordField.text;

            // If the user has input something.
            if (username != "" && email != "" && password != "")
            {
                SubmitForm(username, email, password);
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
        nameField.text = "";
        emailField.text = "";
        passwordField.text = "";
        errorName.text = "";
        errorEmail.text = "";
        errorSystem.text = "";

        userId = 0;
        username = "";
        email = "";
        password = "";

        return;
    }
}
