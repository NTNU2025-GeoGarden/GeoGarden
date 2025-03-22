using System;
using System.Collections.Generic;
using Stateful;
using Stateful.Managers;
using Structs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class ShopUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public delegate void PlayerSoldItem();
        public static PlayerSoldItem OnPlayerSoldItem;

        public ItemIcon baseItem;
        public ItemIcon previewItem;
        public Button sellButton;

        public TextMeshProUGUI priceText;
        public RectTransform scrollView;

        private List<ItemIcon> _inventoryUIitems;
        private InventoryItem _selectedItem;

        public Image coinIcon;

        public void Start()
        {
            LoadData(GameStateManager.CurrentState);
            sellButton.interactable = false;

            sellButton.onClick.AddListener(SellSelectedItem);
            OnPlayerSoldItem = ItemSold;

            previewItem.gameObject.SetActive(false);
        }

        public void LoadData(GameState state)
        {
            // Clear existing UI items
            if (_inventoryUIitems != null)
            {
                foreach (ItemIcon obj in _inventoryUIitems)
                {
                    Destroy(obj.gameObject);
                }
                _inventoryUIitems.Clear();
            }
            else
            {
                _inventoryUIitems = new List<ItemIcon>();
            }

            // Display inventory
            int count = 0;
            foreach (SerializableInventoryEntry entry in state.Inventory)
            {
                ItemIcon newItem = Instantiate(baseItem.gameObject, transform).GetComponent<ItemIcon>();
                newItem.DisplayedItem = new InventoryItem(entry.Id, entry.Amount);
                newItem.transform.localPosition = new Vector3(
                    count % 4 * 225 + 50,
                    -25 - (float)Math.Floor(count / 4f) * 225, 0
                );
                newItem.ClickScreenWithItemIcons = this;
                _inventoryUIitems.Add(newItem);
                count++;
            }

            scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)Math.Floor(count / 4f) * 225);
        }

        public void SaveData(ref GameState state) { }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            // Check if item is valid
            if (item == null || item.Item == null)
            {
                Debug.LogError("InventoryItem or its Item property is null.");
                sellButton.interactable = false; // Disable sell button
                _selectedItem = null;
                previewItem.gameObject.SetActive(false);
                return;
            }

            // Ensure sellButton reference is valid
            if (sellButton == null)
            {
                Debug.LogError("Sell button is not assigned in the Inspector.");
                return;
            }

            // Enable sell button and update preview item
            sellButton.interactable = true;
            _selectedItem = item;
            previewItem.gameObject.SetActive(true);
            previewItem.DisplayedItem = item;
            previewItem.DisplayedItem.Amount = 1;
            priceText.text = $"{item.Item.Value}";
            coinIcon.gameObject.SetActive(true);
            previewItem.UpdateInformation();
        }

        private void SellSelectedItem()
        {
            if (_selectedItem != null)
            {
                int sellValue = _selectedItem.Item.Value;
                GameStateManager.CurrentState.Coins = Math.Min(GameStateManager.CurrentState.Coins + sellValue, GameStateManager.CurrentState.CoinCap);
                FirebaseManager.TelemetryRecordCoinsGenerated(sellValue);
                GameStateManager.RemoveInventoryItem(_selectedItem.Item.ID);
                _selectedItem = null;
                previewItem.gameObject.SetActive(false);
                sellButton.interactable = false;
                Debug.Log($"Coins gathered: {GameStateManager.CurrentState.Coins}");
                coinIcon.gameObject.SetActive(false);
                priceText.text = "";
                OnPlayerSoldItem?.Invoke();
            }
        }



        private void ItemSold()
        {
            LoadData(GameStateManager.CurrentState);
        }

        private void OnDisable()
        {
            previewItem.gameObject.SetActive(false);
            sellButton.interactable = false;
            _selectedItem = null;
        }
    }
}