using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Structs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlantSeedUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public ItemIcon baseItem;
        public ItemIcon previewItem;
        public Button plantButton;
        public GardenCamera cam;
    
        private List<GameObject> _inventoryUIitems;

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
        
            previewItem.gameObject.SetActive(false);
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            previewItem.gameObject.SetActive(true);
            plantButton.interactable = true;
        
            previewItem.DisplayedItem = item;
            previewItem.UpdateInformation();
        }

        public void OnDisable()
        {
            previewItem.gameObject.SetActive(false);
            plantButton.interactable = false;
        
            previewItem.DisplayedItem = null;
        
            cam.uiOpen = false;
        }

        private void OnEnable()
        {
            cam.uiOpen = true;
        }
    }
}
