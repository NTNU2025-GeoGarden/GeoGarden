using System;
using System.Globalization;
using Stateful.Managers;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garden
{
    public class PlantableSpot : MonoBehaviour
    {
        public int spotID;
        public int seedID;
        public bool needsWater;
        public DateTime completionTime;
        public GrowState state;
        
        public GameObject perimeter;
        public GameObject statusSymbolAddPlant;
        public GameObject statusSymbolNeedsWaterStage1;
        public GameObject statusSymbolNeedsWaterStage2;
        public GameObject statusSymbolNeedsWaterStage3;
        public GameObject growingStage1;
        public GameObject growingStage2;
        public GameObject growingStage3;
        public GameObject growingStage4;

        public TMP_Text statusSymbolTimer;

        public void Update()
        {
            if (!needsWater && !completionTime.Equals(DateTime.MinValue))
            {
                TimeSpan timeLeft = completionTime.Subtract(DateTime.Now);
                statusSymbolTimer.text = timeLeft.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);

                if (timeLeft < TimeSpan.Zero)
                {
                    GardenSpotManager.OnSeedTimeout(spotID);
                    needsWater = true;
                }
            }
        }
    }
}
