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
        public List<SpawnerItemDrop> drops;
        [FormerlySerializedAs("minQuality")] public Rarity minRarity;
        [FormerlySerializedAs("maxQuality")] public Rarity maxRarity;
    }

    [Serializable]
    public struct SpawnerItemDrop
    {
        public ItemType drop;
        public int minAmount;
        public int maxAmount;
    }

    public class SpawnerOnMap : MonoBehaviour
    {
        public delegate void CollectResource();
        public CollectResource OnCollectResource;

       
        public Spawner spawner;
        public LatitudeLongitude latLng;
        public GameObject text;
        public Transform player;

        public TMP_Text textQuality;
        public TMP_Text textQuantity;

        public bool collected;

         public void CollectThisResource()
    {
        if (collected) return; // Prevent duplicate collection
        
        StartCoroutine(FlyAndCollect());
     
    }

        public void Start()
        {
            //OnCollectResource += TryCollectThisResource;

            if (collected)
            {
                GetComponent<Renderer>().material.color = Color.gray;
                textQuality.text = "Collected!";
                textQuantity.text = "Good work!";
                return;
            }
        
            GetComponent<Renderer>().material.color = spawner.maxRarity switch
            {
                Rarity.Common    => Color.white,
                Rarity.Uncommon  => new Color(0.19f, 0.38f, 0.65f),
                Rarity.Rare      => new Color(0.91f, 0.09f, 0.32f),
                Rarity.Legendary => new Color(0.91f, 0.63f, 0.09f),
                Rarity.Special   => Color.black,
                _ => throw new ArgumentOutOfRangeException()
            };

            textQuality.text = spawner.maxRarity switch
            {
                Rarity.Common => "Common?",
                Rarity.Uncommon => "<Color=#3061a6>Uncommon?",
                Rarity.Rare => "<Color=#e81752>Rare?",
                Rarity.Legendary => "<Color=#e8a117>Legendary?",
                Rarity.Special => "<Color='black'>Special",
                _ => throw new ArgumentOutOfRangeException()
            };
        
            textQuantity.text =
                spawner.drops[0].minAmount == spawner.drops[0].maxAmount ? spawner.drops[0].minAmount.ToString() : $"{spawner.drops[0].minAmount}~{spawner.drops[0].maxAmount}";

        }

        public void Update()
        {
            text.transform.LookAt(player);
        }

    /*
        private void TryCollectThisResource()
        {
          StartCoroutine(SpinAndCollect());
            // TODO Award resources to player
        } */


    /*
        private IEnumerator SwapToOpenBox()
        {
            yield return new WaitForSeconds(2.0f); // ✅ Optional short delay before swapping

            // ✅ Instantiate the open box at the same position and rotation
            GameObject openBox = Instantiate(openBoxPrefab, transform.position, transform.rotation);

            Debug.Log("✅ Box has been swapped to open!");

            // ✅ Register collection with the resource manager
            MapResourceManager.OnRegisterCollectResource(latLng);

            collected = true;
            GetComponent<Renderer>().material.color = Color.gray;
            Debug.Log("Collected, Good work");
            
            // Register collection with the resource manager
            MapResourceManager.OnRegisterCollectResource(latLng);

            
            Debug.Log("Resource has been collected and flown away!");
            // ✅ Destroy the closed box
            //Destroy(gameObject);
        } */

private IEnumerator FlyAndCollect()
{
    float duration = 5f; // Total flight time
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
    Debug.Log("Collected, Good work");
    
    // Register collection with the resource manager
    MapResourceManager.OnRegisterCollectResource(latLng);

    
    Debug.Log("Resource has been collected and flown away!");
    // Destroy or hide the object after flight
    //Destroy(gameObject); // Remove after flying to "space"
    
    
} 

    }
}