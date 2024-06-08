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

    // Update is called once per frame
    void Update()
    {
        
    }
}
