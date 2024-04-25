using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public TMP_Text userIdText; // Reference to a UI Text component to display the userId

    private void Start()
    {
        // Check if the userId is already stored
        if (PlayerPrefs.HasKey("userId"))
        {
            // If the userId is stored, display it
            userIdText.text = "User ID: " + PlayerPrefs.GetInt("userId");
        }
        else
        {
            // If the userId is not stored, display a message
            userIdText.text = "User ID not found.";
        }
    }

    // This method is called when the button attached to this script is pressed
    public void OnButtonClick()
    {
        // Check if the userId is stored and display it
        if (PlayerPrefs.HasKey("userId"))
        {
            Debug.Log("User ID: " + PlayerPrefs.GetInt("userId"));
        }
        else
        {
            Debug.Log("User ID not found.");
        }
    }
}