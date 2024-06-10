using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectUI : MonoBehaviour
{
    [SerializeField] private AbilitySelectItem _template;
    [SerializeField] private AbilityData _abilityData;

    private List<AbilitySelectItem> _spawnedItems = new();
    
    private RunManager _runManager => FindFirstObjectByType<RunManager>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Dictionary<AbilityType, int> abilities)
    {
        foreach ((var key, int level) in abilities)
        {
            AbilitySelectItem newItem = Instantiate(_template, _template.transform.parent);
            newItem.SetAbility(_abilityData.GetAbilityMeta(key), level);
            
            _spawnedItems.Add(newItem);
        }
        
        _template.gameObject.SetActive(false);
        
        if (_spawnedItems.Count > 0)
            _spawnedItems[0].Select();
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
