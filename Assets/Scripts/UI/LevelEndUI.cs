using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private List<LevelEndSummaryItem> _listItems;
    [SerializeField] private RectTransform _shop;
    
    [SerializeField] private TMP_Text _thisRunTime;
    [SerializeField] private TMP_Text _lastTimeText;
    
    public void OnGameComplete(RunState run, PlayerState player)
    {
        gameObject.SetActive(true);
        
        for (int i = 0; i < 3; i++)
        {
            float levelTime = run.CurrentTimes[i];
            float bestTime = run.BestTimes[i];
            
            _listItems[i].SetTimes(levelTime, bestTime);
        }

        float bestPrevious = player.BestOverallTime / 10f;
        float current = run.RunTime;

        _thisRunTime.text = GetTimeString(current);
        
        if (player.BestOverallTime == 0)
        {
            _lastTimeText.text = "Great job! See if you can do it faster with upgrades!";
        }
        else if (current < bestPrevious)
        {
            string secondText = (bestPrevious - current).ToString("0.0");
            _lastTimeText.text = $"(that's {secondText}s faster than your previous best!)";
        }
        else
        {
            string secondText = (current - bestPrevious).ToString("0.0");
            _lastTimeText.text = $"(you were {secondText}s slower than your personal best)";
        }
    }

    public void ButtonPressAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonPressShop()
    {
        _shop.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ButtonPressMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    private string GetTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        string secondsString = seconds.ToString("D2");
        return $"{minutes}:{secondsString}.{ms}";
    }
}
