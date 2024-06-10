using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void PressPlay()
    {
        SceneManager.LoadScene(1);
    }

    private int wipeCount = 4;
    
    public void PressWipe()
    {
        wipeCount--;

        if (wipeCount > 0)
        {
            _wipeText.text = $"press {wipeCount} times to wipe";
        }

        if (wipeCount == 0)
        {
            _wipeText.text = "Data has been wiped!";
            if (PlayerPrefs.HasKey("PlayerState"))
            {
                PlayerPrefs.DeleteKey("PlayerState");
                PlayerPrefs.Save();
            }
        }

        if (wipeCount < -5)
        {
            _wipeText.text = "why are you still pressing?!";
        }
        
        if (wipeCount < -15)
        {
            _wipeText.text = "This is not part of the game I swear";
        }
        
                
        if (wipeCount < -25)
        {
            _wipeText.text = "Please play the game instead";
        }
        
        if (wipeCount < -35)
        {
            _wipeText.text = "If you keep pressing this I'm just gonna take you to the game";
        }

        if (wipeCount < -45)
        {
            PressPlay();
        }
    }

    public void PressQuit()
    {
        Application.Quit();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            _quitButton.gameObject.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(_playButton.gameObject);
    }

    [SerializeField] private TMP_Text _wipeText;
    [SerializeField] private RectTransform _playButton;
    [SerializeField] private RectTransform _quitButton;
}
