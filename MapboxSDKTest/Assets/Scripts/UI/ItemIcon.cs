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

        [Header("Uncommon Plant Icons")]
        public Sprite appleIcon;        // ID 28
        public Sprite strawberryIcon;   // ID 29
        public Sprite blackberryIcon;   // ID 30
        public Sprite lingonberryIcon;  // ID 31
        public Sprite raspberryIcon;    // ID 32
        public Sprite blackcurrantIcon; // ID 33
        public Sprite apricotIcon;      // ID 34
        public Sprite dragonfruitIcon;  // ID 35
        public Sprite mangoIcon;        // ID 36
        public Sprite limeIcon;         // ID 37
        public Sprite kiwiIcon;         // ID 38
        public Sprite edamameIcon;      // ID 39
        public Sprite orangeIcon;       // ID 40
        public Sprite papayaIcon;       // ID 41
        public Sprite pearIcon;         // ID 42
        public Sprite pineappleIcon;    // ID 43

        [Header("Rare Plant Icons")]
        public Sprite wasabiIcon;       // ID 44
        public Sprite saffronIcon;      // ID 45
        public Sprite vanillaIcon;      // ID 46
        public Sprite cinnamonIcon;     // ID 47
        public Sprite aniseIcon;        // ID 48
        public Sprite nutmegIcon;       // ID 49
        public Sprite cuminIcon;        // ID 50

        [Header("Legendary Plant Icons")]
        public Sprite pumpkinIcon;      // ID 51
        public Sprite watermelonIcon;   // ID 52
        public Sprite coconutIcon;      // ID 53
        public Sprite durianIcon;       // ID 54

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

                // Uncommon plants
                28 => appleIcon,
                29 => strawberryIcon,
                30 => blackberryIcon,
                31 => lingonberryIcon,
                32 => raspberryIcon,
                33 => blackcurrantIcon,
                34 => apricotIcon,
                35 => dragonfruitIcon,
                36 => mangoIcon,
                37 => limeIcon,
                38 => kiwiIcon,
                39 => edamameIcon,
                40 => orangeIcon,
                41 => papayaIcon,
                42 => pearIcon,
                43 => pineappleIcon,

                 // Rare Plants
                44 => wasabiIcon,
                45 => saffronIcon,
                46 => vanillaIcon,
                47 => cinnamonIcon,
                48 => aniseIcon,
                49 => nutmegIcon,
                50 => cuminIcon,

                // Legendary Plants
                51 => pumpkinIcon,
                52 => watermelonIcon,
                53 => coconutIcon,
                54 => durianIcon,

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
            ItemType.Produce => GetSpriteById(DisplayedItem.Item.ID),
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
