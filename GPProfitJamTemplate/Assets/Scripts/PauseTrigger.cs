using UnityEngine;
using UnityEngine.InputSystem;

public class PauseTrigger : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    bool isPaused;

    public InputAction move;
    public InputAction jump;
    public InputAction pause;

    void Awake()
    {
        pause.performed += context => { OnPause(context); };
    }

    void Update()
    {
    }

    void OnPause(InputAction.CallbackContext context)
    {
        if(isPaused) Resume(); else if(!isPaused) Pause();
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

    void OnEnable()
    {
        pause.Enable();
    }

    void OnDisable()
    {
        pause.Disable();
    }
}
