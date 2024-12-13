using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;

    [Header("UI")]
    public GameObject orderUI;
    public TextMeshProUGUI dishName;
    public TextMeshProUGUI ingredientsUI;

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

    public void ShowOrder(DishType dishType, List<OrderIngredient> ingredients)
    {
        orderUI.SetActive(true);
        dishName.text = TranslateDishType(dishType);
        UpdateIngredients(ingredients);
    }

    public void UpdateIngredients(List<OrderIngredient> ingredients)
    {
        ingredientsUI.text = "";
        foreach (var orderIngredient in ingredients)
        {
            string ingredientRU = TranslateIngredient(orderIngredient.ingredient);
            string displayText = orderIngredient.isAdded
                ? $"<s>{ingredientRU}</s>" // Зачеркнутый текст
                : $"- {ingredientRU}";

            ingredientsUI.text += displayText + "\n";
        }
    }

    public void CloseOrderUI()
    {
        orderUI.SetActive(false);
    }

    private string TranslateDishType(DishType dishType)
    {
        return dishType switch
        {
            DishType.StrawberryCake => "Клубничный торт",
            DishType.ChocolateCake => "Шоколадный торт",
            DishType.Cupcake => "Кекс",
            DishType.Cookie => "Печеньки",
            _ => "НЕТ БЛЮДА"
        };
    }

    private string TranslateIngredient(Ingredient ingredient)
    {
        return ingredient switch
        {
            Ingredient.Chocolate => "Шоколад",
            Ingredient.Egg => "Яйцо",
            Ingredient.Milk => "Молоко",
            Ingredient.Flour => "Мука",
            _ => "НЕТ ИНГРЕДИЕНТА"
        };
    }
}
