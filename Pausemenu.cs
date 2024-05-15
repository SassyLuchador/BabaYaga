using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pausemenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;


   
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    
    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
  
    void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
    public void LoadMenu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MAinMenu"); 


    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
