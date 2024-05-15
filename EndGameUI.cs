using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Database;



public class EndGameUI : MonoBehaviour
{
    // References to UI elements
    public GameObject endGameUI;
    public TMP_InputField nameInputField;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public TMP_Text errorMessageText;
    public float errorMessageDisplayTime = 3f;

    // Reference to Firebase Database
    public DatabaseReference databaseReference;

    void Start()
    {
        // Deactivate the end game UI at the start
        endGameUI.SetActive(false);
        // Get the root reference of the Firebase Database
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Method to display the end game UI
    public void DisplayEndGameUI()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Activate the end game UI
        endGameUI.SetActive(true);
        // Update the score text with the collected count and initial total
        scoreText.text = FindObjectOfType<CollectibleCount>().GetCount() + "/" + FindObjectOfType<CollectibleCount>().GetInitialTotal();
        // Update the time text with the current time
        timeText.text = FindObjectOfType<Timer>().GetTimeText();
    }
    // Method to submit player name
    public void SubmitName()
    {
        try
        {
            // Get player name from the input field
            string playerName = nameInputField.text;

            // Check if player name meets the length requirement
            if (playerName.Length < 3 || playerName.Length > 8)
            {
                throw new System.Exception("Player name must be between 3 and 8 characters long.");
            }

            // Get the current score and time from the UI elements
            string score = scoreText.text;
            string time = timeText.text;

            // Check if player name is not empty
            if (!string.IsNullOrEmpty(playerName))
            {
                // Save data to Firebase
                SaveDataToFirebase(playerName, score, time);

                // Set time scale to normal and load the main menu scene
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            // Display error message
            ShowErrorMessage(e.Message);
        }
    }

    private void ShowErrorMessage(string message)
    {
        // Display the error message text
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);

        // Hide the error message after a certain duration
        Invoke("HideErrorMessage", errorMessageDisplayTime);
    }

    private void HideErrorMessage()
    {
        // Hide the error message text
        errorMessageText.gameObject.SetActive(false);
    }

    // Method to save player data to Firebase
    private void SaveDataToFirebase(string playerName, string score, string time)
    {
        // Generate a unique key for the submission
        string submissionKey = databaseReference.Child("playerSubmissions").Push().Key;
        // Create a dictionary to store player data
        Dictionary<string, object> playerData = new Dictionary<string, object>();
        playerData["playerName"] = playerName;
        playerData["score"] = score;
        playerData["time"] = time;
        // Set the player data at the submission key location in Firebase
        databaseReference.Child("playerSubmissions").Child(submissionKey).SetValueAsync(playerData);
    }
}
