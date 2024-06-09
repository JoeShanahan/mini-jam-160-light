using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrophyToast : MonoBehaviour
{
    [Header("Timings")]
    [SerializeField] private float _inactiveX;
    [SerializeField] private float _activeX;
    [SerializeField] private float _transitionTime;
    [SerializeField] private float _visibleTime;

    [Header("UI Elements")] 
    [SerializeField] private Image _trophyBg;
    [SerializeField] private Image _trophyFg;
    [SerializeField] private Image _trophySprite;
    [SerializeField] private TMP_Text _topText;
    [SerializeField] private TMP_Text _bottomText;

    [Header("Colors")] 
    [SerializeField] private Color _bronzeColor;
    [SerializeField] private Color _silverColor;
    [SerializeField] private Color _goldColor;
    [SerializeField] private Color _platinumColor;
    
    private Queue<TrophyData> _toShowList = new();
    private RectTransform _rt;
    
    public void ShowTrophy(TrophyData dat)
    {
        _toShowList.Enqueue(dat);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _rt.DOAnchorPosX(_inactiveX, 0);
        StartCoroutine(TrophyShowRoutine());
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

    private IEnumerator TrophyShowRoutine()
    {
        while (true)
        {
            while (_toShowList.Count == 0)
                yield return null;

            TrophyData newTrophy = _toShowList.Dequeue();
            
            _trophySprite.color = GetColor(newTrophy);
            _trophyFg.color = Color.Lerp(GetColor(newTrophy), Color.black, 0.2f);
            _trophyBg.color = Color.Lerp(GetColor(newTrophy), Color.white, 0.2f);

            _topText.text = newTrophy.DisplayName;
            _bottomText.text = newTrophy.Description;
            
            _rt.DOAnchorPosX(_activeX, _transitionTime).SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(_visibleTime + _transitionTime);
            
            _rt.DOAnchorPosX(_inactiveX, _transitionTime).SetEase(Ease.InSine);

            yield return new WaitForSeconds(_transitionTime);
        }
    }
}
