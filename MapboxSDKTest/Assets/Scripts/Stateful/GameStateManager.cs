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
        public delegate void ForceSaveGame();

        public static ForceSaveGame OnForceSaveGame;
        
        [Header("Save data storage config")] [SerializeField]
        private string fileName;
        
        public static GameStateManager Instance { get; private set; }

        public static GameState CurrentState { get; private set; }
        
        private List<IUsingGameState> _persistenceObjs;
        private FileDataHandler _dataHandler;

        private static int GAMEDATA_VERSION = 6;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("<color=lime>[GameStateManager] Already present in scene. Removing duplicate instance</color>");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(this);

            Application.targetFrameRate = 60;
                
            SceneManager.activeSceneChanged += (_, _) =>
            {
                Debug.Log("<color=lime>[GameStateManager] Scene load triggering game load</color>");
                _dataHandler       = new FileDataHandler(Application.persistentDataPath, fileName);
                _persistenceObjs   = FindAllPersistenceObjs();
                LoadGame();
            };

            OnForceSaveGame       += () =>
            {
                Debug.Log("<color=lime>[GameStateManager] Forcefully saving data</color>");
                SaveGame();
            };
        }

        public void Start()
        {
            _dataHandler     = new FileDataHandler(Application.persistentDataPath, fileName);
            _persistenceObjs = FindAllPersistenceObjs();
        }

        public void OnApplicationQuit()
        {
            Debug.Log("<color=lime>[GameStateManager] Saving due to application exit</color>");
            SaveGame();
        }

        private void NewGame()
        {
            Debug.Log("<color=lime>[GameStateManager] Generating new game data</color>");
            CurrentState = new GameState();
            
            /*TODO debug data - give the player some starting configuration, maybe through tutorial?
                Player is given some amount of starting money and seeds
                The player is taught to use the shop to buy the first spot to plant seeds
                The player is then taught how to plant the seed
            */

            CurrentState.Version      = GAMEDATA_VERSION;
            CurrentState.HouseLevel   = 1;
            CurrentState.LastLogin    = DateTime.MinValue;
            CurrentState.DaysLoggedIn = 0;
            CurrentState.DaysClaimed  = new List<bool>(14) { false, false, false, false, false, false, false, false, false, false, false, false, false, false};
            CurrentState.CoinCap      = HouseUpgrades.CoinCapPerLevel[1];
            CurrentState.Coins        = 20;
            CurrentState.EnergyCap    = HouseUpgrades.EnergyCapPerLevel[1];
            CurrentState.Energy       = 20;
            CurrentState.WaterCap     = HouseUpgrades.WaterCapPerLevel[1];
            CurrentState.Water        = 20;
            CurrentState.GardenTutorial = false;
            CurrentState.MapTutorial    = false;
            CurrentState.LayoutTutorial = false;
            
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 0, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 1, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 2, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 3, Amount = 1});
            
            CurrentState.GardenSpots.Add(new SerializableGardenSpot
            {
                state = GrowState.Vacant,
                X = 0.3f,
                Y = 0,
                Z = 0.3f
            });
        }

        private void LoadGame()
        {
            Debug.Log("<color=cyan>[GameStateManager] Loading data</color>");
            
            CurrentState = _dataHandler.Load();

            if (CurrentState == null)
            {
                Debug.Log("<color=cyan>----> No data found, initializing to default values</color>");
                NewGame();
            }
            
            if(CurrentState!.Version < GAMEDATA_VERSION)
            {
                Debug.Log("<color=cyan>----> Data found, but version is outdated. Initializing to default values</color>");
                CurrentState = null;
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
            Debug.Log("<color=cyan>[GameStateManager] Saving data</color>");
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

        public static void RemoveInventoryItem(int itemID)
        {
            Debug.Log($"<color=lime>[GameStateManager] Removing item (ID {itemID}) from inventory</color>");
            int index = CurrentState.Inventory.FindIndex(x => x.Id == itemID);

            if (index == -1)
            {
                Debug.LogError("----> The inventory did not contain the item requested to be removed.");
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
        
        public static void AddInventoryItem(SerializableInventoryEntry newItem)
        {
            Debug.Log($"<color=lime>[GameStateManager] Adding item (ID {newItem.Id}) to inventory</color>");
            int index = CurrentState.Inventory.FindIndex(x => x.Id == newItem.Id);

            if (index == -1)
            {
                CurrentState.Inventory.Add(newItem);
            }
            else
            {
                SerializableInventoryEntry entry = CurrentState.Inventory[index];
                entry.Amount += newItem.Amount;
                CurrentState.Inventory[index] = entry;
            }
        }
        
    }
}
