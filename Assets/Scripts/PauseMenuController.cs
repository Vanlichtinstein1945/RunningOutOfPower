using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject controlsPanel;

    public Button defaultControlsButton;
    public Button defaultPauseMenuButton;

    public InputAction cancelAction;

    private bool isControlsVisible;

    private void OnEnable()
    {
        cancelAction.Enable();
    }

    private void OnDisable()
    {
        cancelAction.Disable();
    }

    private void Update()
    {
        if (GameManager.instance.isPaused && cancelAction.WasPerformedThisFrame())
        {
            HandleBackButton();
        }
    }

    public void Resume()
    {
        GameManager.instance.TogglePause();
    }

    public void ResetMenu()
    {
        isControlsVisible = false;
        pauseMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);

        if (defaultControlsButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultPauseMenuButton.gameObject);
        }
    }

    public void ToggleControls()
    {
        isControlsVisible = !isControlsVisible;
        pauseMenuPanel.SetActive(!isControlsVisible);
        controlsPanel.SetActive(isControlsVisible);

        if (isControlsVisible && defaultControlsButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultControlsButton.gameObject);
        }
        else if (defaultPauseMenuButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultPauseMenuButton.gameObject);
        }
    }

    private void HandleBackButton()
    {
        if (isControlsVisible)
        {
            ToggleControls();
        }
        else
        {
            Resume();
        }
    }

    public void Restart()
    {
        GameManager.instance.ReloadCurrentLevel();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
