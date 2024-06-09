using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectUI : MonoBehaviour
{
    [SerializeField] private AbilitySelectItem _template;
    [SerializeField] private AbilityData _abilityData;

    private List<AbilitySelectItem> _spawnedItems = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (AbilityData.AbilityDataItem item in _abilityData.AllAbilities)
        {
            AbilitySelectItem newItem = Instantiate(_template, _template.transform.parent);
            newItem.SetAbility(item);
            
            _spawnedItems.Add(newItem);
        }
        
        _template.gameObject.SetActive(false);
    }

    public void Refresh(AbilityType ability, int amount)
    {
        foreach (var item in _spawnedItems)
        {
            if (item.Ability.AbilityType == ability)
            {
                item.Refresh(amount);
            }
        }
    }

    public void SelectAbility(AbilityType ability)
    {
        foreach (var item in _spawnedItems)
        {
            if (item.Ability.AbilityType == ability)
            {
                item.Select();
            }
            else
            {
                item.Deselect();
            }
        }
    }
}
