using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform _actualMenu;

    public bool IsCurrentlyOnScreen => _actualMenu.gameObject.activeSelf;
    public void EnableMenu()
    {
        _actualMenu.gameObject.SetActive(true);
    }
    
    public void ButtonPressContinue()
    {
        _actualMenu.gameObject.SetActive(false);
    }

    public void ButtonPressRestart()
    {
        FindFirstObjectByType<PlayerController>().Die();
        _actualMenu.gameObject.SetActive(false);
        // FindFirstObjectByType<RunManager>().RevertTimeToStart();
    }

    public void ButtonPressRestartRun()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonPressMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    [SerializeField] private RectTransform _firstButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
    }
}
