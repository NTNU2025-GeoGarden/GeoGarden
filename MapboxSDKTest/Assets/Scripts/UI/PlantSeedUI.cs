using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Stateful.Managers;
using Structs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlantSeedUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public delegate void PlayerPlantedSeed();
        public static PlayerPlantedSeed OnPlayerPlantedSeed;
        
        public ItemIcon baseItem;
        public ItemIcon previewItem;
        public Button plantButton;
        public GardenCamera cam;
    
        private List<ItemIcon> _inventoryUIitems;

        public void Start()
        {
            LoadData(GameStateManager.CurrentState);

            OnPlayerPlantedSeed += SeedPlanted;
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
        
            previewItem.gameObject.SetActive(false);
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            plantButton.interactable = false;
            
            previewItem.gameObject.SetActive(true);
            previewItem.DisplayedItem = item;
            previewItem.DisplayedItem.Amount = 1;
            previewItem.UpdateInformation();
            
            if(previewItem.DisplayedItem.Item.Type == ItemType.Seed)
                plantButton.interactable = true;
        
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
            LoadData(GameStateManager.CurrentState);
        }

        private void SeedPlanted()
        {
            GardenSpotManager.OnPlantSeed(cam.lastSelectedGardenSpotID, previewItem.DisplayedItem.Item.AppendID);
            GameStateManager.OnRemoveInventoryItem(previewItem.DisplayedItem.Item.ID);
        }
    }
}
