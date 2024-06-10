using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private RunManager _runManager => FindFirstObjectByType<RunManager>();
    private GameUI _gameUi =>  FindFirstObjectByType<GameUI>();

    private Dictionary<AbilityType, int> _currentCounts;
    private Dictionary<AbilityType, int> _startOfLevelCounts;
    private List<AbilityType> _allAbilities;

    public void OnFirstLevelStart()
    {
        _runManager.PlayerState.LoadFromPrefs();
        _currentCounts = new Dictionary<AbilityType, int>();
        _allAbilities = new();
        
        foreach (var ab in _runManager.PlayerState.AllAbilities())
        {
            int level = _runManager.PlayerState.GetAbilityLevel(ab);

            if (level > 0)
            {
                _currentCounts[ab] = level;
                _allAbilities.Add(ab);
            }
        }
        
        FindFirstObjectByType<AbilitySelectUI>().Init(_currentCounts);
        _startOfLevelCounts = new Dictionary<AbilityType, int>(_currentCounts);
    }

    public void OnLevelComplete()
    {
        _startOfLevelCounts = new Dictionary<AbilityType, int>(_currentCounts);
    }

    public void OnLevelRestart()
    {
        _currentCounts = new Dictionary<AbilityType, int>(_startOfLevelCounts);

        foreach (var ab in _allAbilities)
        {
            FindFirstObjectByType<AbilitySelectUI>().Refresh(ab, _currentCounts[ab]);
        }
    }

    private int _abilityIdx;

    public void CycleAbility(bool forward)
    {
        if (_allAbilities == null || _allAbilities.Count == 0)
            return;
        
        if (forward)
        {
            _abilityIdx++;

            if (_abilityIdx >= _allAbilities.Count)
                _abilityIdx = 0;
        } 
        else
        {
            _abilityIdx--;
            
            if (_abilityIdx < 0)
                _abilityIdx = _allAbilities.Count - 1;
        }
        
        Debug.Log($"Equipped ability: {_allAbilities[_abilityIdx]}");
        FindFirstObjectByType<AbilitySelectUI>().SelectAbility(_allAbilities[_abilityIdx]);
    }

    public void TryUseCurrentAbility()
    {
        if (_allAbilities.Count == 0)
            return;
        
        var ability = _allAbilities[_abilityIdx];

        if (_currentCounts.ContainsKey(ability) == false)
            return;

        if (_currentCounts[ability] <= 0)
            return;

        if (_runManager.PlayerState.HasUnlimited == false)
            _currentCounts[ability]--;
        
        FindFirstObjectByType<AbilitySelectUI>().Refresh(_allAbilities[_abilityIdx], _currentCounts[ability]);
        FindFirstObjectByType<PlayerController>().UseAbility(_allAbilities[_abilityIdx]);

    }
}
