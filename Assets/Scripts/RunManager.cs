using System;
using System.Collections;
using System.Collections.Generic;
using LudumDare55;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    [SerializeField] private int _worldCount = 3;
    [SerializeField] private RunState _runState;
    [SerializeField] private PlayerState _playerState = new();
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private TrophyManager _trophyManager;

    public RunState RunState => _runState;
    public PlayerState PlayerState => _playerState;
    
    private bool _isActive = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<MusicController>().SwapToGameMusic();
        Application.targetFrameRate = 60;
        _playerState.LoadFromPrefs();

        _runState.CurrentTimes = new float[_worldCount];
        _runState.BestTimes = new float[_worldCount];
        _runState.RunTime = 0;
        _runState.CurrentWorld = 0;

        for (int i = 0; i < _worldCount; i++)
        {
            if (_playerState.BestRunTimes.Count > i)
            {
                _runState.BestTimes[i] = _playerState.BestRunTimes[i] / 10f;
            }
        }
        
        _gameUI.OnWorldChange(0, _playerState);
        _trophyManager.InitTrophies(_playerState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
            IncrementTimes();
        
        _gameUI.UpdateTimes(_runState);
    }

    private void IncrementTimes()
    {
        if (_runState.CurrentWorld < _runState.CurrentTimes.Length)
        {
            _runState.RunTime += Time.deltaTime;
            _runState.CurrentTimes[_runState.CurrentWorld] += Time.deltaTime;
        }
    }

    public void ResetRun()
    {
        Start();
    }
    
    public void OnLevelComplete()
    {
        _runState.CurrentWorld ++;
        _gameUI.OnWorldChange(_runState.CurrentWorld, _playerState);
        _trophyManager.CheckForTrophies(_runState, _playerState);

        StartCoroutine(LevelCompleteRoutine());
    }

    private IEnumerator LevelCompleteRoutine()
    {
        FindFirstObjectByType<Porthole>()?.ClosePorthole();
        FindFirstObjectByType<PlayerController>()?.SetRigidBodyKinematic(true);

        yield return new WaitForSeconds(1.3f);
        FindFirstObjectByType<PlayerController>()?.SetRigidBodyKinematic(false);

        _levelManager.OnNewLevelReached(_runState.CurrentWorld);
        
        if (_runState.CurrentWorld == _worldCount)
            OnGameComplete();
        else
            FindFirstObjectByType<Porthole>()?.OpenPorthole();
    }

    private bool _isGameComplete;

    public void OnGameComplete()
    {
        if (_isGameComplete)
            return;

        _isGameComplete = true;
        
        _gameUI.OnGameComplete(_runState, _playerState);

        bool isNewRecord = true;

        if (_playerState.BestOverallTime > 0)
        {
            float previousBest = _playerState.BestOverallTime / 10f;
            isNewRecord = _playerState.BestOverallTime == 0 || previousBest > _runState.RunTime;

            if (isNewRecord)
            {
                Debug.Log($"New overall record! {_runState.RunTime} < {previousBest}");
            }
        }

        if (isNewRecord)
        {
            _playerState.BestRunTimes = new List<int>();
            _playerState.BestOverallTime = (int)(_runState.RunTime * 10);
            
            foreach (float f in _runState.CurrentTimes)
            {
                _playerState.BestRunTimes.Add((int) (f * 10));
            }
        }

        List<int> bestSectionTimes = new();
        
        while (_playerState.BestSectionTimes.Count < 3)
            _playerState.BestSectionTimes.Add(0);

        for (int i = 0; i < 3; i++)
        {
            float oldTime = _playerState.BestSectionTimes[i] / 10f;
            float newTime = _runState.CurrentTimes[i];

            if (_playerState.BestSectionTimes[i] == 0 || newTime < oldTime)
            {
                Debug.Log($"World {i+1} has a new record! ({newTime} < {oldTime}");
                bestSectionTimes.Add((int) (newTime * 10));
            }
            else
            {
                Debug.Log($"World {i+1} does not have a record! ({newTime} > {oldTime}");
                bestSectionTimes.Add(_playerState.BestSectionTimes[i]);
            }
        }

        _playerState.BestSectionTimes = bestSectionTimes;
        _playerState.SaveToPrefs();
    }
    
    public void SavePlayerProgress()
    {
        Debug.Log(_playerState.BestOverallTime);
        _playerState.SaveToPrefs();
    }
    
    public void WipePlayerProgress()
    {
        _playerState.Wipe();
    }
    
}
