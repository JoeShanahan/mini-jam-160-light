using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrophyPipUI : MonoBehaviour
{
    [SerializeField]
    private Image _templateObject;

    [SerializeField] 
    private RectTransform _trophyLayout;

    [SerializeField] private Color _bronzeColor;
    [SerializeField] private Color _silverColor;
    [SerializeField] private Color _goldColor;
    [SerializeField] private Color _platinumColor;

    private List<Image> _spawnedTrophies = new();
    
    public void InitTrophies(List<TrophyData> allTrophies, List<string> unlockedTrophies)
    {
        foreach (TrophyData dat in allTrophies)
        {
            Image newObj = Instantiate(_templateObject, _templateObject.transform.parent);
            newObj.color = Color.Lerp(GetColor(dat), Color.black, 0.7f);
            _spawnedTrophies.Add(newObj);
        }
        
        _templateObject.gameObject.SetActive(false);
        UpdateVisuals(allTrophies, unlockedTrophies);
    }

    private Color GetColor(TrophyData dat)
    {
        return dat.Rarity switch
        {
            TrophyData.TrophyClass.Bronze => _bronzeColor,
            TrophyData.TrophyClass.Silver => _silverColor,
            TrophyData.TrophyClass.Gold => _goldColor,
            TrophyData.TrophyClass.Platinum => _platinumColor,
            _ => Color.white
        };
    }
    
    public void UpdateVisuals(List<TrophyData> allTrophies, List<string> unlockedTrophies)
    {
        for (int i = 0; i < allTrophies.Count; i++)
        {
            TrophyData dat = allTrophies[i];
            bool isUnlocked = unlockedTrophies.Contains(dat.name);

            Color correctColor = Color.Lerp(GetColor(dat), Color.black, isUnlocked ? 0 : 0.7f);

            if (_spawnedTrophies[i].color != correctColor)
            {
                _spawnedTrophies[i].DOColor(correctColor, 0.5f);
            }
        }
    }
}
