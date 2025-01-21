using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public InventoryItem Item;
    public TMP_Text amount;
    
    public Image icon;
    public Image itemBg;
    public Sprite seedIcon;

    public GameObject star1;
    public GameObject star2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        amount.text = "x" + Item.amount;
        
        switch(Item.type)
        {
            case ResourceType.Seed:
                icon.sprite = seedIcon;
                break;
            case ResourceType.Water:
                break;
            case ResourceType.Fertilizer:
                break;
            case ResourceType.Item:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        switch(Item.quality)
        {
            case Quality.Common:
                itemBg.color = Color.white;
                break;
            case Quality.Uncommon:
                itemBg.color = new Color(0.29f, 0.722f, 1f);
                break;
            case Quality.Rare:
                itemBg.color = new Color(1f, 0.325f, 0.204f);
                star2.SetActive(true);
                break;
            case Quality.Special:
            case Quality.Legendary:
                itemBg.color = new Color(1f, 0.655f, 0f);
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
}
