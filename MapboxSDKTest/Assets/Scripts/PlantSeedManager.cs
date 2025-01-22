using System;
using System.Collections.Generic;
using Persistence;
using UnityEngine;
using UnityEngine.UI;

public class PlantSeedManager : MonoBehaviour, IPersistence, IInventoryHandler, IUIScreen
{
    public InventoryItemUI baseItem;
    public InventoryItemUI previewItem;
    public Button plantButton;
    public HomeCameraRotation cam;
    
    private List<GameObject> _inventoryUIitems;

    public void Start()
    {
        LoadData(GameStateManager.CurrentState);
    }

    public void LoadData(GameState state)
    {
        if (_inventoryUIitems != null)
        {
            foreach (GameObject obj in _inventoryUIitems)
            {
                Destroy(obj);
            }
        
            _inventoryUIitems.Clear();
        }
        else
        {
            _inventoryUIitems = new List<GameObject>();
        }

        int count = 0;
        foreach ((int id, int amount) in state.Inventory)
        {
            InventoryItemUI newInventoryItem = Instantiate(baseItem.gameObject, transform).GetComponent<InventoryItemUI>();
            newInventoryItem.DisplayedItem = new InventoryItem(id, amount);
            newInventoryItem.transform.localPosition = new Vector3((count - (float)Math.Floor(count / 4f)) * 225 + 25, -25 - (float)Math.Floor(count / 4f) * 225, 0);
            newInventoryItem.ClickHandler = this;
            
            count++;
        }
        
        previewItem.gameObject.SetActive(false);
    }

    public void SaveData(ref GameState state)
    {
    }

    public void HandleCallbackFromItem(InventoryItem item)
    {
        previewItem.gameObject.SetActive(true);
        plantButton.interactable = true;
        
        previewItem.DisplayedItem = item;
        previewItem.UpdateInformation();
    }

    public void OnDisable()
    {
        previewItem.gameObject.SetActive(false);
        plantButton.interactable = false;
        
        previewItem.DisplayedItem = null;
        
        HandleUIClose();
    }

    private void OnEnable()
    {
        HandleUIOpen();
    }
    
    public void HandleUIOpen()
    {
        cam.uiOpen = true;
    }

    public void HandleUIClose()
    {
        cam.uiOpen = false;
    }
}
