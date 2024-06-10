using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private AbilityType _abilityType;
    [SerializeField] private AbilityData _abilityData;
    
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _sepText;
    [SerializeField] private TMP_Text _pointText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private List<Image> _bgCircles;
    [SerializeField] private List<Image> _middleCircles;

    [SerializeField] private Color _unfilledUnlimitedColor;
    [SerializeField] private Color _filledUnlimitedColor;
    
    private RunManager _runManager => FindFirstObjectByType<RunManager>();
    
    public void Refresh(PlayerState state)
    {
        int numberUnlocked = 0;
        Color unfilledColor = Color.black;
        Color filledColor = Color.white;
        
        if (_abilityType == AbilityType.None)
        {
            numberUnlocked = state.HasUnlimited ? 1 : 0;
            filledColor = _filledUnlimitedColor;
            unfilledColor = _unfilledUnlimitedColor;
        }
        else
        {
            var abilityMeta = AbilityMeta;
            filledColor = abilityMeta.Color;
            unfilledColor = Color.Lerp(filledColor, Color.black, 0.65f);
            
            numberUnlocked = state.GetAbilityLevel(_abilityType);
        }
        
        for (int i = 0; i < _middleCircles.Count; i++)
        {
            _middleCircles[i].color = i < numberUnlocked ? filledColor : unfilledColor;
        }
    }

    private AbilityData.AbilityDataItem AbilityMeta
    {
        get
        {
            foreach (var adata in _abilityData.AllAbilities)
            {
                if (adata.AbilityType == _abilityType)
                {
                    return adata;
                }
            }

            return null;
        }
    }

    private void Start()
    {
        AbilityData.AbilityDataItem adata = AbilityMeta;
        
        if (adata == null)
            return;
        

        Color lightColor = Color.Lerp(adata.Color, Color.white, 0.4f);
        Color darkColor = Color.Lerp(adata.Color, Color.black, 0.4f);
        
        _nameText.text = adata.DisplayName;
        _pointText.text = $"{adata.ShopPrice} points";
        _descriptionText.text = adata.Description;

        _nameText.color = lightColor;
        _descriptionText.color = lightColor;
            
        _pointText.color = darkColor;
        _sepText.color = darkColor;

        foreach (Image img in _bgCircles)
        {
            img.color = darkColor;
        }

        foreach (Image img in _middleCircles)
        {
            img.color = lightColor;
        }
        
        Refresh(_runManager.PlayerState);
    }
    
    public void OnButtonPress()
    {
        int points = _runManager.PlayerState.TrophyPoints;
        var meta = AbilityMeta;

        int currentLevel = _runManager.PlayerState.GetAbilityLevel(_abilityType);

        if (currentLevel >= 3)
            return;

        if (points < meta.ShopPrice)
            return;

        _runManager.PlayerState.IncrementAbilityLevel(_abilityType, meta.ShopPrice);
        Refresh(_runManager.PlayerState);
        FindFirstObjectByType<Shop>().UpdateCurrencyText();
    }

    public void OnButtonPressUnlimited()
    {
        int points = _runManager.PlayerState.TrophyPoints;

        if (_runManager.PlayerState.HasUnlimited)
            return;

        if (points < 260)
            return;

        _runManager.PlayerState.HasUnlimited = true;
        _runManager.PlayerState.TrophyPoints -= 260;
        _runManager.PlayerState.SaveToPrefs();
        Refresh(_runManager.PlayerState);
        FindFirstObjectByType<Shop>().UpdateCurrencyText();
    }
}
