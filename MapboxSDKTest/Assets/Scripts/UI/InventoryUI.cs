using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Structs;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public ItemIcon baseItem;
        private List<ItemIcon> _inventoryUIitems;
        public GardenCamera cam;

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
            Debug.Log("inventory save");
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            Debug.Log("Got called");
        }

        private void OnEnable()
        {
            cam.uiOpen = true;
            LoadData(GameStateManager.CurrentState);
        }
    
        private void OnDisable()
        {
            cam.uiOpen = false;
        }
    }
}
