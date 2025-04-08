using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public Canvas pauseMenu;
    public bool isPaused;

    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
            isPaused = true;
        }
    }

    public void MainScene()
    {
        SceneManager.LoadScene("main");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

}
