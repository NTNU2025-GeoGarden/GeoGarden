using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public ItemIcon baseItem;
        private List<ItemIcon> _inventoryUIitems;
        public GardenCamera cam;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI itemTypeText;
        public TextMeshProUGUI itemRarityText;
        public void Start()
        {
            LoadData(GameStateManager.CurrentState);
        }
    
        public void LoadData(GameState state)
        {
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

               
               itemTypeText.text = item.Item.Type.ToString();
               descriptionText.text = item.Item.Description;
               itemRarityText.text = item.Item.Rarity.ToString();
           
        }

        private void OnEnable()
        {
            LoadData(GameStateManager.CurrentState);
        }
    }
}
