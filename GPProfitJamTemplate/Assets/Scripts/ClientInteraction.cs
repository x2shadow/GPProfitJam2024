using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Для работы с UI

public class ClientInteraction : MonoBehaviour
{
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    private bool playerInRange = false;  // Проверка, находится ли игрок в зоне взаимодействия
    [SerializeField] Client client;
    public List<OrderIngredient> currentOrderIngredients = new List<OrderIngredient>();
    public bool isReadyToMix = false;

    void Start()
    {
        // Изначально кнопка скрыта
        interactionButton.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Используем E для взаимодействия
        {
            TakeOrder(); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionButton.SetActive(true);  // Показываем кнопку, когда игрок в зоне
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionButton.SetActive(false);  // Скрываем кнопку, когда игрок выходит из зоны
        }
    }

    void TakeOrder()
    {
        DishType order = client.dishType;
        //GameManager.Instance.order = order;

        Debug.Log("Заказ: " + order);

        //GameManager.Instance.orderUI.SetActive(true);
        //GameManager.Instance.dishName.text = order.ToString();
        
        Ingredient[] ingredients =  Dish.GetIngredients(order);
        currentOrderIngredients.Clear();
        //GameManager.Instance.ingridients.text = "";

        //for(int i = 0; i < ingredients.Length; i++) GameManager.Instance.ingridients.text += "- "+ ingredients[i].ToString() + "\n";


        foreach (Ingredient ingredient in ingredients)
        {
            currentOrderIngredients.Add(new OrderIngredient { ingredient = ingredient, isAdded = false });
        }

        UpdateIngredientListUI();
    }

    void UpdateIngredientListUI()
    {
        //GameManager.Instance.ingridients.text = "";

        foreach (var orderIngredient in currentOrderIngredients)
        {
            string displayText = orderIngredient.isAdded 
                ? $"<s>{orderIngredient.ingredient}</s>" // Зачёркнутый текст
                : $"- {orderIngredient.ingredient}";

            //GameManager.Instance.ingridients.text += displayText + "\n";
        }
    }

    public void AddIngredientToMixer(Ingredient ingredient)
    {
        foreach (var orderIngredient in currentOrderIngredients)
        {
            if (orderIngredient.ingredient == ingredient && !orderIngredient.isAdded)
            {
                orderIngredient.isAdded = true;
                Debug.Log($"Добавлен ингредиент {ingredient}");
                UpdateIngredientListUI();
                CheckIfOrderCompleted();
                return;
            }
        }

        Debug.LogWarning("Этот ингредиент не нужен для текущего заказа!");
    }

    void CheckIfOrderCompleted()
    {
        foreach (var orderIngredient in currentOrderIngredients)
        {
            if (!orderIngredient.isAdded)
                return;
        }

        Debug.Log("Все ингредиенты добавлены. Можно смешивать!");
        isReadyToMix = true; // Активируем возможность смешивания
        //GameManager.Instance.CompleteOrder(); // Метод для обработки завершения заказа
    }

    public bool IsIngredientNeeded(Ingredient ingredient)
    {
        foreach (var orderIngredient in currentOrderIngredients)
        {
            if (orderIngredient.ingredient == ingredient && !orderIngredient.isAdded)
            {
                return true;
            }
        }
        return false;
    }
}