using System;
using Firebase.Firestore;

namespace Stateful
{
    [FirestoreData]
    public class FirebaseData
    {
        // User Identification
        private string _uid;
        
        // Creation time of user
        private DateTime _creationTime;
        
        // --- TELEMETRY
        // Last login
        private DateTime _lastLogin;
        
        // Amount of times they logged in
        private int _logins;
        
        // Total amount of playtime
        private float _playtime;
        
        // House level
        private int _level;
        
        // Amount of plants harvested
        private int _harvests;
        
        // Total distance walked
        private float _dist;
        
        [FirestoreProperty]
        public string UID
        {
            get => _uid;
            set => _uid = value;
        }
        
        [FirestoreProperty]
        public DateTime CreationTime
        {
            get => _creationTime;
            set => _creationTime = value;
        }
        
        [FirestoreProperty]
        public DateTime LastLogin
        {
            get => _lastLogin;
            set => _lastLogin = value;
        }
        
        [FirestoreProperty]
        public int Logins
        {
            get => _logins;
            set => _logins = value;
        }
        
        [FirestoreProperty]
        public float Playtime
        {
            get => _playtime;
            set => _playtime = value;
        }
        
        [FirestoreProperty]
        public int Level
        {
            get => _level;
            set => _level = value;
        }
        [FirestoreProperty]
        public int Harvests
        {
            get => _harvests;
            set => _harvests = value;
        }
        [FirestoreProperty]
        public float Distance
        {
            get => _dist;
            set => _dist = value;
        }
    }
}
