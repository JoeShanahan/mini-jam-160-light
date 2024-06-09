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

    private void Start()
    {
        foreach (var adata in _abilityData.AllAbilities)
        {
            if (adata.AbilityType == _abilityType)
            {
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
                
                break;
            }
        }
    }
    
    public void OnButtonPress()
    {
        
    }
}
