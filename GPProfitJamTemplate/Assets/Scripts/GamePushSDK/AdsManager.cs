using UnityEngine;
using GamePush;

public class AdsManager : MonoBehaviour
{
    // Подпишитесь на событие GP_Ads.OnRewardedReward;
    private void OnEnable()
    {
        GP_Ads.OnRewardedReward += OnRewarded;
    }

    private void OnDisable()
    {
        GP_Ads.OnRewardedReward -= OnRewarded;
    }

    // При вызове метода можно указать любое текстовое значение
    // Например: COINS или GEMS
    private void ShowRewarded(string idOrTag)
    {
        GP_Ads.ShowRewarded(idOrTag);
    }

    // При успешном просмотре Reward рекламы
    // можно давать награду проверяя указанное значение:
    private void OnRewarded(string idOrTag)
    {
        if (idOrTag == "COINS")
            Debug.Log("Получено 250 монет!");

        if (idOrTag == "GEMS")
            Debug.Log("Получено 15 драгоценностей!");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) ShowRewarded("COINS");    
    }
}
