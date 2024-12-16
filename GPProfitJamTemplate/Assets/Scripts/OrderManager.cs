using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [SerializeField] private OrderUIManager orderUIManager;

    // Список активных заказов
    public Queue<Client> orderQueue = new Queue<Client>(); 

    public List<OrderIngredient> currentOrderIngredients = new List<OrderIngredient>();

    public event Action OnOrderCompleted; // Событие завершения заказа

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void InitializeOrder(DishType dishType)
    {
        Debug.Log("Инициализация заказа: " + dishType);

        Ingredient[] ingredients = Dish.GetIngredients(dishType);
        currentOrderIngredients.Clear();

        foreach (var ingredient in ingredients)
        {
            currentOrderIngredients.Add(new OrderIngredient { ingredient = ingredient, isAdded = false });
        }

        UpdateOrderUI(dishType, currentOrderIngredients);
    }

    public bool IsOrderComplete()
    {
        foreach (var orderIngredient in currentOrderIngredients)
        {
            if (!orderIngredient.isAdded)
                return false;
        }
        return true;
    }

    public void MarkIngredientAdded(Ingredient ingredient)
    {
        var orderIngredient = currentOrderIngredients.Find(oi => oi.ingredient == ingredient && !oi.isAdded);
        if (orderIngredient != null)
        {
            orderIngredient.isAdded = true;
            Debug.Log($"Ингредиент {ingredient} добавлен!");

            UpdateIngredientListUI();

            if (IsOrderComplete())
            {
                Debug.Log("Заказ выполнен!");
                OnOrderCompleted?.Invoke();
            }
        }
    }

    private void UpdateOrderUI(DishType dishType, List<OrderIngredient> ingredients)
    {
        orderUIManager.ShowOrder(dishType, ingredients);
    }

    private void UpdateIngredientListUI()
    {
        orderUIManager.UpdateIngredients(currentOrderIngredients);
    }
}
