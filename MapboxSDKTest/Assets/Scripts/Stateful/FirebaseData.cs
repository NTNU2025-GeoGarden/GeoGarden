using System;
using Firebase.Firestore;

namespace Stateful
{
    [FirestoreData]
    public class FirebaseData
    {
        private string _uid;
        private DateTime _creationTime;
        private DateTime _lastLogin;
        private int _logins;
        private float _playtime;
        
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
    }
}
