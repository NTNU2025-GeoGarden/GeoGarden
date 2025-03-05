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
        
        [Header("Seed Icons")]
        public Sprite commonSeed;
        public Sprite uncommonSeed;
        public Sprite rareSeed;
        public Sprite legendarySeed;
        public Sprite itemIcon;

        [Header("Common Plant Icons")]
        public Sprite tomatoIcon;      // ID 4
        public Sprite cucumberIcon;    // ID 5
        public Sprite potatoIcon;      // ID 6
        public Sprite lettuceIcon;     // ID 7
        public Sprite celeryIcon;      // ID 8
        public Sprite dillIcon;        // ID 9
        public Sprite leekIcon;        // ID 10
        public Sprite onionIcon;       // ID 11
        public Sprite springOnionIcon; // ID 12
        public Sprite cauliflowerIcon; // ID 13
        public Sprite broccoliIcon;    // ID 14
        public Sprite artichokeIcon;   // ID 15
        public Sprite daikonIcon;      // ID 16
        public Sprite riceIcon;        // ID 17
        public Sprite sugarcaneIcon;   // ID 18
        public Sprite soybeanIcon;     // ID 19
        public Sprite wheatIcon;       // ID 20
        public Sprite cottonIcon;      // ID 21
        public Sprite sweetPotatoIcon; // ID 22
        public Sprite cabbageIcon;     // ID 23
        public Sprite spinachIcon;     // ID 24
        public Sprite coffeeBeanIcon;  // ID 25
        public Sprite cornIcon;        // ID 26
        public Sprite eggplantIcon;    // ID 27

        private Sprite GetSpriteById(int id)
        {
            return id switch
            {
                4 => tomatoIcon,
                5 => cucumberIcon,
                6 => potatoIcon,
                7 => lettuceIcon,
                8 => celeryIcon,
                9 => dillIcon,
                10 => leekIcon,
                11 => onionIcon,
                12 => springOnionIcon,
                13 => cauliflowerIcon,
                14 => broccoliIcon,
                15 => artichokeIcon,
                16 => daikonIcon,
                17 => riceIcon,
                18 => sugarcaneIcon,
                19 => soybeanIcon,
                20 => wheatIcon,
                21 => cottonIcon,
                22 => sweetPotatoIcon,
                23 => cabbageIcon,
                24 => spinachIcon,
                25 => coffeeBeanIcon,
                26 => cornIcon,
                27 => eggplantIcon,
                _ => itemIcon
            };
        }
            
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
        if (amount == null || DisplayedItem == null || icon == null || iconShadow == null)
        {
            return;
        }

        amount.text = "x" + DisplayedItem.Amount;

        // Update icon sprite based on item type and ID
        icon.sprite = DisplayedItem.Item.Type switch
        {
            ItemType.Seed => DisplayedItem.Item.Rarity switch
            {
                Rarity.Common => commonSeed,
                Rarity.Uncommon => uncommonSeed,
                Rarity.Rare => rareSeed,
                Rarity.Legendary => legendarySeed,
                Rarity.Special => legendarySeed,
                _ => throw new ArgumentOutOfRangeException()
            },
            ItemType.Generic => GetSpriteById(DisplayedItem.Item.ID),
            _ => throw new ArgumentOutOfRangeException()
        };

        iconShadow.sprite = icon.sprite;

        // ...existing star logic...


        iconShadow.sprite = icon.sprite;

        if (star1 == null || star2 == null)
        {
            Debug.LogError("[ItemIcon] Star GameObjects are not set!");
            return;
        }


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
