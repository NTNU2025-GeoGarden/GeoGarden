using System;
using System.Collections.Generic;
using System.Linq;
using Structs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stateful
{
    public class GameStateManager : MonoBehaviour
    {
        public delegate void DelegateInventoryEvent(int itemID);

        public static DelegateInventoryEvent OnRemoveInventoryItem;
        public static DelegateInventoryEvent OnAddInventoryItem;
        
        [Header("Save data storage config")] [SerializeField]
        private string fileName;
        
        public static GameStateManager Instance { get; private set; }

        public static GameState CurrentState { get; private set; }
        
        private List<IUsingGameState> _persistenceObjs;
        private FileDataHandler _dataHandler;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Already loaded. Destroying.");
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(this);

            SceneManager.sceneLoaded += (_, _) =>
            {
                _dataHandler     = new FileDataHandler(Application.persistentDataPath, fileName);
                _persistenceObjs = FindAllPersistenceObjs();
                LoadGame();
            };

            OnRemoveInventoryItem += RemoveInventoryItem;
            OnAddInventoryItem    += AddInventoryItem;
        }

        public void Start()
        {
            _dataHandler     = new FileDataHandler(Application.persistentDataPath, fileName);
            _persistenceObjs = FindAllPersistenceObjs();
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        private void NewGame()
        {
            CurrentState = new GameState();
            
            /*TODO debug data - give the player some starting configuration, maybe through tutorial?
                Player is given some amount of starting money and seeds
                The player is taught to use the shop to buy the first spot to plant seeds
                The player is then taught how to plant the seed
            */

            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 0, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 1, Amount = 2});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 2, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 3, Amount = 1});
            
            CurrentState.GardenSpots.Add(new SerializableGardenSpot
            {
                state = GrowState.Vacant
            });
        }

        private void LoadGame()
        {
            CurrentState = _dataHandler.Load();


            if (CurrentState == null)
            {
                Debug.Log("No data. Initializing game state to default values.");
                NewGame();
            }
            
            // Push loaded data to other scripts

            foreach (IUsingGameState persistenceObj in _persistenceObjs)
            {
                persistenceObj.LoadData(CurrentState);
            }
        }

        private void SaveGame()
        {
            GameState currentState = CurrentState;
            foreach (IUsingGameState persistenceObj in _persistenceObjs)
            {
                persistenceObj.SaveData(ref currentState);
            }
            
            _dataHandler.Save(currentState);
        }

        private List<IUsingGameState> FindAllPersistenceObjs()
        {
            IEnumerable<IUsingGameState> persistenceObjs = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IUsingGameState>();
            
            return new List<IUsingGameState>(persistenceObjs);
        }

        private void RemoveInventoryItem(int itemID)
        {
            int index = CurrentState.Inventory.FindIndex(x => x.Id == itemID);

            if (index == -1)
            {
                Debug.LogError("Tried to remove an item from the inventory which the player doesn't have. This should never happen!");
                return;
            }
            
            SerializableInventoryEntry entry = CurrentState.Inventory[index];
            entry.Amount--;
            
            if (entry.Amount <= 0)
            {
                CurrentState.Inventory.RemoveAt(index);
            }
            else
            {
                CurrentState.Inventory[index] = entry;
            }
        }
        
        private void AddInventoryItem(int itemID)
        {
            int index = CurrentState.Inventory.FindIndex(x => x.Id == itemID);

            if (index == -1)
            {
                CurrentState.Inventory.Add(new SerializableInventoryEntry
                {
                    Id = itemID,
                    Amount = 1
                });
            }
            else
            {
                SerializableInventoryEntry entry = CurrentState.Inventory[index];
                entry.Amount++;
                
                CurrentState.Inventory[index] = entry;
            }
        }
    }
}
