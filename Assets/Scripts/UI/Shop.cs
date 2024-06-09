using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] private TMP_Text _currencyText;
    [SerializeField] private RectTransform _unlimitedRect;
    [SerializeField] private RectTransform _firstRect;
    
    [SerializeField] private List<ShopItemUI> _shopItems;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private RunManager _runManager => FindFirstObjectByType<RunManager>();
    
    void OnEnable()
    {
        _unlimitedRect.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_firstRect.gameObject);

        foreach (ShopItemUI item in _shopItems)
        {
            item.Refresh(_runManager.PlayerState);
        }
        
        UpdateCurrencyText();

        bool hasUnlimited = _runManager.PlayerState.TrophyPoints > 250 || _runManager.PlayerState.HasUnlimited;
        _unlimitedRect.gameObject.SetActive(hasUnlimited);
    }

    public void UpdateCurrencyText()
    {
        int currency = _runManager.PlayerState.TrophyPoints;
        _currencyText.text = $"You Have {currency} Points";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
