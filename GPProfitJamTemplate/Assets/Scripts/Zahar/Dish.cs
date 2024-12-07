using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredient
{
    None,
    Milk,
    Flour,
    Egg,
    Chocolate
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
    StrawberryCake,
    Cupcake,
    Cookie,
    ChocolateCake
}

public static class Dish
{
    private static Dictionary<DishType, Ingredient[]> dishes = new Dictionary<DishType, Ingredient[]>
    {
        { DishType.StrawberryCake, new Ingredient[] { Ingredient.Milk, Ingredient.Flour, Ingredient.Egg } },
        { DishType.Cupcake,        new Ingredient[] { Ingredient.Chocolate, Ingredient.Flour, Ingredient.Egg } },
        { DishType.Cookie,         new Ingredient[] { Ingredient.Milk, Ingredient.Flour, Ingredient.Chocolate } },
        { DishType.ChocolateCake,  new Ingredient[] { Ingredient.Milk, Ingredient.Egg, Ingredient.Chocolate } }
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