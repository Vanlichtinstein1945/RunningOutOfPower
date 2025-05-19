using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool isPaused { get; private set; }
    public bool playerIsDead { get; private set; }

    public InputAction pauseAction;

    private GameObject pauseMenu;
    private GameObject deathMenu;

    private void OnEnable()
    {
        instance = this;
        pauseAction.Enable();
        pauseAction.performed += ctx => TogglePause();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        pauseAction.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        playerIsDead = false;
        pauseMenu = FindAnyObjectByType<PauseMenuController>().gameObject;
        deathMenu = FindAnyObjectByType<DeathScreenController>().gameObject;
        pauseMenu.SetActive(isPaused);
        deathMenu.SetActive(playerIsDead);
    }

    public void TogglePause()
    {
        if (playerIsDead)
        {
            return;
        }

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        pauseMenu.GetComponent<PauseMenuController>().ResetMenu();

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void ToggleDeathScreen()
    {
        playerIsDead = !playerIsDead;
        deathMenu.SetActive(playerIsDead);
        deathMenu.GetComponent<DeathScreenController>().ResetMenu();

        if (playerIsDead)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
