using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool isPaused { get; private set; }
    public bool isLevelCompleted { get; private set; }

    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action OnLevelCompleted;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Dish.LoadResources();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        OnGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        OnGameResumed?.Invoke();
    }

    public void CompleteLevel()
    {
        isLevelCompleted = true;
        OnLevelCompleted?.Invoke();
    }
}

