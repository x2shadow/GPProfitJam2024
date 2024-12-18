using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mixer : MonoBehaviour, IInteractable
{
    [SerializeField] private int capacity = 3; // Вместимость миксера
    private List<Ingredient> loadedIngredients = new List<Ingredient>(); // Список ингредиентов в миксере
    private bool isMixing = false; // Флаг, идет ли процесс смешивания
    private DishType mixedDishType; // Тип готового блюда
    [SerializeField] private GameObject productPrefab;


    [SerializeField] private float mixingTime = 2f; // Время на смешивание
    
    [Header("UI")]
    public Slider mixerSlider;
    [SerializeField] private Image[] ingredientSlots; // Слоты для ингредиентов
    [SerializeField] private Image   productSlot; // Слоты для ингредиентов
    [SerializeField] private Sprite  plusIcon; // Иконка для пустого слота
    [SerializeField] private Sprite  flourIcon; // Иконка муки
    [SerializeField] private Sprite  chocolateIcon; // Иконка яйца
    [SerializeField] private Sprite  milkIcon; // Иконка молока
    [SerializeField] private Sprite  noIcon; // Иконка молока
    [SerializeField] private Sprite  cookieIcon; // Иконка молока


    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mixerSound;
    [SerializeField] AudioClip mixerFilledSound;

    public void Interact(PlayerInteraction player)
    {
        TrayManager tray = player.GetComponent<TrayManager>();
        if (tray != null && tray.HasIngredients())
        {
            AddIngredientsFromTray(tray);
        }
        // Если игрок хочет забрать готовое блюдо
        else if (mixedDishType != DishType.None)
        {
            if (player.currentDishType == DishType.None)
            {
                player.trayManager.UpdateTray(player, true, mixedDishType, productPrefab, false);
                Debug.Log($"Игрок забрал блюдо: {mixedDishType}");
                mixedDishType = DishType.None;
                ClearSlots();
                productSlot.sprite = noIcon;
                Debug.Log("Готовое блюдо забрано из миксера.");
                SoundManager.Instance.PlaySound("AddedToMixer");
            }
            else
            {
                Debug.LogWarning("У игрока нет места на подносе для готового блюда.");
            }
        }
    }

    private void AddIngredientsFromTray(TrayManager tray)
    {
        int availableSlots = capacity - loadedIngredients.Count;
        int ingredientsToLoad = Mathf.Min(tray.GetIngredientCount(), availableSlots);

        if (ingredientsToLoad > 0)
        {
            List<Ingredient> ingredients = tray.TakeIngredients(ingredientsToLoad);
            foreach (var ingredient in ingredients)
            {
                loadedIngredients.Add(ingredient);
            }
            UpdateIngredientSlots();

            if (loadedIngredients.Count == capacity)
            {
                StartMixing();
            }
        }
        else
        {
            Debug.LogWarning("Недостаточно слотов в миксере.");
        }
    }

    private void UpdateIngredientSlots()
    {
        for (int i = 0; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].sprite = i < loadedIngredients.Count
                ? GetIngredientIcon(loadedIngredients[i])
                : plusIcon;
        }
    }

    private Sprite GetIngredientIcon(Ingredient ingredient)
    {
        return ingredient switch
        {
            Ingredient.Flour => flourIcon,
            Ingredient.Chocolate => chocolateIcon,
            Ingredient.Milk => milkIcon,
            _ => plusIcon,
        };
    }

    private void ClearSlots()
    {
        foreach (var slot in ingredientSlots)
        {
            slot.sprite = plusIcon;
        }
    }

    private void StartMixing()
    {
        if (!isMixing && loadedIngredients.Count == capacity)
        {
            isMixing = true;
            mixerSlider.gameObject.SetActive(true);
            ingredientSlots[0].sprite = noIcon; 
            ingredientSlots[1].sprite = noIcon; 
            ingredientSlots[2].sprite = noIcon; 
            StartCoroutine(MixingProcess());
            SoundManager.Instance.PlaySound("MixingSound");
            Debug.Log("Начинаем смешивать ингредиенты...");
        }
    }

    void Start()
    {
        mixerSlider.gameObject.SetActive(false);  // Скрыть слайдер при старте
        mixerSlider.maxValue = mixingTime;
    }

    private void Update()
    {

    }

    private IEnumerator MixingProcess()
    {
        float elapsedTime = 0f;

        while (elapsedTime < mixingTime)
        {
            elapsedTime += Time.deltaTime;
            mixerSlider.value = elapsedTime;
            yield return null; // Ждем до следующего кадра
        }

        CompleteMixing();
    }

    private void CompleteMixing()
    {
        isMixing = false;
        mixerSlider.gameObject.SetActive(false);
        mixedDishType = Dish.CreateDishFromIngredients(loadedIngredients); // Определяем тип блюда
        loadedIngredients.Clear(); // Очищаем миксер
        productSlot.sprite = cookieIcon;
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



