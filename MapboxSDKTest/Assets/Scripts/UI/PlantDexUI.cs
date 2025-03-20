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
                    count % 4 * 225 + 50,
                    -25 - (float)Math.Floor(count / 4f) * 225, 0
                    );

                newItem.ClickScreenWithItemIcons = this;

                if (!state.SeenPlants[item.ID])
                    newItem.DisplayedItem.Item.Name = "Not seen";

                _inventoryUIitems.Add(newItem);

                count++;
            }

            scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((float)Math.Floor(count / 4f) + 1) * 225);
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            plantNameText.text = item.Item.Name.ToString();
            if (!GameStateManager.CurrentState.SeenPlants[item.Item.ID])
                plantRarityText.text = item.Item.Rarity.ToString() + ". " + "???";
            else
                plantRarityText.text = item.Item.Rarity.ToString() + ". " + item.Item.Description;

        }

        private void OnEnable()
        {
            LoadData(GameStateManager.CurrentState);
        }
    }
}
