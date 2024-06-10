using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _timeText;
    [SerializeField] private TMP_Text _personalBestText;
    [SerializeField] private TMP_Text _sumOfBestText;
    [SerializeField] private List<TimeListItem> _listItems;
    [SerializeField] private RectTransform[] _firstTimeDisable;
    
    public void UpdateTimes(RunState runState)
    {
        SetMainTimeString(runState.RunTime);

        for (int i = 0; i < _listItems.Count; i++)
        {
            if (i != runState.CurrentWorld)
                continue;

            float bestTime = runState.BestTimes[i];
            _listItems[i].SetPlusMinusTime(runState.RunTime - bestTime);
        }
    }

    public void OnWorldChange(int newWorld, PlayerState playerState)
    {
        if (newWorld == 0)
        {
            if (playerState.BestOverallTime == 0)
            {
                foreach (RectTransform rect in _firstTimeDisable)
                    rect.gameObject.SetActive(false);
            }
        }
        
        for (int i = 0; i < _listItems.Count; i++)
        {
            _listItems[i].SetHighlightActive(i == newWorld);

            if (newWorld == 0 && playerState.BestOverallTime == 0)
                _listItems[i].SetFirstRun(true);

            
            if (i == newWorld - 1)
                _listItems[i].LockInTime();
            
            float bestTime = 0f;

            if (i < playerState.BestRunTimes.Count)
            {
                bestTime = playerState.BestRunTimes[i] / 10f;
            }

            _listItems[i].SetBestTime(GetTimeString(bestTime));
        }

        int sumOfBest = 0;

        foreach (int val in playerState.BestSectionTimes)
        {
            sumOfBest += val;
        }

        _sumOfBestText.text = GetTimeString(sumOfBest / 10f);
        _personalBestText.text = GetTimeString(playerState.BestOverallTime / 10f);
    }

    private string GetTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        string secondsString = seconds.ToString("D2");
        return $"{minutes}:{secondsString}.{ms}";
    }

    // Yes this is stupid but it allows us to use a non-monospace font without it jittering
    private void SetMainTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        _timeText[0].text = minutes.ToString();
        
        _timeText[2].text = (seconds / 10).ToString();
        _timeText[3].text = (seconds % 10).ToString();

        _timeText[5].text = ms.ToString();
    }
}
