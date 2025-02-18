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
        public Image iconShadow;
        
        public Sprite commonSeed;
        public Sprite uncommonSeed;
        public Sprite rareSeed;
        public Sprite legendarySeed;
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

            icon.sprite = DisplayedItem.Item.Type switch
            {
                ItemType.Seed => DisplayedItem.Item.Rarity switch
                {
                    Rarity.Common    => commonSeed,
                    Rarity.Uncommon  => uncommonSeed,
                    Rarity.Rare      => rareSeed,
                    Rarity.Legendary => legendarySeed,
                    Rarity.Special   => legendarySeed,
                    _ => throw new ArgumentOutOfRangeException()
                },
                ItemType.Generic => itemIcon,
                _ => throw new ArgumentOutOfRangeException()
            };

            iconShadow.sprite = icon.sprite;

            switch(DisplayedItem.Item.Rarity)
            {
                case Rarity.Common:
                    star1.SetActive(false);
                    star2.SetActive(false);
                    break;
                case Rarity.Uncommon:
                    star1.SetActive(false);
                    star2.SetActive(false);
                    break;
                case Rarity.Rare:
                    star1.SetActive(false);
                    star2.SetActive(true);
                    star2.GetComponent<Image>().color  = new Color(1f, 0.3f, 0.8f, 0.6f);
                    break;
                case Rarity.Special:
                case Rarity.Legendary:
                    star1.SetActive(true);
                    star2.SetActive(true);
                    star1.GetComponent<Image>().color  = new Color(1f, 0.8f, 0.3f );
                    star2.GetComponent<Image>().color  = new Color(1f, 0.8f, 0.3f );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
