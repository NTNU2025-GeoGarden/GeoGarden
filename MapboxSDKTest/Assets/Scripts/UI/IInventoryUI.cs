using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Structs;
using UnityEngine;

namespace UI
{
    public class IInventoryUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public ItemIcon baseItem;
        private List<GameObject> _inventoryUIitems;
        public GardenCamera cam;

        public void Start()
        {
            LoadData(GameStateManager.CurrentState);
        }
    
        public void LoadData(GameState state)
        {
            if (_inventoryUIitems != null)
            {
                foreach (GameObject obj in _inventoryUIitems)
                {
                    Destroy(obj);
                }
        
                _inventoryUIitems.Clear();
            }
            else
            {
                _inventoryUIitems = new List<GameObject>();
            }

            int count = 0;
            foreach ((int id, int amount) in state.Inventory)
            {
                ItemIcon newItem = Instantiate(baseItem.gameObject, transform).GetComponent<ItemIcon>();
                newItem.DisplayedItem = new InventoryItem(id, amount);
                newItem.transform.localPosition = new Vector3((count - (float)Math.Floor(count / 4f)) * 225 + 25, -25 - (float)Math.Floor(count / 4f) * 225, 0);
                newItem.ClickScreenWithItemIcons = this;

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
        }
    
        private void OnDisable()
        {
            cam.uiOpen = false;
        }
    }
}
