using System;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

namespace Stateful.Managers
{
    public class WalkingManager : MonoBehaviour, IUsingGameState
    {
        public MapboxMapBehaviour map;
        public Transform playerPosition;
        private bool _isReady;
        private bool _loaded;
        private LatitudeLongitude _lastPosition;
        private double _totalDistance;
        private double rewardDistanceTracker; // Track distance for rewards

        private double rewardDistance = 0.1; // Track distance for rewards

        void Start()
        {
            map.Initialized += m =>
            {
                _isReady = true;
                _lastPosition = m.mapInformation.LatitudeLongitude;
            };
        }

        void Update()
        {
            if (!(_isReady && _loaded)) return;
            LatitudeLongitude newPosition = map.MapInformation.ConvertPositionToLatLng(playerPosition.position);
            // rewardDistanceTracker is now a class-level variable
            
            int energyReward = 10;
            rewardDistanceTracker += Distance(_lastPosition, newPosition);
            _totalDistance += Distance(_lastPosition, newPosition);

            _lastPosition = newPosition;

            GameStateManager.CurrentState.DistanceWalked = (float)Math.Round(_totalDistance, 1, MidpointRounding.ToEven);

        
            if (rewardDistanceTracker >= rewardDistance)
            {
                GameStateManager.CurrentState.Energy += energyReward;
                rewardDistanceTracker = 0; // Reset reward distance tracker after awarding energy
            }
 
            
        }

        /// <summary>
        /// Haversine formula. Distance between two coordinates on the Earth.
        /// Thanks to https://stackoverflow.com/questions/639695/how-to-convert-latitude-or-longitude-to-meters
        /// </summary>
        /// <param name="a">Location 1</param>
        /// <param name="b">Location 2</param>
        /// <returns>The distance b - a in kilometers.</returns>
        private double Distance(LatitudeLongitude a, LatitudeLongitude b)
        {
            double R = 6378.137; // Radius of earth in KM
            double dLat = b.Latitude * Math.PI / 180 - a.Latitude * Math.PI / 180;
            double dLon = b.Longitude * Math.PI / 180 - a.Longitude * Math.PI / 180;
            double _a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
                    Math.Cos(a.Latitude * Math.PI / 180) * Math.Cos(b.Latitude * Math.PI / 180) *
                    Math.Sin(dLon/2) * Math.Sin(dLon/2);
            double c = 2 * Math.Atan2(Math.Sqrt(_a), Math.Sqrt(1-_a));
            double d = R * c;
            return d;
        }

        public void LoadData(GameState state)
        {
            _totalDistance = GameStateManager.CurrentState.DistanceWalked;
            _loaded = true;
        }

        public void SaveData(ref GameState state)
        {
        }
    }
}
