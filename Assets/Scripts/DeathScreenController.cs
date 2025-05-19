using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    public Button defaultDeathScreenButton;

    public void ResetMenu()
    {
        if (defaultDeathScreenButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultDeathScreenButton.gameObject);
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
