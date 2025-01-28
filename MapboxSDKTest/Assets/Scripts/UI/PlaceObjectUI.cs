using System;
using Garden;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlaceObjectUI : MonoBehaviour
    {
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder;
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject woodPileHolder;
        public TMP_Text amountText;
        
        public PlaceableObjectType type;
        
        public int available;
        public int used;

        public void Start()
        {
            switch(type)
            {
                case PlaceableObjectType.Tree:
                    treeHolder.SetActive(true);
                    break;
                case PlaceableObjectType.Fence:
                    fenceHolder.SetActive(true);
                    break;
                case PlaceableObjectType.Spot:
                    spotHolder.SetActive(true);
                    break;
                case PlaceableObjectType.Lantern:
                    lanternHolder.SetActive(true);
                    break;
                case PlaceableObjectType.Banner:
                    bannerHolder.SetActive(true);
                    break;
                case PlaceableObjectType.Cart:
                    cartHolder.SetActive(true);
                    break;
                case PlaceableObjectType.WoodPile:
                    woodPileHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            amountText.text = $"{used}/{available}";
        }

        public void CheckSpawnObject()
        {
            if (used >= available) return;
            
            used++;
            amountText.text = $"{used}/{available}";
            
            Debug.Log($"{type} spawned");
        }
    }
}
