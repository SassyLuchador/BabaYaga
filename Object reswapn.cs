using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectreswapn : MonoBehaviour
{
    public Transform oB;
    public Transform[] spawnPoints;



    void Start()
    {
       
        int indexNumber = Random.Range(0, spawnPoints.Length);
        
        oB.position = spawnPoints[indexNumber].position;

    }

   
    void Update()
    {
        
    }
}
