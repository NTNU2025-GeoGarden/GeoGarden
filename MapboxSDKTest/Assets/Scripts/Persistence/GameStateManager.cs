using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Persistence
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        private GameState _gameState;
        private List<IPersistence> _persistenceObjs;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one GameStateManager already exsists. This should not be possible. Are there more than one GameStateManager scripts in the scene?");
            }

            Instance = this;
        }

        public void Start()
        {
            _persistenceObjs = FindAllPersistenceObjs();
            LoadGame();
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        public void NewGame()
        {
            _gameState = new GameState();
        }

        public void LoadGame()
        {
            // TODO
            // Load today's map configuration -> if it hasn't been generated, then generate.
            // --> Contains what nodes the user might have already visited
            // Load gamestate

            
            if (_gameState != null) return;
            
            Debug.Log("No data. Initializing game state to default values.");
            NewGame();

            // Push loaded data to other scripts

            foreach (IPersistence persistenceObj in _persistenceObjs)
            {
                persistenceObj.LoadData(_gameState);
            }
        }

        public void SaveGame()
        {
            // TODO - pass data to other scripts so they can update
            // TODO - save using the data handler
            
            foreach (IPersistence persistenceObj in _persistenceObjs)
            {
                persistenceObj.SaveData(ref _gameState);
            }
        }

        private List<IPersistence> FindAllPersistenceObjs()
        {
            IEnumerable<IPersistence> persistenceObjs = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPersistence>();
            
            return new List<IPersistence>(persistenceObjs);
        }
    }
}
