using System;
using System.Globalization;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garden
{
    public class PlantableSpot : MonoBehaviour
    {
        public int ID;
        [FormerlySerializedAs("perimiter")] public GameObject perimeter;

        public DateTime completionTime;
        public GrowState state;
        
        public GameObject statusSymbolAddPlant;
        public GameObject statusSymbolNeedsWaterStage1;
        public GameObject statusSymbolNeedsWaterStage2;
        public GameObject statusSymbolNeedsWaterStage3;
        public TMP_Text statusSymbolTimer;
    
        public GameObject growingStage1;
        public GameObject growingStage2;
        public GameObject growingStage3;
        public GameObject growingStage4;


        public void Update()
        {
            if (!completionTime.Equals(DateTime.MinValue))
                statusSymbolTimer.text = completionTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
    }
}
