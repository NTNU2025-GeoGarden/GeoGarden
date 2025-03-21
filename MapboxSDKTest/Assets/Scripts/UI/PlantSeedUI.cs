using System;
using System.Collections.Generic;
using Garden;
using Stateful;
using Stateful.Managers;
using Structs;
using TMPro;
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
        public RectTransform scrollView;

        private List<ItemIcon> _inventoryUIitems;

        private InventoryItem _lastItemTapped;

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
                newItem.transform.localPosition = new Vector3(
                    count % 4 * 225 + 50,
                    -25 - (float)Math.Floor(count / 4f) * 225, 0
                );
                newItem.ClickScreenWithItemIcons = this;

                _inventoryUIitems.Add(newItem);

                count++;
            }

            previewItem.gameObject.SetActive(false);
            scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)Math.Floor(count / 4f) * 225);
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            plantButton.interactable = false;

            _lastItemTapped = item;

            previewItem.gameObject.SetActive(true);
            previewItem.DisplayedItem = item;
            previewItem.DisplayedItem.Amount = 1;
            previewItem.UpdateInformation();

            if (previewItem.DisplayedItem.Item.Type == ItemType.Seed)
            {
                int seedId = previewItem.DisplayedItem.Item.AppendID;
                Seed seed = Seeds.FromID(seedId);

                int neededEnergy = seed.Energy;
                TextMeshProUGUI buttonText = plantButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "" + neededEnergy + "";
                if (GameStateManager.CurrentState.Energy < neededEnergy)
                {
                    //TODO give user feedback
                    Debug.Log("Not enough energy");
                    return;
                }

                plantButton.interactable = true;
            }
        }

        public void OnDisable()
        {
            previewItem.gameObject.SetActive(false);
            plantButton.interactable = false;

            previewItem.DisplayedItem = null;
        }

        private void OnEnable()
        {
            LoadData(GameStateManager.CurrentState);
        }

        private void SeedPlanted()
        {
            int seedId = _lastItemTapped.Item.ID;
            int neededEnergy = Seeds.FromID(seedId).Energy;

            GardenManager.OnPlantSeed(seedId);
            GameStateManager.CurrentState.Energy -= neededEnergy;

            Debug.Log("Planted seed with id: " + seedId + "and rarity: " + Seeds.FromID(seedId).Rarity);

            FirebaseManager.TelemetryRecordEnergySpent(neededEnergy);
            GameStateManager.RemoveInventoryItem(previewItem.DisplayedItem.Item.ID);
        }
    }
}
