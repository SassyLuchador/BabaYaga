using UnityEngine;

namespace DoorInteractionKit
{
    public class DoorItem : MonoBehaviour
    {
      
        public enum DoorType
        {
            None,
            Door,
            Key,
            Plank,
        }

        [Header("Text Parameters")]
        [SerializeField] private bool showObjectName = true; 
        [SerializeField] private string objectName = "Use";

        [Header("Item Type")]
        [SerializeField] private DoorType doorType = DoorType.None; 
    
        private DoorInteractable doorController;
        private KeyCollectable keyCollectable;
        private PlankInteractable plankInteractable;

        private void Awake()
        {
          
            switch (doorType)
            {
                case DoorType.Door:
                    doorController = GetComponent<DoorInteractable>();
                    break;
                case DoorType.Key:
                    keyCollectable = GetComponent<KeyCollectable>();
                    break;
                case DoorType.Plank:
                    plankInteractable = GetComponent<PlankInteractable>();
                    break;
            }
        }
       
        public void ObjectInteraction()
        {
            
            switch (doorType)
            {
                case DoorType.Door:
                    doorController?.CheckDoor();
                    break;
                case DoorType.Key:
                    keyCollectable?.KeyPickup();
                    break;
                case DoorType.Plank:
                    plankInteractable?.RemovePlank();
                    break;
            }
        }
       
        public void ShowObjectName(bool isActive)
        {
            
            if (showObjectName)
            {
                DoorUIManager.instance.ShowName(objectName, isActive);
            }
        }
    }
}

