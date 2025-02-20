using System;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemIcon : MonoBehaviour
    {
        public IUIScreenWithItemIcons ClickScreenWithItemIcons;
        public InventoryItem DisplayedItem;
        public TMP_Text amount;
    
        public Image icon;
        public Image itemBg;
        public Sprite seedIcon;
        public Sprite itemIcon;

        public GameObject star1;
        public GameObject star2;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Start()
        {
            UpdateInformation();
        }

        public void HandleItemClicked()
        {
            ClickScreenWithItemIcons.HandleCallbackFromItem(DisplayedItem);
        }

        public void UpdateInformation()
        {
            amount.text = "x" + DisplayedItem.Amount;
        
            switch(DisplayedItem.Item.Type)
            {
                case ItemType.Seed:
                    icon.sprite = seedIcon;
                    break;
                case ItemType.Water:
                    break;
                case ItemType.Fertilizer:
                    break;
                case ItemType.Generic:
                    icon.sprite = itemIcon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            switch(DisplayedItem.Item.Rarity)
            {
                case Rarity.Common:
                    itemBg.color = new Color(0, 0, 0, 0.3490196f);
                    star1.SetActive(false);
                    star2.SetActive(false);
                    break;
                case Rarity.Uncommon:
                    itemBg.color = new Color(0.29f, 0.722f, 1f);
                    star1.SetActive(false);
                    star2.SetActive(false);
                    break;
                case Rarity.Rare:
                    itemBg.color = new Color(1f, 0.325f, 0.204f);
                    star1.SetActive(false);
                    star2.SetActive(true);
                    break;
                case Rarity.Special:
                case Rarity.Legendary:
                    itemBg.color = new Color(1f, 0.655f, 0f);
                    star1.SetActive(true);
                    star2.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
