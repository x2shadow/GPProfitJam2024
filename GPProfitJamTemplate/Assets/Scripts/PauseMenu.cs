using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    public void ButtonResume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void ButtonStartMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }
}
