using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using Stateful.Managers;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    [Serializable]
    public struct SpawnerCluster
    {
        public LatitudeLongitude latLng;
        public List<Spawner> spawners;
    }

    [Serializable]
    public struct Spawner
    {
        public int itemId;
        public int minAmount;
        public int maxAmount;
    }

    public class SpawnerOnMap : MonoBehaviour
    {
        public Spawner spawner;
        public int spawnerId;
        public LatitudeLongitude latLng;
        public GameObject text;
        public Transform player;

        public TMP_Text textQuality;
        public TMP_Text textQuantity;

        public bool collected;

        public MapResourceManager resourceManager;

        public void CollectThisResource()
        {
            if (collected)
                return;
            StartCoroutine(FlyAndCollect());
        }

        public void Start()
        {
            if (collected)
            {
                GetComponent<Renderer>().material.color = Color.gray;
                textQuality.text = "Collected!";
                textQuantity.text = "Good work!";
                return;
            }

            GetComponent<Renderer>().material.color = Items.FromID(spawner.itemId).Rarity switch
            {
                Rarity.Common => Color.white,
                Rarity.Uncommon => new Color(0.19f, 0.38f, 0.65f),
                Rarity.Rare => new Color(0.91f, 0.09f, 0.32f),
                Rarity.Legendary => new Color(0.91f, 0.63f, 0.09f),
                Rarity.Special => Color.black,
                _ => throw new ArgumentOutOfRangeException()
            };

            textQuality.text = Items.FromID(spawner.itemId).Rarity switch
            {
                Rarity.Common => "Common",
                Rarity.Uncommon => "<Color=#3061a6>Uncommon",
                Rarity.Rare => "<Color=#e81752>Rare",
                Rarity.Legendary => "<Color=#e8a117>Legendary",
                Rarity.Special => "<Color='black'>Special",
                _ => throw new ArgumentOutOfRangeException()
            };

            textQuantity.text =
                spawner.minAmount == spawner.maxAmount ? spawner.minAmount.ToString() : $"{spawner.minAmount}~{spawner.maxAmount}";

        }

        public void Update()
        {
            text.transform.LookAt(Camera.main.transform);
        }

        private IEnumerator FlyAndCollect()
        {
            float duration = 3f; // Total flight time
            float acceleration = 7f; // Speed multiplier for acceleration
            float elapsedTime = 0f;

            Vector3 direction = Vector3.up; // Fly straight up

            while (elapsedTime < duration)
            {
                // Accelerate over time (quadratic growth)
                float speed = Mathf.Pow(elapsedTime + 1, acceleration); // Starts slow, speeds up
                transform.position += speed * Time.deltaTime * direction;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // Mark as collected immediately
            collected = true;
            GetComponent<Renderer>().material.color = Color.gray;

            // Register collection with the resource manager
            resourceManager.Collect(this);

            Debug.Log("Resource has been collected and flown away!");
        }
    }
}