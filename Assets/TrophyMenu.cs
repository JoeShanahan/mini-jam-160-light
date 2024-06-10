using System.Collections.Generic;
using UnityEngine;

public class TrophyMenu : MonoBehaviour
{
    [SerializeField] private List<TrophyData> _allTrophies;
    [SerializeField] private TrophyToast _placeholder;

    private List<TrophyToast> _spawnedTrophies = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        PlayerState state = new PlayerState();
        state.LoadFromPrefs();
        
        foreach (TrophyToast toast in _spawnedTrophies)
            Destroy(toast.gameObject);
        
        _spawnedTrophies = new();
        
        foreach (TrophyData dat in _allTrophies)
        {
            bool isEarned = state.UnlockedAchievements != null && state.UnlockedAchievements.Contains(dat.name);
            
            TrophyToast newToast = Instantiate(_placeholder, _placeholder.transform.parent);
            newToast.SetTrophyInList(dat, isEarned);
            newToast.gameObject.SetActive(true);
            _spawnedTrophies.Add(newToast);
        }
        
        _placeholder.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
