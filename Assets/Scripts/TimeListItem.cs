using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeDiffText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _activeHighlight;
    
    [Header("Colors")]
    [SerializeField] private Color _colorGood;
    [SerializeField] private Color _colorMid;
    [SerializeField] private Color _colorBad;

    private bool _isActive;
    private float _lastKnownSeconds;
    private bool _isFirstRun;

    private void Start()
    {
        _timeDiffText.text = "";
    }
    
    public void SetFirstRun(bool isFirstRun)
    {
        _isFirstRun = isFirstRun;
        _timeText.text = "-";
        _timeDiffText.text = "";
    }
    
    public void SetHighlightActive(bool isActive)
    {
        _isActive = isActive;
        _activeHighlight.DOFade(isActive ? 1 : 0, 0.3f).SetEase(Ease.OutExpo);
    }

    public void SetBestTime(string bestTimeString)
    {
        if (_isFirstRun)
            return;
        
        _timeText.text = bestTimeString;
    }

    public void LockInTime()
    {
        if (_isFirstRun)
        {
            _timeText.text = GetTimeString(_lastKnownSeconds);
            return;
        }

        SetPlusMinusTime(_lastKnownSeconds);
    }
    
    private string GetTimeString(float totalSeconds)
    {
        int minutes = (int) totalSeconds / 60;
        int seconds = (int) totalSeconds - (minutes * 60);
        int ms = (int)((totalSeconds - (int) totalSeconds) * 10);

        string secondsString = seconds.ToString("D2");
        return $"{minutes}:{secondsString}.{ms}";
    }

    public void SetPlusMinusTime(float seconds)
    {
        _lastKnownSeconds = seconds;
     
        if (_isFirstRun)
            return;
        
        if (_isActive && seconds < -5)
        {
            _timeDiffText.text = "";
            return;
        }
        
        if (seconds > 0)
        {
            _timeDiffText.text = "+" + seconds.ToString("0.0");
        }
        else
        {
            _timeDiffText.text = seconds.ToString("0.0");
        }

        if (_isActive)
        {
            _timeDiffText.color = Color.white;
        }
        else if (seconds < -2)
        {
            _timeDiffText.color = _colorGood;
        }
        else if (seconds < 2)
        {
            _timeDiffText.color = _colorMid;
        }
        else
        {
            _timeDiffText.color = _colorBad;
        }
    }
}
