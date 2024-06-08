using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void InitTrophies(List<TrophyData> allTrophies)
    {
        foreach (TrophyData dat in allTrophies)
        {
            Image newObj = Instantiate(_templateObject, _templateObject.transform.parent);
            newObj.color = Color.Lerp(GetColor(dat), Color.black, 0.7f);
        }
        
        _templateObject.gameObject.SetActive(false);
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
        
    }
}
