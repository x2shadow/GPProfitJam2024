using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    public void OnLevelButtonClick()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
