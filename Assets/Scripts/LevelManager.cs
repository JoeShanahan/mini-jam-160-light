using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private RunManager _runManager;
    [SerializeField] private List<LevelContainer> _levelPrefabs;
    [SerializeField] private PlayerController _player;

    private LevelContainer _currentLevel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnNewLevelReached(0);
    }

    public void OnNewLevelReached(int levelNumber)
    {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        int levelNum = Mathf.Clamp(levelNumber, 0, _levelPrefabs.Count - 1);
        
        _currentLevel = Instantiate(_levelPrefabs[levelNum]);
        _player.SetSpawnPosition(_currentLevel.SpawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
