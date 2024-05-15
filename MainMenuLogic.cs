using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using System;


using TMPro;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject Leaderboard;
    public GameObject Loading;
  
    
    DatabaseReference databaseReference;
    List<string> playerNames = new List<string>();
    List<string> times = new List<string>();

    public AudioSource buttonSound;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        MainMenu = GameObject.Find("Main Menu Canvas");
        Options = GameObject.Find("Options Canvas");
        Leaderboard = GameObject.Find("Leadrboard Canvas");
        Loading = GameObject.Find("Loadin Canvas");

        MainMenu.GetComponent<Canvas>().enabled = true;
        Options.GetComponent<Canvas>().enabled = false;
        Leaderboard.GetComponent<Canvas>().enabled = false;
        Loading.GetComponent<Canvas>().enabled = false;

        
      
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void StartButton()
    {
        Loading.GetComponent<Canvas>().enabled = true;
        MainMenu.GetComponent<Canvas>().enabled = false;
        buttonSound.Play();
        SceneManager.LoadScene("SampleScene");
    }

    public void OptionButton()
    {
        buttonSound.Play();
        MainMenu.GetComponent<Canvas>().enabled = false;
        Options.GetComponent<Canvas>().enabled = true;
    }

    public void LeaderboardButton()
    {
        buttonSound.Play();
        MainMenu.GetComponent<Canvas>().enabled = false;
        Leaderboard.GetComponent<Canvas>().enabled = true;

       LoadLeaderboard();
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        MainMenu.GetComponent<Canvas>().enabled = true;
        Options.GetComponent<Canvas>().enabled = false;
        Leaderboard.GetComponent<Canvas>().enabled = false;
    }

    void LoadLeaderboard()
    {

        databaseReference.Child("playerSubmissions").OrderByChild("time").LimitToFirst(10).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to fetch leaderboard data: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            playerNames.Clear();
            times.Clear();

            foreach (DataSnapshot entrySnapshot in snapshot.Children)
            {
                string playerName = entrySnapshot.Child("playerName").Value.ToString();
                string time = entrySnapshot.Child("time").Value.ToString();

                playerNames.Add(playerName);
                times.Add(time);
            }

            UpdateLeaderboardUI();
        });
    }

    void UpdateLeaderboardUI()
    {
        GameObject leaderboardCanvas = GameObject.Find("Leadrboard Canvas");
        Transform leaderboard = leaderboardCanvas.transform.Find("Leaderboard");

        for (int i = 0; i < playerNames.Count; i++)
        {
            string playerName = playerNames[i];
            string time = times[i];

            Transform position = leaderboard.Find("Pos").Find((i + 1).ToString());

            if (position != null)
            {
                Transform nameText = position.Find("Name");
                Transform timeText = position.Find("Time"); 
                if (nameText != null && timeText != null)
                {
                    nameText.GetComponent<TMP_Text>().text = playerName;
                    timeText.GetComponent<TMP_Text>().text = time;
                }
            }
        }


    }

}

