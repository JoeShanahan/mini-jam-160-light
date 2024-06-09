using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject {
    [Serializable]
    public class AbilityDataItem {
        public string DisplayName;
        public AbilityType AbilityType;
        public Sprite Icon;
        public float Power;
        public float Cooldown;
        public Color Color;
        public int ShopPrice;

        [TextArea(1, 5)] public string Description;
    }

    public IEnumerable<AbilityDataItem> AllAbilities => _abilities;
    [SerializeField] private List<AbilityDataItem> _abilities;

    public float GetPower(AbilityType atype) {
        foreach (var ability in _abilities) {
            if (ability.AbilityType == atype)
                return ability.Power;
        }
        return 0;
    }

    public float GetCooldown(AbilityType atype) {
        foreach (var ability in _abilities) {
            if (ability.AbilityType == atype)
                return ability.Cooldown;
        }
        return 0;
    }
}