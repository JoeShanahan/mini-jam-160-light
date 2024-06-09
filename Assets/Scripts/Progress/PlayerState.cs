using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerState
{
    public List<int> BestSectionTimes;
    public List<int> BestRunTimes;
    public int BestOverallTime;
    public List<string> UnlockedAchievements;

    public int TrophyPoints;

    private const string PREFS_KEY = "PlayerState";

    public void OnTrophyEarned(TrophyData dat)
    {
        if (dat.Rarity == TrophyData.TrophyClass.Bronze) TrophyPoints += 10;
        if (dat.Rarity == TrophyData.TrophyClass.Silver) TrophyPoints += 20;
        if (dat.Rarity == TrophyData.TrophyClass.Gold) TrophyPoints += 50;
        if (dat.Rarity == TrophyData.TrophyClass.Platinum) TrophyPoints += 280;
        
        UnlockedAchievements.Add(dat.name);
    }
    
    public void SaveToPrefs()
    {
        string jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PREFS_KEY, jsonString);
        Debug.Log(jsonString);
        PlayerPrefs.Save();
    }

    public void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY) == false)
            return;

        string jsonString = PlayerPrefs.GetString(PREFS_KEY);
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }

    public void Wipe()
    {
        BestSectionTimes = new List<int>();
        BestRunTimes = new List<int>();
        BestOverallTime = 0;
        UnlockedAchievements = new List<string>();

        TrophyPoints = 0;
        
        if (PlayerPrefs.HasKey(PREFS_KEY) == false)
            return;
        
        PlayerPrefs.DeleteKey(PREFS_KEY);
        PlayerPrefs.Save();
    }
}
