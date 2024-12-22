using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] GameObject canvasTutorial;
    [SerializeField] GameObject canvasGameplay;

    public void ShowTutorial()
    {
        Time.timeScale = 0;
        canvasTutorial.SetActive(true);
        canvasGameplay.SetActive(false);
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1;
        canvasTutorial.SetActive(false);
        canvasGameplay.SetActive(true);
    }
}
