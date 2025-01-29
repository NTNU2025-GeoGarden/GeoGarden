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
        public Button sellButton;   
        
public TextMeshProUGUI coinText;


        private List<ItemIcon> _inventoryUIitems;
        private InventoryItem _selectedItem;

        public void Start()
        {
            LoadData(GameStateManager.CurrentState);
            UpdateCoinText();
            sellButton.interactable = false;

            sellButton.onClick.AddListener(SellSelectedItem);

            OnPlayerSoldItem += ItemSold;
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
                newItem.transform.localPosition = new Vector3((count - (float)Math.Floor(count / 4f)) * 225 + 25, -25 - (float)Math.Floor(count / 4f) * 225, 0);
                newItem.ClickScreenWithItemIcons = this;

                _inventoryUIitems.Add(newItem);
                count++;
            }
        }

        public void SaveData(ref GameState state)
        {
        }
        public void HandleCallbackFromItem(InventoryItem item)
        {
            // Check if item is valid
            if (item == null || item.Item == null)
            {
                Debug.LogError("InventoryItem or its Item property is null.");
                sellButton.interactable = false; // Disable sell button
                _selectedItem = null;
                return;
            }

            // Ensure sellButton reference is valid
            if (sellButton == null)
            {
                Debug.LogError("Sell button is not assigned in the Inspector.");
                return;
            }

            // Enable sell button if the item is valid
            sellButton.interactable = true;
            _selectedItem = item;

            Debug.Log($"Selected item for sale: {item.Item.Name}, Value: {item.Item.Value} coins.");
        }

        private void SellSelectedItem()
        {   
          
            if (_selectedItem != null)
            {
                int sellValue = _selectedItem.Item.Value; // Assuming `Value` represents the value of one seed
                GameStateManager.CurrentState.Coins += sellValue;
                GameStateManager.OnRemoveInventoryItem(_selectedItem.Item.ID);
                UpdateCoinText();
                _selectedItem = null;
                Debug.Log($"Coins gathered: {GameStateManager.CurrentState.Coins}");
                OnPlayerSoldItem?.Invoke();
            }
        }

        private void UpdateCoinText()
        {
            coinText.text = $"{GameStateManager.CurrentState.Coins}";
        }

        private void ItemSold()
        {
            LoadData(GameStateManager.CurrentState);
        }
    }
}

