using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TimingUI _timeUI;
    [SerializeField] private LevelEndUI _endUI;
    [SerializeField] private TrophyPipUI _trophyPips;
    
    public void UpdateTimes(RunState runState)
    {
        _timeUI.UpdateTimes(runState);
    }

    public void OnWorldChange(int newWorld, PlayerState playerState)
    {
        _timeUI.OnWorldChange(newWorld, playerState);
    }

    public void OnGameComplete(RunState run, PlayerState player)
    {
        _endUI.OnGameComplete(run, player);
    }

    public void InitTrophies(List<TrophyData> allTrophies)
    {
        _trophyPips.InitTrophies(allTrophies);
    }

    public void DebugPowerButtonPressed(int idx)
    {
        FindFirstObjectByType<PlayerController>().DebugUseAbility(idx);
    }

    public void DebugCompleteLevelPressed()
    {
        FindFirstObjectByType<RunManager>().OnLevelComplete();
    }
}
