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
        public int used;

        private GameState _state;
        
        public void Start()
        {
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

            foreach (SerializableObject obj in GameStateManager.CurrentState.Objects.Where(obj => obj.Type == type))
            {
                used++;
            }
            
            amountText.text = $"{used}/{available}";
        }

        public void CheckSpawnObject()
        {
            if (used >= available) return;
            
            used++;
            amountText.text = $"{used}/{available}";

            EditableObject obj = Instantiate(editableObjectPrefab, manager.transform)
                .GetComponent<EditableObject>();
            obj.type = type;
            obj.editControls.SetActive(false);
            obj.ObjectID = manager.GetObjectCount();
            manager.AddObject(obj);

            GameStateManager.OnForceSaveGame();
        }
    }
}
