using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace DoorInteractionKit
{
    public class DoorUIManager : MonoBehaviour
    {
        [Header("Object Name UI")]
        [SerializeField] private GameObject interactTextBG;
        [SerializeField] private TMP_Text interactTextUI;

        [Header("Locked Door UI")]
        [SerializeField] private TMP_Text lockedDoorTextUI;
        [SerializeField] private GameObject lockedDoorUIBG;

        [Header("Locked Door - Text Customisation")]
        [SerializeField] private int TextSize = 36;
        [SerializeField] private TMP_FontAsset FontType = null;
        [SerializeField] private FontStyles FontStyle = FontStyles.Bold;
        [SerializeField] private Color FontColor = Color.white;

        [Header("Inventory Fields")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject inventoryPanel;

        [Header("Timer")]
        [SerializeField] private float onScreenTimer = 2f;
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private float fadeOutDuration = 1f;

        [Header("Crosshair")]
        [SerializeField] private Image crosshairUI = null;

        private Dictionary<Key, GameObject> inventorySlots = new Dictionary<Key, GameObject>();
        private CanvasGroup lockedDoorUICanvasGroup;
        private CanvasGroup interactNameUICanvasGroup;
        private bool startTimer;
        private float timer;

        public static DoorUIManager instance;

        void Awake()
        {
            
                
            
             instance = this; DontDestroyOnLoad(gameObject); 

            lockedDoorUICanvasGroup = lockedDoorUIBG.GetComponent<CanvasGroup>();
            interactNameUICanvasGroup = interactTextBG.GetComponent<CanvasGroup>();

            lockedDoorUICanvasGroup.alpha = 0;
            interactNameUICanvasGroup.alpha = 0;

            SetTextSettings();
        }

        void SetTextSettings()
        {
            lockedDoorTextUI.fontSize = TextSize;
            lockedDoorTextUI.font = FontType;
            lockedDoorTextUI.fontStyle = FontStyle;
            lockedDoorTextUI.color = FontColor;
        }

        private void Update()
        {
            if (startTimer)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    ClearDoorText();
                    startTimer = false;
                }
            }
        }

        public void ShowDoorText(string lockedDoorString)
        {
            lockedDoorTextUI.text = lockedDoorString;
            StartCoroutine(FadeUI(true, fadeInDuration));
            timer = onScreenTimer;
            startTimer = true;
        }

        public void ShowName(string objectName, bool show)
        {
            if (show)
            {
                interactNameUICanvasGroup.alpha = 1;
                interactTextUI.text = objectName;
            }
            else
            {
                interactNameUICanvasGroup.alpha = 0;
                interactTextUI.text = "";
            }
        }

        void ClearDoorText()
        {
            StartCoroutine(FadeUI(false, fadeOutDuration));
        }

        public IEnumerator FadeUI(bool fadeIn, float duration)
        {
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = 1f - startAlpha;
            float elapsedTime = 0f;

            lockedDoorUICanvasGroup.alpha = startAlpha;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / duration;
                lockedDoorUICanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                yield return null;
            }
            lockedDoorUICanvasGroup.alpha = endAlpha;
        }

        public void AddInventorySlot(Key key)
        {
          
            GameObject newSlot = Instantiate(slotPrefab, inventoryPanel.transform);
            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();

           
            slotScript.SetSlot(key._KeySprite, key._KeyName);

            
            inventorySlots[key] = newSlot;
        }

        public void RemoveInventorySlot(Key key)
        {
            if (inventorySlots.TryGetValue(key, out GameObject slot))
            {
              
                Destroy(slot);

               
                inventorySlots.Remove(key);
            }
        }

        public void OpenInventory()
        {
            if (inventoryPanel != null)
            {
              
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }
        }

        public void HighlightCrosshair(bool on)
        {
            crosshairUI.color = on ? Color.red : Color.white;
        }
    }
}