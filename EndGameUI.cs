using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Database;



public class EndGameUI : MonoBehaviour
{
    
    public GameObject endGameUI;
    public TMP_InputField nameInputField;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public TMP_Text errorMessageText;
    public float errorMessageDisplayTime = 3f;

    
    public DatabaseReference databaseReference;

    void Start()
    {
        
        endGameUI.SetActive(false); 
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    
    public void DisplayEndGameUI()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endGameUI.SetActive(true);
        
        scoreText.text = FindObjectOfType<CollectibleCount>().GetCount() + "/" + FindObjectOfType<CollectibleCount>().GetInitialTotal();
        timeText.text = FindObjectOfType<Timer>().GetTimeText();
    }
    
    public void SubmitName()
    {
        try
        {
           
            string playerName = nameInputField.text;

            
            if (playerName.Length < 3 || playerName.Length > 8)
            {
                throw new System.Exception("Player name must be between 3 and 8 characters long.");
            }

            
            string score = scoreText.text;
            string time = timeText.text;

           
            if (!string.IsNullOrEmpty(playerName))
            {
                
                SaveDataToFirebase(playerName, score, time);

                
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
            ShowErrorMessage(e.Message);
        }
    }

    private void ShowErrorMessage(string message)
    {
      
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);

        
        Invoke("HideErrorMessage", errorMessageDisplayTime);
    }

    private void HideErrorMessage()
    {
        errorMessageText.gameObject.SetActive(false);
    }

    
    private void SaveDataToFirebase(string playerName, string score, string time)
    {

        string submissionKey = databaseReference.Child("playerSubmissions").Push().Key;
        Dictionary<string, object> playerData = new Dictionary<string, object>();
        playerData["playerName"] = playerName;
        playerData["score"] = score;
        playerData["time"] = time;
        databaseReference.Child("playerSubmissions").Child(submissionKey).SetValueAsync(playerData);
    }
}
