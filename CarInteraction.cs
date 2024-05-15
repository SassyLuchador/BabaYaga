using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarInteraction : MonoBehaviour
{
    bool gameEnded = false;  
    bool Vanclicked = false; 

    void Update()
    {
       
        CollectibleCount Collectible = FindObjectOfType<CollectibleCount>();
       
        if (Collectible.GetCount() == Collectible.GetInitialTotal()) {
            gameEnded = true;
        }
       
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.transform.name == "Van")
                {
                     Vanclicked = true;
                }
                else
                {
                    Vanclicked = false;
                }
            }
        }
      
        if (Vanclicked && gameEnded)
        {
            EndGame();
        }

    }


   
    void EndGame()
    {
        Time.timeScale = 0f; 
        Timer timer = FindObjectOfType<Timer>();
       
        if (timer == null)
        {
            Debug.LogError("Timer not found!");
            return;
        }

        timer.StopTimer();

        EndGameUI endGameUI = FindObjectOfType<EndGameUI>(true);
        if (endGameUI == null)
        {
            Debug.LogError("EndGameUI not found!"); 
            return;
        }

        endGameUI.DisplayEndGameUI(); 

        gameEnded = true;
    }
}