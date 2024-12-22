using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name; // Название звука
        public AudioClip clip; // Сам аудиофайл
        public float volume = 1.0f; // Громкость
    }

    public Sound[] sounds; // Список звуков, который можно задавать в инспекторе
    public AudioSource audioSource;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null && sound.clip != null)
        {
            audioSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found!");
        }
    }
}
