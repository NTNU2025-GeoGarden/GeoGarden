using System;
using Firebase;
using Firebase.Firestore;
using UnityEngine;

namespace Stateful
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager Instance { get; private set; }
        
        public static FirebaseApp App { get; private set; }
        public static FirebaseFirestore Database { get; private set; }
        
        public static bool FirebaseAvailable { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("<color=lime>[FirebaseManager] Already present in scene. Removing duplicate instance</color>");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                DependencyStatus status = task.Result;
                FirebaseAvailable = false;
                
                if (status == DependencyStatus.Available)
                {
                    Debug.Log("<color=lime>[FirebaseManager] Google Play services dependencies resolved.");
                    App = FirebaseApp.DefaultInstance;
                    Database = FirebaseFirestore.DefaultInstance;
                    
                    FirebaseAvailable = true;
                }
                else
                {
                    Debug.LogError("<color=red>[FirebaseManager] Google Play services dependencies could not be resolved!");
                    Debug.LogError(status);
                }
            });
        }

        public static void CreateNewUserDocument()
        {
            GameState state = GameStateManager.CurrentState;
            FirebaseData data = new()
            {
                UID = state.UID,
                CreationTime = DateTime.Now,
                LastLogin = DateTime.Now,
                Logins = 0,
                Playtime = 0
            };

            if (FirebaseAvailable)
            {
                Database.Collection("users").Document(data.UID).SetAsync(data);
            }
        }

        public static void TelemetryRecordLogin()
        {
            if (!FirebaseAvailable) return;
            
            DocumentReference thisUser = Database.Collection("users").Document(GameStateManager.CurrentState.UID);
                
            thisUser.UpdateAsync("Logins", FieldValue.Increment(1));
            thisUser.UpdateAsync("LastLogin", DateTime.Now);
        }
    }
}