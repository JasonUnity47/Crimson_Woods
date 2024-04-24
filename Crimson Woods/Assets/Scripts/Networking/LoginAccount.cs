using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginAccount : MonoBehaviour
{
    // Input Fields
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;

    // Message Texts
    [Header("Message Texts")]
    [SerializeField] private TMP_Text emailErrorText;
    [SerializeField] private TMP_Text passwordErrorText;

    // URL
    [Header("URL")]
    [SerializeField] private string url = "http://localhost/cwgd/login.php";

    // Method to handle the login button click event
    public void LoginButton()
    {
        // Collect input data from the input fields
        string email = emailField.text;
        string password = passwordField.text;

        // Validate inputs
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            // Start the login process
            StartCoroutine(Login(email, password));
        }
        else
        {
            // Display error messages if inputs are empty
            if (string.IsNullOrEmpty(email))
            {
                emailErrorText.text = "Please enter your email.";
            }
            if (string.IsNullOrEmpty(password))
            {
                passwordErrorText.text = "Please enter your password.";
            }
        }
    }

    // Coroutine to handle the login request
    IEnumerator Login(string email, string password)
    {
        // Create a form to send the data
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // Create a UnityWebRequest to send the form data via POST
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // Send the request and wait for the response
            yield return www.SendWebRequest();

            // Check the result of the request
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Process the server response
                string responseText = www.downloadHandler.text;

                // Clear any previous error messages
                emailErrorText.text = "";
                passwordErrorText.text = "";

                // Check the response for success or error messages
                if (responseText.Contains("Login successful"))
                {
                    // Handle successful login
                    Debug.Log("Login successful!");
                    // You can navigate to another scene or update the UI here
                }
                else
                {
                    // Split the response to separate error messages
                    string[] responseLines = responseText.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                    // Process each line for errors
                    foreach (string line in responseLines)
                    {
                        if (line.Contains("Invalid email") || line.Contains("Email not found"))
                        {
                            // Display email error
                            emailErrorText.text = line;
                        }
                        else if (line.Contains("Incorrect password"))
                        {
                            // Display password error
                            passwordErrorText.text = line;
                        }
                    }
                }
            }
            else
            {
                // Display an error message if the request failed
                Debug.Log("Failed to connect to the server: " + www.error);
            }
        }
    }
}