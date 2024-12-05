using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameplayPanel;

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
}
