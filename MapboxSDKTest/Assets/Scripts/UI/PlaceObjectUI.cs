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

        [Header("Object information")]
        public bool onlyForDisplay;
        public EditableObjectType type;
        [Space(10)]
        
        [Header("Links to other objects")]
        public GardenCamera gardenCamera;
        public GameObject editableObjectPrefab;
        public ObjectManager manager;
        [Space(10)]
        
        [Header("Internal references")]
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder; 
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject woodPileHolder;
        public GameObject dirtHolder;
        public GameObject feeTextHolder;
        public TMP_Text amountText;
        public TMP_Text feeText;
        
        public int available;
        private int _used;
        private int _cost;

        private GameState _state;
        
        public void Start()
        {
            OnUpdatePlaceObjectUICount += UpdateAmount;

            _cost = EditableObjectCost.GetCostByType(type);
            
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
                case EditableObjectType.DirtPatch:
                    dirtHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (onlyForDisplay) return;
            
            _used = manager.GetAmountUsed(type);
            
            amountText.text = $"{_used}/{available}";
            feeText.text = _cost.ToString();
        }

        public void Update()
        {
            feeText.color = GameStateManager.CurrentState.Coins >= _cost ? new Color(0.690f, 0.972f, 0.741f) : new Color(0.971f, 0.694f, 0.692f);
        }

        private void UpdateAmount()
        {
            if (onlyForDisplay) return;
            
            _used = manager.GetAmountUsed(type);
            Debug.Log($"New amount for {type}: {_used}");
            
            amountText.text = $"{_used}/{available}";
        }

        public void CheckSpawnObject()
        {
            if (onlyForDisplay) return;
            if (_used >= available) return;
            if (GameStateManager.CurrentState.Coins < _cost) return;
            
            amountText.text = $"{_used}/{available}";

            EditableObject obj = Instantiate(editableObjectPrefab, manager.transform)
                .GetComponent<EditableObject>();
            obj.type = type;
            obj.editControls.SetActive(false);
            obj.ObjectID = manager.GetObjectCount();
            obj.gardenCamera = gardenCamera;
            obj.transform.position = Vector3.zero;
            manager.AddObject(obj);
            
            GameStateManager.CurrentState.Coins -= _cost;
            FirebaseManager.TelemetryRecordCoinsUsed(_cost);
            GameStateManager.OnForceSaveGame();
        }
    }
}
