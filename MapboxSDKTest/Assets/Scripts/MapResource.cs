using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct ResourceCluster
{
    public LatitudeLongitude latLng;
    public List<ResourceSpawn> spawners;
}

[Serializable]
public struct ResourceSpawn
{
    public List<ResourceDrop> drops;
    public Quality minQuality;
    public Quality maxQuality;
}

/// <summary>
/// ResourceDrop describes a one-time drop event. The resource of drop is given between
/// minAmount and maxAmount of times.
/// If minAmount == maxAmount, that exact amount is always given.
/// </summary>
[Serializable]
public struct ResourceDrop
{
    public ResourceType drop;
    public int minAmount;
    public int maxAmount;
}

[Serializable]
public class MapResource : MonoBehaviour
{
    public delegate void CollectResource();
    public CollectResource OnCollectResource;

    public ResourceSpawn spawner;
    public LatitudeLongitude latLng;
    public GameObject text;
    public Transform player;

    public TMP_Text textQuality;
    public TMP_Text textQuantity;

    public bool collected;
    
    public void Start()
    {
        OnCollectResource += TryCollectThisResource;

        if (collected)
        {
            GetComponent<Renderer>().material.color = Color.gray;
            textQuality.text = "Collected!";
            textQuantity.text = "Good work!";
            return;
        }
        
        GetComponent<Renderer>().material.color = spawner.maxQuality switch
        {
            Quality.Common    => Color.white,
            Quality.Uncommon  => new Color(0.19f, 0.38f, 0.65f),
            Quality.Rare      => new Color(0.91f, 0.09f, 0.32f),
            Quality.Legendary => new Color(0.91f, 0.63f, 0.09f),
            Quality.Special   => Color.black,
            _ => throw new ArgumentOutOfRangeException()
        };

        textQuality.text = spawner.maxQuality switch
        {
            Quality.Common => "Common?",
            Quality.Uncommon => "<Color=#3061a6>Uncommon?",
            Quality.Rare => "<Color=#e81752>Rare?",
            Quality.Legendary => "<Color=#e8a117>Legendary?",
            Quality.Special => "<Color='black'>Special",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        textQuantity.text =
            spawner.drops[0].minAmount == spawner.drops[0].maxAmount ? spawner.drops[0].minAmount.ToString() : $"{spawner.drops[0].minAmount}~{spawner.drops[0].maxAmount}";

    }

    public void Update()
    {
        text.transform.LookAt(player);
    }

    private void TryCollectThisResource()
    {
        GetComponent<Renderer>().material.color = Color.gray;
        textQuality.text = "Collected!";
        textQuantity.text = "Good work!";
        collected = true;

        MapResourceManager.OnRegisterCollectResource(latLng);
        // Award resources to player
    }
}
