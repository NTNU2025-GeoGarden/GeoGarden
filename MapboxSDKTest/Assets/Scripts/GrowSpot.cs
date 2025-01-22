using UnityEngine;

public enum GrowState
{
    Seed,
    Stage1,
    Stage2,
    Stage3,
    Complete
}

public class GrowSpot : MonoBehaviour
{
    public GameObject perimiter;
    
    public GameObject statusSymbolAddPlant;
    public GameObject statusSymbolNeedsWaterStage1;
    public GameObject statusSymbolNeedsWaterStage2;
    public GameObject statusSymbolNeedsWaterStage3;
    
    public GameObject growingStage1;
    public GameObject growingStage2;
    public GameObject growingStage3;
    public GameObject growingStage4;
}
