using System;
using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IPersistence
{
    public InventoryItemUI baseItem;
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
        foreach (InventoryItem item in state.inventory)
        {
            InventoryItemUI newInventoryItem = Instantiate(baseItem.gameObject, transform).GetComponent<InventoryItemUI>();
            newInventoryItem.Item = item;
            newInventoryItem.transform.localPosition = new Vector3((count - (float)Math.Floor(count / 4f)) * 225 + 25, -25 - (float)Math.Floor(count / 4f) * 225, 0);


            count++;
        }
    }

    public void SaveData(ref GameState state)
    {
        Debug.Log("inventory save");
    }
}
