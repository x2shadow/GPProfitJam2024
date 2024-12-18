using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Mixer : MonoBehaviour, IInteractable
{
    [SerializeField] private int capacity = 3; // Вместимость миксера
    private List<Ingredient> loadedIngredients = new List<Ingredient>(); // Список ингредиентов в миксере
    private bool isMixing = false; // Флаг, идет ли процесс смешивания
    private DishType mixedDishType; // Тип готового блюда
    [SerializeField] private GameObject productPrefab;


    [SerializeField] private float mixingTime = 5f; // Время на смешивание
    private float mixingTimer = 0f;
    
    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mixerSound;
    [SerializeField] AudioClip mixerFilledSound;

    public void Interact(PlayerInteraction player)
    {
        // Если игрок хочет загрузить ингредиенты
        if (player.HasIngredients())
        {
            int availableSlots = capacity - loadedIngredients.Count;
            int ingredientsToLoad = Mathf.Min(player.GetIngredientCount(), availableSlots);

            if (ingredientsToLoad > 0)
            {
                List<Ingredient> ingredientsFromPlayer = player.TakeIngredients(ingredientsToLoad);
                List<Ingredient> ingredientsToRemove = new List<Ingredient>();

                foreach (var ingredient in ingredientsFromPlayer)
                {
                    loadedIngredients.Add(ingredient);
                    ingredientsToRemove.Add(ingredient);
                }

                // Удаляем ингредиенты из trayIngredients и очищаем соответствующие слоты
                foreach (var ingredient in ingredientsToRemove)
                {
                    int index = player.trayIngredients.FindIndex(item => item == ingredient); // Используем лямбда-выражение
                    
                    if (index != -1)
                    {
                        player.trayIngredients.RemoveAt(index);
                    }
                }

                for (int i = 0; i < ingredientsToLoad; i++) player.ClearTraySlot(i);

                Debug.Log($"Загружено {ingredientsToLoad} ингредиентов в миксер. Осталось слотов: {capacity - loadedIngredients.Count}");

                player.hasTray = false;
                player.tray.SetActive(false);

                if (loadedIngredients.Count == capacity)
                {
                    StartMixing();
                }
            }
            else
            {
                Debug.LogWarning("Недостаточно слотов в миксере для загрузки ингредиентов.");
            }
        }
        // Если игрок хочет забрать готовое блюдо
        else if (mixedDishType != DishType.None)
        {
            if (player.CanTakeDish())
            {
                player.TakeDish(mixedDishType, productPrefab);
                mixedDishType = DishType.None;
                Debug.Log("Готовое блюдо забрано из миксера.");
            }
            else
            {
                Debug.LogWarning("У игрока нет места на подносе для готового блюда.");
            }
        }
        else Debug.Log("У вас ничего нет");
    }

    private void StartMixing()
    {
        if (!isMixing && loadedIngredients.Count == capacity)
        {
            isMixing = true;
            mixingTimer = mixingTime;
            Debug.Log("Начинаем смешивать ингредиенты...");
        }
    }

    private void Update()
    {
        if (isMixing)
        {
            mixingTimer -= Time.deltaTime;
            if (mixingTimer <= 0f)
            {
                CompleteMixing();
            }
        }
    }

    private void CompleteMixing()
    {
        isMixing = false;
        mixedDishType = Dish.CreateDishFromIngredients(loadedIngredients); // Определяем тип блюда
        loadedIngredients.Clear(); // Очищаем миксер
        Debug.Log($"Смешивание завершено. Готовое блюдо: {mixedDishType}.");
    }

    public string GetInteractionHint()
    {
        if (mixedDishType != DishType.None)
        {
            return "Нажмите Interact, чтобы забрать готовое блюдо.";
        }
        else if (loadedIngredients.Count < capacity)
        {
            return "Нажмите Interact, чтобы загрузить ингредиенты.";
        }
        else
        {
            return "Миксер полон и идет смешивание.";
        }
    }
}



