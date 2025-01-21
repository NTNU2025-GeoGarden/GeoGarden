using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Persistence
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("Save data storage config")] [SerializeField]
        private string fileName;
        
        public static GameStateManager Instance { get; private set; }

        public static GameState CurrentState { get; private set; }
        
        private List<IPersistence> _persistenceObjs;
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

        public void NewGame()
        {
            CurrentState = new GameState();
            
            CurrentState.inventory.Add(new InventoryItem
            {
                amount  = 1,
                quality = Quality.Common,
                type    = ResourceType.Seed
            });
            
            CurrentState.inventory.Add(new InventoryItem
            {
                amount  = 1,
                quality = Quality.Uncommon,
                type    = ResourceType.Seed
            });
            
            CurrentState.inventory.Add(new InventoryItem
            {
                amount  = 1,
                quality = Quality.Rare,
                type    = ResourceType.Seed
            });
            
            CurrentState.inventory.Add(new InventoryItem
            {
                amount  = 1,
                quality = Quality.Legendary,
                type    = ResourceType.Seed
            });
        }

        public void LoadGame()
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

            foreach (IPersistence persistenceObj in _persistenceObjs)
            {
                persistenceObj.LoadData(CurrentState);
            }
        }

        public void SaveGame()
        {
            // TODO - pass data to other scripts so they can update
            // TODO - save using the data handler

            GameState currentState = CurrentState;
            foreach (IPersistence persistenceObj in _persistenceObjs)
            {
                persistenceObj.SaveData(ref currentState);
            }
            
            _dataHandler.Save(currentState);
        }

        private List<IPersistence> FindAllPersistenceObjs()
        {
            IEnumerable<IPersistence> persistenceObjs = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPersistence>();
            
            return new List<IPersistence>(persistenceObjs);
        }
    }
}
