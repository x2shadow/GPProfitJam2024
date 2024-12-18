using UnityEngine;

public class Mixer : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerInteraction player;

    public bool isReadyToMix = false;
    public bool IsMixed = false;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mixerSound;
    [SerializeField] AudioClip mixerFilledSound;

    public void Interact(PlayerInteraction player)
    {
        if (!isReadyToMix && player.currentIngredient != Ingredient.None) // Добавить в миксер
        {
            AddIngredientToMixer(player.currentIngredient);
            player.currentIngredient = Ingredient.None;
        }
        else if (isReadyToMix)
        {
            // Смешать
            TryMix();
        }
        else
        {
            Debug.Log("У вас ничего нет.");
        }
    }
    
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
        player.hasMixedProduct = true;
        isReadyToMix = false;
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

    public string GetInteractionHint()
    {
        return "Добавляй или миксуй";
    }
}
