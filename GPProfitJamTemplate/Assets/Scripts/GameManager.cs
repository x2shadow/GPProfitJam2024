using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPaused;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha3)) SceneManager.LoadScene("Level 1");
        if(Input.GetKeyDown(KeyCode.Alpha3)) SceneManager.LoadScene("Level 2");
    }
}
