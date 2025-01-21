using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IPersistence
{
    public InventoryItemUI baseItem;
    private List<GameObject> _inventoryUIitems;
    
    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    public void LoadData(GameState state)
    {
        Debug.Log("ran once");
        Debug.Log(state.inventory);
        
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

        foreach (InventoryItem item in state.inventory)
        {
            Debug.Log(item);
        }
    }

    public void SaveData(ref GameState state)
    {
        Debug.Log("inventory save");
    }
}
