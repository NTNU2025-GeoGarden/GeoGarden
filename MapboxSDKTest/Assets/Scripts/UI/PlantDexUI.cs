using System;
using System.Collections.Generic;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlantDexUI : MonoBehaviour, IUsingGameState, IUIScreenWithItemIcons
    {
        public ItemIcon baseItem;
        public ItemIcon questionItem;

        private List<ItemIcon> _inventoryUIitems;
        public TextMeshProUGUI plantNameText;
        public TextMeshProUGUI plantRarityText;

        public RectTransform scrollView;

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
            foreach (Item item in Items.ItemList)
            {
                if (item.Type == ItemType.Seed) continue;

                ItemIcon newItem = Instantiate(state.SeenPlants[item.ID] ? baseItem.gameObject : questionItem.gameObject, transform).GetComponent<ItemIcon>();
                newItem.DisplayedItem = new InventoryItem(item.ID, 1);

                newItem.transform.localPosition = new Vector3(
                    count % 5 * 225 + 5,
                    -25 - (float)Math.Floor(count / 5f) * 225, 0
                    );

                newItem.ClickScreenWithItemIcons = this;

                _inventoryUIitems.Add(newItem);

                count++;
            }

            scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((float)Math.Floor(count / 5f) + 1) * 225);
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            if (!GameStateManager.CurrentState.SeenPlants[item.Item.ID])
            {
                plantRarityText.text = item.Item.Rarity.ToString() + ". " + "???";
                plantNameText.text = "Not seen";
            }
            else
            {
                plantRarityText.text = item.Item.Rarity.ToString() + ". " + item.Item.Description;
                plantNameText.text = item.Item.Name;
            }
        }

        private void OnEnable()
        {
            LoadData(GameStateManager.CurrentState);
        }
    }
}
