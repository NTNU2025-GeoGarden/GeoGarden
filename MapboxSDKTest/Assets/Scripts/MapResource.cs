using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ResourceDrop describes a one-time drop event. The resource of drop is given between
/// minAmount and maxAmount of times.
/// If minAmount == maxAmount, that exact amount is always given.
/// </summary>
public struct ResourceDrop
{
    public ResourceType drop;
    public int minAmount;
    public int maxAmount;
}

/// <summary>
/// MapResource describes a one-time location based event that the player can trigger.
/// The MapResource will, upon the player reaching the location, as long as it is active,
/// give the player random drop(s) from the described types and qualities. It should work as this:
/// For each type in dropTypes, drop between
/// </summary>
public class MapResource : MonoBehaviour
{
    public List<ResourceDrop> drops;
    public Quality minQuality;
    public Quality maxQuality;
}
