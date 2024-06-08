using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    [SerializeField]
    private List<TrophyData> _allTrophies;

    [SerializeField] 
    private GameUI _gameUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameUI.InitTrophies(_allTrophies);
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
    }


    private void GetTrophy(TrophyData dat, PlayerState player)
    {
        Debug.Log($"Trophy Get! {dat.DisplayName}");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
