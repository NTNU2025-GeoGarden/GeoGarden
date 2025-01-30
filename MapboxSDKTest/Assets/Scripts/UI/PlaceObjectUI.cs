using System;
using System.Linq;
using Garden;
using Stateful;
using Stateful.Managers;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlaceObjectUI : MonoBehaviour
    {
        public delegate void DelegateUpdatePlaceObjectUICount();

        public static DelegateUpdatePlaceObjectUICount OnUpdatePlaceObjectUICount;

        public GardenCamera gardenCamera;
        public GameObject editableObjectPrefab;
        public ObjectManager manager;
        
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder;
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject woodPileHolder;
        public TMP_Text amountText;
        
        public EditableObjectType type;
        
        public int available;
        private int _used;

        private GameState _state;
        
        public void Start()
        {
            OnUpdatePlaceObjectUICount += UpdateAmount;
            
            switch(type)
            {
                case EditableObjectType.Tree:
                    treeHolder.SetActive(true);
                    break;
                case EditableObjectType.Fence:
                    fenceHolder.SetActive(true);
                    break;
                case EditableObjectType.Spot:
                    spotHolder.SetActive(true);
                    break;
                case EditableObjectType.Lantern:
                    lanternHolder.SetActive(true);
                    break;
                case EditableObjectType.Banner:
                    bannerHolder.SetActive(true);
                    break;
                case EditableObjectType.Cart:
                    cartHolder.SetActive(true);
                    break;
                case EditableObjectType.WoodPile:
                    woodPileHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _used = manager.GetAmountUsed(type);
            
            amountText.text = $"{_used}/{available}";
        }

        private void UpdateAmount()
        {
            _used = manager.GetAmountUsed(type);
            Debug.Log($"New amount for {type}: {_used}");
            
            amountText.text = $"{_used}/{available}";
        }

        public void CheckSpawnObject()
        {
            if (_used >= available) return;
            
            amountText.text = $"{_used}/{available}";

            EditableObject obj = Instantiate(editableObjectPrefab, manager.transform)
                .GetComponent<EditableObject>();
            obj.type = type;
            obj.editControls.SetActive(false);
            obj.ObjectID = manager.GetObjectCount();
            obj.gardenCamera = gardenCamera;
            manager.AddObject(obj);

            GameStateManager.OnForceSaveGame();
        }
    }
}
