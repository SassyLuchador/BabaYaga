using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{
    public static event Action OnCollected; 
    public static int total; 
    public float maxDistance = 2f;

    void Start() => total++; 

    void Update()
    {
    
    }

    void OnMouseDown()
    {
        Collect(); 
    }

    void Collect()
    {
       
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= maxDistance)
        {
            OnCollected?.Invoke(); 
            Destroy(gameObject); 
            total--; 
        }
    }
}
