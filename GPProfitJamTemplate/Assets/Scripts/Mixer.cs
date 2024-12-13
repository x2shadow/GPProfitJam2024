using UnityEngine;

public class Mixer : MonoBehaviour
{
    [SerializeField] Oven oven;

    public bool isReadyToMix = false;
    public bool IsMixed = false;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mixerSound;
    [SerializeField] AudioClip mixerFilledSound;

    public void TryMix()
    {
        if (!OrderManager.Instance.IsOrderComplete())
        {
            Debug.Log("Ингредиенты ещё не добавлены.");
            return;
        }

        // Проигрывание звука
        if (audioSource != null && mixerSound != null)
        {
            audioSource.PlayOneShot(mixerSound);
        }

        Debug.Log("Ингредиенты смешаны. Блюдо готово!");
        IsMixed = true;
        isReadyToMix = false;

        // Событие готовности к запеканию
        oven.hasMixedProduct = true;
    }

    public void AddIngredientToMixer(Ingredient ingredient)
    {
        foreach (var orderIngredient in OrderManager.Instance.currentOrderIngredients)
        {
            if (orderIngredient.ingredient == ingredient && !orderIngredient.isAdded)
            {
                orderIngredient.isAdded = true;
                Debug.Log($"Добавлен ингредиент {ingredient}");

                // Проигрывание звука
                if (audioSource != null && mixerFilledSound != null)
                {
                    audioSource.PlayOneShot(mixerFilledSound);
                }

                OrderUIManager.Instance.UpdateIngredients(OrderManager.Instance.currentOrderIngredients);
                CheckIfOrderCompleted();
                return;
            }
        }

        Debug.LogWarning("Этот ингредиент не нужен для текущего заказа!");
    }

    void CheckIfOrderCompleted()
    {
        if(OrderManager.Instance.IsOrderComplete())
        {
            Debug.Log("Все ингредиенты добавлены. Можно смешивать!");
            isReadyToMix = true;
        }
    }

    public void ResetMixer()
    {
        IsMixed = false;
        Debug.Log("Миксер очищен.");
    }
}
