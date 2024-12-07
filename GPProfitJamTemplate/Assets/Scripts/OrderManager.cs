using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public List<OrderIngredient> currentOrderIngredients = new List<OrderIngredient>();
    
    [Header("UI")]
    public GameObject orderUI;
    public TextMeshProUGUI dishName;
    public TextMeshProUGUI ingredientsUI;

    void Awake()
    {
        if(Instance != null && Instance != this)
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
        Debug.Log("Заказ: " + dishType);

        orderUI.SetActive(true);
        dishName.text = dishType.ToString();

        Ingredient[] ingredients = Dish.GetIngredients(dishType);
        currentOrderIngredients.Clear();

        foreach (var ingredient in ingredients)
        {
            currentOrderIngredients.Add(new OrderIngredient { ingredient = ingredient, isAdded = false });
        }

        UpdateIngredientListUI();
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

    public void UpdateIngredientListUI()
    {
        ingredientsUI.text = "";

        foreach (var orderIngredient in currentOrderIngredients)
        {
            string displayText = orderIngredient.isAdded
                ? $"<s>{orderIngredient.ingredient}</s>" // Зачеркнутый текст
                : $"- {orderIngredient.ingredient}";

            ingredientsUI.text += displayText + "\n";
        }
    }
}
