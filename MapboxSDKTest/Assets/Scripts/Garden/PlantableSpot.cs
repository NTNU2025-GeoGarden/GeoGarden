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
        public bool harvestable;
        public DateTime completionTime;
        public GrowState state;
        
        public GameObject perimeter;
        public GameObject statusSymbolAddPlant;
        public GameObject statusSymbolNeedsWater;
        public GameObject statusSymbolFinished;
        public GameObject growingStage1;
        public GameObject growingStage2;
        public GameObject growingStage3;
        public GameObject growingStage4;
        [FormerlySerializedAs("collider")] public BoxCollider boxCollider;

        public TMP_Text statusSymbolTimer;
        public AudioClip waterPopSoundEffect;
        
        private AudioSource _audioSource;
        public void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            if (!needsWater && !completionTime.Equals(DateTime.MinValue))
            {
                TimeSpan timeLeft = completionTime.Subtract(DateTime.Now);
                statusSymbolTimer.text = timeLeft.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);

                if (timeLeft < TimeSpan.Zero)
                {
                    GardenManager.OnSeedTimeout(this);
                    needsWater = true;
                }
            }
            else if (state == GrowState.Complete)
                harvestable = true;
        }

        public void UserPoppedWaterPopup()
        {
            _audioSource.PlayOneShot(waterPopSoundEffect);
            needsWater = false;
        }
        
        public void UserHarvestedPlant()
        {
            _audioSource.PlayOneShot(waterPopSoundEffect);

            harvestable = false;
        }



    }
}
