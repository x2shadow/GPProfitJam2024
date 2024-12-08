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
        
        string dishTypeRU = "";
        
        switch(dishType)
        {
            case DishType.StrawberryCake: dishTypeRU = "Клубничный торт"; break;
            case DishType.ChocolateCake:  dishTypeRU = "Шоколадный торт"; break;
            case DishType.Cupcake:        dishTypeRU = "Кекс";            break;
            case DishType.Cookie:         dishTypeRU = "Печеньки";        break;
            case DishType.None:           dishTypeRU = "НЕТ БЛЮДА";       break;
        }

        dishName.text = dishTypeRU;

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

    public void CloseOrderUI()
    {
        orderUI.SetActive(false);
    }

    public void UpdateIngredientListUI()
    {
        ingredientsUI.text = "";

        string ingredientRU = "";

        foreach (var orderIngredient in currentOrderIngredients)
        {
            switch(orderIngredient.ingredient)
            {
                case Ingredient.Chocolate: ingredientRU = "Шоколад";         break;
                case Ingredient.Egg:       ingredientRU = "Яйцо";            break;
                case Ingredient.Milk:      ingredientRU = "Молоко";          break;
                case Ingredient.Flour:     ingredientRU = "Мука";            break;
                case Ingredient.None:      ingredientRU = "НЕТ ИНГРЕДИЕНТА"; break;
            }

            string displayText = orderIngredient.isAdded
                ? $"<s>{ingredientRU}</s>" // Зачеркнутый текст
                : $"- {ingredientRU}";

            ingredientsUI.text += displayText + "\n";
        }
    }
}
