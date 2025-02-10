using System;
using Firebase;
using Firebase.Extensions;
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
        
        public static float Playtime { get; private set; }

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

        public void Update()
        {
            Playtime += Time.deltaTime;
        }

        public void OnApplicationQuit()
        {
            Debug.Log("<color=lime>[FirebaseManager] Uploading telemetry on close</color>");
            TelemetryRecordLogout();
        }


        public static void CreateNewUserDocument()
        {
            GameState state = GameStateManager.CurrentState;
            
            if (!FirebaseAvailable) return;
            
            Database.Collection("users").Document(state.UID).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                FirebaseData data;
                if (task.Result.Exists)
                {
                    data = task.Result.ConvertTo<FirebaseData>();
                }
                else
                {
                    data = new FirebaseData
                    {
                        UID = state.UID,
                        CreationTime = DateTime.Now,
                        LastLogin = DateTime.Now,
                        Logins = 0,
                        Playtime = 0,
                        Level = 1,
                        Harvests = 0,
                        Distance = 0
                    };
                }
                
                Database.Collection("users").Document(data.UID).SetAsync(data);
            });
        }

        public static void TelemetryRecordLogin()
        {
            if (!FirebaseAvailable) return;
            
            DocumentReference thisUser = Database.Collection("users").Document(GameStateManager.CurrentState.UID);
                
            thisUser.UpdateAsync("Logins", FieldValue.Increment(1));
            thisUser.UpdateAsync("LastLogin", DateTime.Now);
        }

        public static void TelemetryRecordLogout()
        {
            if (!FirebaseAvailable) return;
            
            DocumentReference thisUser = Database.Collection("users").Document(GameStateManager.CurrentState.UID);

            thisUser.UpdateAsync("Playtime", FieldValue.Increment(Playtime));
            thisUser.UpdateAsync("Level", GameStateManager.CurrentState.HouseLevel);
            thisUser.UpdateAsync("Harvests", GameStateManager.CurrentState.PlantsHarvested);
            thisUser.UpdateAsync("Distance", GameStateManager.CurrentState.DistanceWalked);
        }
    }
}