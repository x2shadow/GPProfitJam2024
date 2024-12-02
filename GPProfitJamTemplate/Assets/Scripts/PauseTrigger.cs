using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTrigger : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    bool isPaused;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) Resume(); else Pause();
        }
        
    }

    void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
        pausePanel.SetActive(false);
    }
}
