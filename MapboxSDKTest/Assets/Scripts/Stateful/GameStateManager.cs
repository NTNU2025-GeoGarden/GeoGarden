using System;
using System.Collections.Generic;
using System.Linq;
using Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stateful
{
    public class GameStateManager : MonoBehaviour
    {
        public delegate void DelegateRemoveInventoryItem(int itemID);

        public static DelegateRemoveInventoryItem OnRemoveInventoryItem;
        
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
            // TODO
            // Load today's map configuration -> if it hasn't been generated, then generate.
            // --> Contains what nodes the user might have already visited
            // Load gamestate

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
            // TODO - pass data to other scripts so they can update
            // TODO - save using the data handler

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
    }
}
