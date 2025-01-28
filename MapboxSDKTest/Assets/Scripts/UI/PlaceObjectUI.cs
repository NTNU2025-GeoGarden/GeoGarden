using System;
using Garden;
using Structs;
using UnityEngine;

namespace UI
{
    public class PlaceItemUI : MonoBehaviour
    {
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder;
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject woodPileHolder;
        
        public PlaceableObjectType type;

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
        }
    }
}
