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
        
        public delegate void DelegateInventoryEvent(int itemID);

        public static DelegateInventoryEvent OnRemoveInventoryItem;
        public static DelegateInventoryEvent OnAddInventoryItem;
        
        [Header("Save data storage config")] [SerializeField]
        private string fileName;
        
        public static GameStateManager Instance { get; private set; }

        public static GameState CurrentState { get; private set; }
        
        private List<IUsingGameState> _persistenceObjs;
        private FileDataHandler _dataHandler;

        public static int GAMEDATA_VERSION = 2;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"<color=orange>[GameStateManager] Already present in scene. Removing duplicate instance</color>");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            Application.targetFrameRate = 60;
                
            SceneManager.activeSceneChanged += (_, _) =>
            {
                Debug.Log($"<color=orange>[GameStateManager] Scene load triggering game load</color>");
                _dataHandler       = new FileDataHandler(Application.persistentDataPath, fileName);
                _persistenceObjs   = FindAllPersistenceObjs();
                LoadGame();
            };

            OnRemoveInventoryItem += RemoveInventoryItem;
            OnAddInventoryItem    += AddInventoryItem;
            OnForceSaveGame       += () =>
            {
                Debug.Log($"<color=orange>[GameStateManager] Force saving!</color>");
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

            CurrentState.Version = GAMEDATA_VERSION;
            
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 0, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 1, Amount = 2});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 2, Amount = 1});
            CurrentState.Inventory.Add(new SerializableInventoryEntry{Id = 3, Amount = 1});
            
            CurrentState.GardenSpots.Add(new SerializableGardenSpot
            {
                state = GrowState.Vacant,
                X = 0.3f,
                Y = 0,
                Z = 0.3f
            });
            
            CurrentState.Objects.Add(new SerializableObject
            {
                   X        = 0,
                   Y        = 0,
                   Z        = 0,
                RotX        = 0,
                RotY        = 0,
                RotZ        = 0,
                RotW        = 1,
                Type     = EditableObjectType.Tree
            });
        }

        private void LoadGame()
        {
            Debug.Log("<color=cyan>[GameStateManager] Loading game data</color>");
            
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
            Debug.Log("<color=cyan>[GameStateManager] Saving game data</color>");
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
            Debug.Log($"<color=cyan>[GameStateManager] Removing itemID {itemID} from inventory</color>");
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
        
        private void AddInventoryItem(int itemID)
        {
            Debug.Log($"<color=cyan>[GameStateManager] Adding itemID {itemID} to inventory</color>");
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
