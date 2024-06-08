using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrophyData", menuName = "Scriptable Objects/TrophyData")]
public class TrophyData : ScriptableObject
{
    public enum TrophyClass { Bronze, Silver, Gold, Platinum };
    
    public string DisplayName;
    
    [TextArea(3, 15)]
    public string Description;
    
    public TrophyClass Rarity;

    [Header("Time Trophies")] 
    public bool IsTimeBased;
    public int WorldNumber;
    public float TimeToBeat;

    [Header("Requires Others")] 
    public List<TrophyData> OthersNeeded;
}
