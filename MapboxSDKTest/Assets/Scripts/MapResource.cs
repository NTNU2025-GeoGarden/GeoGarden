using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using UnityEngine;

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

public class MapResource : MonoBehaviour
{
    
}
