using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        inputReader.PauseEvent  += HandlePause;
        inputReader.ResumeEvent += HandleResume;
    }

    void HandlePause()
    {
        pauseMenu.SetActive(true);
        inputReader.SetUI();  // Удалить, должно быть в InputReader
    }

    public void HandleResume()
    {
        pauseMenu.SetActive(false);
        inputReader.SetGameplay();  // Удалить, должно быть в InputReader
    }
}
