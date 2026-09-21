using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //[SerializeField] private GameObject pauseMenuUI;
    private bool isPauseMenuOpen = false;

    public void TogglePauseMenu()
    {
        if (isPauseMenuOpen)
        {
            gameObject.SetActive(false);
            isPauseMenuOpen = !isPauseMenuOpen;
        }
        else
        {
            gameObject.SetActive(true);
            isPauseMenuOpen = !isPauseMenuOpen;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
