using UnityEngine;
using GamePush;

public class TestScript : MonoBehaviour
{
    private async void Start()
    {
        // Проверяем, готов ли плагин
        await GP_Init.Ready;
        OnPluginReady();
    }

    private void OnPluginReady()
    {
        Debug.Log("Plugin ready");
    }
}
