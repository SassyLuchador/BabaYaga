using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoorInteractionKit
{
    public class DoorInteractable : MonoBehaviour
    {
       
        public enum InteractableType { Door, Drawer }
        [SerializeField] private InteractableType interactableType = InteractableType.Door;

       
        [SerializeField] private Transform doorTransform = null;

    
        [SerializeField] private bool doorOpenRight = true; 
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 3f;
        [SerializeField] private float closeSpeed = 3f;

       
        public enum DrawerDirection { Forward, Backward, Left, Right, Up, Down }
        [SerializeField] private DrawerDirection drawerOpenDirection = DrawerDirection.Forward;
        [SerializeField] private float drawerOpenSpeed = 3f;
        [SerializeField] private float drawerCloseSpeed = 3f;
        [SerializeField] private float slideDistance = 0.5f;

       
        [SerializeField] private int plankCount = 0; 
        [SerializeField] private string doorPlankedText = "Blocked by Planks";

        
        [SerializeField] private bool isLocked = false;
        [SerializeField] private string lockedDoorText = "Locked";
        [SerializeField] private Sound doorLockedSound = null;
        [SerializeField] private Key keyScriptable = null;
        [SerializeField] private bool removeKeyAfterUse = false;
        [SerializeField] private Sound doorUnlockSound = null;

       
        [SerializeField] private Sound doorOpenSound = null;
        [SerializeField] private float openDelay = 0;

        
        [SerializeField] private Sound doorCloseSound = null;

        [SerializeField] private float closeDelay = 0.8f;

        
        [SerializeField] private UnityEvent onDrawerOpen;

        private bool hasSpawnedItems = false; 
        private Vector3 closedPosition;
        private Vector3 openPosition;
        private bool isUnlocking = false;
        private bool isOpening = false;
        private bool isClosing = false;
        private Quaternion closedRotation;
        private Quaternion openRotation;

        private bool doorOpen = false;

        private void Start()
        {
            if (doorTransform == null)
            {
                Debug.LogWarning("Door Transform not set in the DoorController");
                return;
            }
            
            switch (interactableType)
            {
                case InteractableType.Door:
                    closedRotation = doorTransform.rotation;
                    openRotation = Quaternion.Euler(doorTransform.eulerAngles + new Vector3(0, doorOpenRight ? -openAngle : openAngle, 0));
                    break;

                case InteractableType.Drawer:
                    closedPosition = doorTransform.position;
                    openPosition = closedPosition + GetDirection(drawerOpenDirection) * slideDistance;
                    break;
            }
        }
        
        private Vector3 GetDirection(DrawerDirection dir)
        {
            switch (dir)
            {
                case DrawerDirection.Forward: return doorTransform.forward;
                case DrawerDirection.Backward: return -doorTransform.forward;
                case DrawerDirection.Left: return -doorTransform.right;
                case DrawerDirection.Right: return doorTransform.right;
                case DrawerDirection.Up: return doorTransform.up;
                case DrawerDirection.Down: return -doorTransform.up;
                default: return Vector3.forward;
            }
        }

        private void Update()
        {
            if (doorTransform == null)
            {
                return;
            }
            switch (interactableType)
            {
                case InteractableType.Door:
                    if (isOpening)
                    {
                        doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, openRotation, Time.deltaTime * openSpeed);
                        if (Quaternion.Angle(doorTransform.rotation, openRotation) < 0.1f)
                        {
                            doorTransform.rotation = openRotation;
                            isOpening = false;
                        }
                    }
                    else if (isClosing)
                    {
                        doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, closedRotation, Time.deltaTime * closeSpeed);
                        if (Quaternion.Angle(doorTransform.rotation, closedRotation) < 0.1f)
                        {
                            doorTransform.rotation = closedRotation;
                            isClosing = false;
                        }
                    }
                    break;

                case InteractableType.Drawer:
                    if (isOpening && interactableType == InteractableType.Drawer)
                    {
                        doorTransform.position = Vector3.MoveTowards(doorTransform.position, openPosition, Time.deltaTime * drawerOpenSpeed);
                        if (Vector3.Distance(doorTransform.position, openPosition) < 0.01f)
                        {
                            doorTransform.position = openPosition;
                            isOpening = false;
                        }
                    }
                    else if (isClosing && interactableType == InteractableType.Drawer)
                    {
                        doorTransform.position = Vector3.MoveTowards(doorTransform.position, closedPosition, Time.deltaTime * drawerCloseSpeed);
                        if (Vector3.Distance(doorTransform.position, closedPosition) < 0.01f)
                        {
                            doorTransform.position = closedPosition;
                            isClosing = false;
                        }
                    }
                    break;
            }
        }
        public void CheckDoor()
        {
            if (plankCount > 0)
            {
                DoorAudioManager.instance.Play(doorLockedSound);
                ShowLockedPrompt(doorPlankedText);
                return;
            }

            if (isLocked)
            {
                if (DoorInventory.instance._keyList.Contains(keyScriptable))
                {
                    if (removeKeyAfterUse)
                    {
                        DoorInventory.instance.RemoveKey(keyScriptable);
                        ShowLockedPrompt(keyScriptable._KeyName + " Removed");
                    }

                    DoorAudioManager.instance.Play(doorUnlockSound);
                    StartCoroutine(UnlockDoor());
                    return;
                }
                else
                {
                    DoorAudioManager.instance.Play(doorLockedSound);
                    ShowLockedPrompt(lockedDoorText);
                    return;
                }
            }

            if (isOpening || isClosing || isUnlocking)
                return;
            if (doorOpen)
            {
                isClosing = true;
                DoorAudioManager.instance.Play(doorCloseSound, closeDelay);
            }
            else
            {
                isOpening = true;
                DoorAudioManager.instance.Play(doorOpenSound, openDelay);

                if (!hasSpawnedItems)
                {
                    onDrawerOpen?.Invoke();
                    hasSpawnedItems = true;
                }
            }

            doorOpen = !doorOpen;
        }
        public void RemovePlank()
        {
            if (plankCount > 0)
            {
                plankCount--;
                if (plankCount == 0)
                {
                }
            }
        }
        private IEnumerator UnlockDoor()
        {
            isUnlocking = true;

            yield return new WaitForSeconds(doorUnlockSound.clip.length);

            isLocked = false;
            isUnlocking = false;
        }
        public void ShowLockedPrompt(string promptText)
        {
            DoorUIManager.instance.ShowDoorText(promptText);
        }
    }
}