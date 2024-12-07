using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Client> clients = new List<Client>();
    public float levelTime = 120f; // Время на уровень в секундах
    private float timer;

    public TextMeshProUGUI timerText; // UI элемент для отображения времени
    public GameObject winPanel; // Панель победы
    public GameObject losePanel; // Панель проигрыша

    bool isLevelCompleted;

    void Start()
    {
        timer = levelTime;
        UpdateTimerText();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void Update()
    {
        if (isLevelCompleted)
            return;

        timer -= Time.deltaTime;
        UpdateTimerText();

        if (timer <= 0)
        {
            EndLevel(false);
        }
    }

    public void CompleteLevel()
    {
        if (!isLevelCompleted)
        {
            EndLevel(true);
        }
    }

    private void EndLevel(bool isWin)
    {
        isLevelCompleted = true;

        if (isWin)
        {
            winPanel.SetActive(true);
            timerText.text = "Победа скибидоп ес ес";
            Debug.Log("Уровень завершён!");
        }
        else
        {
            losePanel.SetActive(true);
            timerText.text = "GG XD";
            Debug.Log("Время вышло!");
        }

        Time.timeScale = 0; // Останавливаем время
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"Время: {minutes:00}:{seconds:00}";
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1; // Возобновляем время
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
