using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TimingUI _timeUI;
    [SerializeField] private LevelEndUI _endUI;
    [SerializeField] private TrophyPipUI _trophyPips;
    [SerializeField] private TrophyToast _trophyToast;
    
    public void UpdateTimes(RunState runState) => _timeUI.UpdateTimes(runState);
    public void OnWorldChange(int newWorld, PlayerState playerState) => _timeUI.OnWorldChange(newWorld, playerState);
    public void OnGameComplete(RunState run, PlayerState player) => _endUI.OnGameComplete(run, player);

    public void InitTrophies(List<TrophyData> allTrophies, List<string> unlocked) => _trophyPips.InitTrophies(allTrophies, unlocked);
    
    public void DebugPowerButtonPressed(int idx) => FindFirstObjectByType<PlayerController>().DebugUseAbility(idx);
    public void DebugCompleteLevelPressed() => FindFirstObjectByType<RunManager>().OnLevelComplete();
    
    public void UnlockTrophy(TrophyData unlocked, List<TrophyData> allTrophies, PlayerState player)
    {
        _trophyPips.UpdateVisuals(allTrophies, player.UnlockedAchievements);
        _trophyToast.ShowTrophy(unlocked);
    }
}
