using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameplayPanel;

    void Start()
    {
        GameStateManager.Instance.OnGamePaused += OnGamePaused;
        GameStateManager.Instance.OnGameResumed += OnGameResumed;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGamePaused -= OnGamePaused;
        GameStateManager.Instance.OnGameResumed -= OnGameResumed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.isPaused) GameStateManager.Instance.ResumeGame();
            else GameStateManager.Instance.PauseGame();
        }
    }

    public void Resume()
    {
        GameStateManager.Instance.ResumeGame();
    }

    public void Pause()
    {
        GameStateManager.Instance.PauseGame();
    }

    public void OnGamePaused()
    {
        pausePanel.SetActive(true);
        gameplayPanel.SetActive(false);
    }

    public void OnGameResumed()
    {
        pausePanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }

    public void StartMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= totalScenes)
        {
            nextSceneIndex = 0; // Индекс первой сцены
        }

        SceneManager.LoadScene(nextSceneIndex);
        Time.timeScale = 1;
    }
}
