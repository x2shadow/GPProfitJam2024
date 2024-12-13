using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip startMenuMusic;
    [SerializeField] private AudioClip level1Music;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null)
            return;

        // Включаем музыку для сцены StartMenu
        if (scene.name == "StartMenu")
        {
            PlayMusic(startMenuMusic);
        }
        // Включаем музыку для Level1 и всех остальных уровней
        else if (scene.name.StartsWith("Level"))
        {
            PlayMusic(level1Music);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
