using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelectItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup _dotsGroup;
    [SerializeField] private Image[] _dots;
    [SerializeField] private Image _abilityImage;

    private RectTransform _rt;
    private bool _isSelected;
    
    public AbilityData.AbilityDataItem Ability { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rt = GetComponent<RectTransform>();

        _isSelected = true;
        Deselect(0);
    }

    public void SetAbility(AbilityData.AbilityDataItem ability)
    {
        _abilityImage.sprite = ability.Icon;
        Ability = ability;
        
        foreach (Image img in _dots)
        {
            img.color = ability.Color;
        }
    }

    public void Deselect(float time = 0.5f)
    {
        if (_isSelected == false)
            return;
        
        _isSelected = false;
        _rt.DOScale(0.5f, time).SetEase(Ease.OutExpo);
        _dotsGroup.DOFade(0, time);
        _abilityImage.DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), time);
    }
    
    public void Select(float time = 0.5f)
    {
        if (_isSelected == true)
            return;

        _isSelected = true;
        _rt.DOScale(1, time).SetEase(Ease.OutExpo);
        _dotsGroup.DOFade(1, time);
        _abilityImage.DOColor(Color.white, time);
    }

    public void Refresh(int count)
    {
        Debug.LogWarning($"{Ability.AbilityType}: is now {count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
