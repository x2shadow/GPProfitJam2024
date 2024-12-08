using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject bgmusic;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.isPaused) Resume(); else Pause();
        }
        
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.Instance.isPaused = true;
        pausePanel.SetActive(true);
        gameplayPanel.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPaused = false;
        pausePanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }

    public void StartMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPaused = false;
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
        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Получаем общее количество сцен в Build Settings
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // Определяем индекс следующей сцены
        int nextSceneIndex = currentSceneIndex + 1;

        // Если следующей сцены нет (выход за пределы списка), переходим к первой сцене
        if (nextSceneIndex >= totalScenes)
        {
            nextSceneIndex = 0; // Индекс первой сцены
            Destroy(bgmusic);
        }

        // Загружаем следующую сцену
        SceneManager.LoadScene(nextSceneIndex);
        Time.timeScale = 1;
    }
}
