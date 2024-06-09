using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    [SerializeField]
    private List<TrophyData> _allTrophies;

    [SerializeField] 
    private GameUI _gameUI;
    
    public void InitTrophies(PlayerState player)
    {
        _gameUI.InitTrophies(_allTrophies, player.UnlockedAchievements);
    }

    public void CheckForTrophies(RunState run, PlayerState player)
    {
        // Segment time based trophies
        for (int i = 0; i < run.CurrentWorld; i++)
        {
            float completeTime = run.CurrentTimes[i];
            int worldNum = i + 1;

            foreach (TrophyData dat in _allTrophies)
            {
                if (player.UnlockedAchievements.Contains(dat.name))
                    continue;
                
                if (dat.IsTimeBased && dat.WorldNumber == worldNum)
                {
                    if (completeTime < dat.TimeToBeat)
                    {
                        GetTrophy(dat, player);
                    }
                }
            }
        }
        
        // Game completion speed trophies
        bool isGameComplete = run.CurrentWorld >= 3;
        
        if (isGameComplete)
        {
            float gameCompleteTime = run.RunTime;
            
            foreach (TrophyData dat in _allTrophies)
            {
                if (player.UnlockedAchievements.Contains(dat.name))
                    continue;
                
                if (dat.IsTimeBased && dat.WorldNumber == 0)
                {
                    if (gameCompleteTime < dat.TimeToBeat)
                    {
                        GetTrophy(dat, player);
                    }
                }
            }
        }
        
        // Trophies based on other trophies
        foreach (TrophyData dat in _allTrophies)
        {
            if (dat.OthersNeeded.Count == 0)
                continue;

            bool haveAll = true;
            
            foreach (TrophyData other in dat.OthersNeeded)
            {
                if (player.UnlockedAchievements.Contains(other.name) == false)
                {
                    haveAll = false;
                    break;
                }
            }

            if (haveAll)
            {
                GetTrophy(dat, player);
            }
        }
    }
    
    private void GetTrophy(TrophyData dat, PlayerState player)
    {
        player.UnlockedAchievements.Add(dat.name);
        _gameUI.UnlockTrophy(dat, _allTrophies, player);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
