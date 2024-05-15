using UnityEngine;
using TMPro;

public class CollectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int count; 
    int initialTotal; 

    void Awake()
    {   
        
        text = GetComponent<TMPro.TMP_Text>();
        
        Collectible.OnCollected += OnCollectibleCollected;
       
        InitializeCount();
    }

    
    void InitializeCount()
    {
        
        Collectible[] collectibles = FindObjectsOfType<Collectible>();
        
        initialTotal = collectibles.Length;
       
        UpdateCount();
    }

    
    void OnDisable() => Collectible.OnCollected -= OnCollectibleCollected;

   
    void OnCollectibleCollected()
    {
        
        count++;
      
        UpdateCount();
    }

   
    public int GetCount()
    {
        return count;
    }

   
    public int GetInitialTotal()
    {
        return initialTotal;
    }

   
    void UpdateCount()
    {
        int remainingCollectibles = 0;
       
        Collectible[] collectibles = FindObjectsOfType<Collectible>();
        
        foreach (Collectible collectible in collectibles)
        {
            
            if (collectible.isActiveAndEnabled)
            {
                remainingCollectibles++;
            }
        }
        
        text.text = $"{count}/{initialTotal}";
    }
}
