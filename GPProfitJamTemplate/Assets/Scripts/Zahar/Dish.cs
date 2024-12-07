using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredient
{
    None,
    Sugar,
    Egg,
    Milk,
    Flour
}

[System.Serializable]
public class OrderIngredient
{
    public Ingredient ingredient;
    public bool isAdded;
}

public enum DishType
{
    None,
    Cake,
    Cookie
}

public static class Dish
{
    private static Dictionary<DishType, Ingredient[]> dishes = new Dictionary<DishType, Ingredient[]>
    {
        { DishType.Cake,   new Ingredient[] { Ingredient.Flour, Ingredient.Milk, Ingredient.Egg } },
        { DishType.Cookie, new Ingredient[] { Ingredient.Flour, Ingredient.Egg, Ingredient.Milk } }
    };

    public static Ingredient[] GetIngredients(DishType dishType)
    {
        if (dishes.TryGetValue(dishType, out Ingredient[] ingredients))
        {
            return ingredients;
        }
        else
        {
            throw new KeyNotFoundException($"Блюдо {dishType} не найдено в книге рецептов");
        }
    }
}