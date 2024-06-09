using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
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

    private int _currentLevelIdx;

    public void OnNewLevelReached(int levelNumber)
    {
        _currentLevelIdx = levelNumber;
        
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        int levelNum = Mathf.Clamp(levelNumber, 0, _levelPrefabs.Count - 1);

        StreamerCam.NotifyStreamer(StreamerEvent.LevelComplete);

        _currentLevel = Instantiate(_levelPrefabs[levelNum]);
        _player.SetSpawnPosition(_currentLevel.SpawnPosition);
        FindFirstObjectByType<ClampedPlayerFollow>()?.SetBounds(_currentLevel);
        StartCoroutine(SnapCamRoutine());
    }

    public void RespawnSameLevel()
    {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        _currentLevel = Instantiate(_levelPrefabs[_currentLevelIdx]);
        StartCoroutine(SnapCamRoutine());
    }

    private IEnumerator SnapCamRoutine()
    {
        var cam = FindFirstObjectByType<CinemachinePositionComposer>();
        Vector3 damping = cam.Damping;
        cam.Lookahead.Enabled = false;
        cam.Damping = Vector3.zero;
        yield return null;
        cam.Lookahead.Enabled = true;
        cam.Damping = damping;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
