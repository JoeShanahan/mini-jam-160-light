using UnityEngine;
using UnityEngine.InputSystem;

public class DebugToggler : MonoBehaviour {

    public string gameCanvasName = "GameCanvas"; 
    public string debugCanvasName = "DebugButtons"; 
    private GameObject debugCanvas;
    private PlayerInput playerInput;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start() {
        GameObject parentCanvas = GameObject.Find(gameCanvasName);
        if (parentCanvas != null) {
            debugCanvas = FindChildRecursive(parentCanvas.transform, debugCanvasName);
        } else {
            Debug.LogError("Parent canvas not found");
        }

        if (debugCanvas == null) {
            Debug.LogError("Debug canvas not found");
        }
    }

    private void PauseMenuToggle(InputAction.CallbackContext context)
    {
        FindFirstObjectByType<PauseMenuController>().EnableMenu();
    }

    private void OnEnable() {
        playerInput.actions["DebugMenuToggle"].performed += DebugMenuToggle;
        playerInput.actions["PauseMenuToggle"].performed += PauseMenuToggle;
    }

    private void OnDisable() {
        playerInput.actions["DebugMenuToggle"].performed -= DebugMenuToggle;
    }

    private void DebugMenuToggle(InputAction.CallbackContext context) {
        if (debugCanvas != null) {
            debugCanvas.SetActive(!debugCanvas.activeSelf);
        }
    }

    private GameObject FindChildRecursive(Transform parent, string childName) {
        foreach (Transform child in parent) {
            if (child.name == childName) {
                return child.gameObject;
            }
            GameObject result = FindChildRecursive(child, childName);
            if (result != null) {
                return result;
            }
        }
        return null;
    }
}