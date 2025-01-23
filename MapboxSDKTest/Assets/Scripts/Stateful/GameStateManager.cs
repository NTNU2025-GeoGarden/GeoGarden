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

            CurrentState.Inventory.Add((0, 1));
            CurrentState.Inventory.Add((1, 1));
            CurrentState.Inventory.Add((2, 1));
            CurrentState.Inventory.Add((3, 1));
            
            CurrentState.GardenSpots.Add(new SerializableGardenSpot
            {
                state = GrowState.Vacant
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

            foreach (IUsingGameState persistenceObj in _persistenceObjs)
            {
                persistenceObj.LoadData(CurrentState);
            }
        }

        public void SaveGame()
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
    }
}
