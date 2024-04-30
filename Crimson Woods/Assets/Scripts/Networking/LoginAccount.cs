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
                Debug.Log("Failed to connect to the server: " + www.error);
            }
        }
    }

    // Process the server response
    void ProcessServerResponse(string responseText)
    {
        // Log the server response for debugging
        Debug.Log("Server Response: " + responseText);

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
                    Debug.Log($"Saved userId in PlayerPrefs: {response.userId}");
                    // Handle successful login
                    Debug.Log("Login successful!");
                    SceneManager.LoadScene(1);
                }
                else
                {
                    // Handle server response with errors
                    DisplayErrorMessages(response);
                }
            }
            else
            {
                Debug.Log("Invalid JSON response.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("JSON parse error: " + ex.Message);
        }
    }

    // Display error messages from server response
    void DisplayErrorMessages(UserResponse response)
    {
        if (!string.IsNullOrEmpty(response.emailError))
        {
            emailErrorText.text = response.emailError;
        }
        if (!string.IsNullOrEmpty(response.passwordError))
        {
            passwordErrorText.text = response.passwordError;
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