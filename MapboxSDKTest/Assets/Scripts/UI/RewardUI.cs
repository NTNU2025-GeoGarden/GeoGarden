using System;
using System.Collections.Generic;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RewardUI : MonoBehaviour, IUIScreenWithItemIcons
    {
        public InventoryItem itemReward;
        
        public ItemIcon baseItem;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI itemTypeText;
        public TextMeshProUGUI itemRarityText;

        public RectTransform scrollView;

        public void Start()
        {
            ItemIcon newItem = Instantiate(baseItem.gameObject, transform).GetComponent<ItemIcon>();
            newItem.DisplayedItem = itemReward;
            newItem.transform.localPosition = new Vector3(
                0 % 4 * 225 + 50,
                -25 - (float)Math.Floor(0 / 4f) * 225, 0
                );

            newItem.ClickScreenWithItemIcons = this;
            newItem.HandleItemClicked();
        }

        public void SaveData(ref GameState state)
        {
        }

        public void HandleCallbackFromItem(InventoryItem item)
        {
            itemTypeText.text = item.Item.Name;
            descriptionText.text = item.Item.Description;
            itemRarityText.text = item.Item.Rarity.ToString();
        }
    }
}
