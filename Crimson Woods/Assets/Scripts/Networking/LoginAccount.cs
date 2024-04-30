using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    // Coroutine to handle login request
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
                // Clear previous error messages
                emailErrorText.text = "";
                passwordErrorText.text = "";

                // Process the server response
                ProcessServerResponse(www.downloadHandler.text);
            }
            else
            {
                // Display error message if the request failed
                passwordErrorText.text = "Failed to connect to the server.";
            }
        }
    }

    // Process the server response
    void ProcessServerResponse(string responseText)
    {
        try
        {
            // Parse the JSON response
            UserResponse response = JsonUtility.FromJson<UserResponse>(responseText);

            // Check if the response is valid
            if (response != null)
            {
                if (response.status == "success")
                {
                    // Save userId to PlayerPrefs
                    PlayerPrefs.SetInt("userId", response.userId);

                    // Handle successful login
                    SceneManager.LoadScene(1);
                }
                else
                {
                    // Handle errors in the server response
                    DisplayErrorMessages(response);
                }
            }
            else
            {
                passwordErrorText.text = "Invalid server response.";
            }
        }
        catch (System.Exception)
        {
            passwordErrorText.text = "JSON parse error.";
        }
    }

    void DisplayErrorMessages(UserResponse response)
    {
        // Display email error message if present
        if (!string.IsNullOrEmpty(response.message))
        {
            if (response.message == "Invalid email." || response.message == "Email not found.")
            {
                emailErrorText.text = response.message;
            }
            else if (response.message == "Password is required." || response.message == "Incorrect password.")
            {
                passwordErrorText.text = response.message;
            }
            else
            {
                passwordErrorText.text = "Unexpected server response: " + response.message;
            }
        }
    }

    // Define the UserResponse class to match server JSON response
    [System.Serializable]
    public class UserResponse
    {
        public string status;
        public string message;
        public int userId;
        public string emailError;
        public string passwordError;
    }
}