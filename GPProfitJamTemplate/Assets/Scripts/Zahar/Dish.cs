using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredient
{
    Sugar,
    Egg,
    Milk,
    Flour
}

public enum DishType
{
    Cake,
    Cookie
}

public static class Dish
{
    private static Dictionary<DishType, Ingredient[]> dishes = new Dictionary<DishType, Ingredient[]>
    {
        { DishType.Cake,   new Ingredient[] { Ingredient.Flour, Ingredient.Sugar, Ingredient.Egg } },
        { DishType.Cookie, new Ingredient[] { Ingredient.Flour, Ingredient.Sugar, Ingredient.Egg, Ingredient.Milk } }
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

/*
[System.Serializable]
public class Dish
{
    [SerializeField]
    List<Ingredient> Ingredients;

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }
}

public class DishFactory
{
    private static System.Random random = new System.Random();

    public static Dish CreateCake()
    {
        Dish cake = new Dish();
        cake.AddIngredient(Ingredient.Flour);
        cake.AddIngredient(Ingredient.Sugar);
        cake.AddIngredient(Ingredient.Egg);
        return cake;
    }

    public static Dish CreateCookie()
    {
        Dish cookie = new Dish();
        cookie.AddIngredient(Ingredient.Flour);
        cookie.AddIngredient(Ingredient.Sugar);
        cookie.AddIngredient(Ingredient.Egg);
        cookie.AddIngredient(Ingredient.Milk);
        return cookie;
    }

    public static Dish CreateRandomDish()
    {
        // Создаём список всех возможных блюд
        Dish[] possibleDishes = new Dish[]
        {
            CreateCake(),
            CreateCookie()
        };

        // Выбираем случайное блюдо
        int randomIndex = random.Next(possibleDishes.Length);
        return possibleDishes[randomIndex];
    }

    public static Dish CreateDishByType(DishType type)
    {
        switch (type)
        {
            case DishType.Cake:
                return CreateCake();
            case DishType.Cookie:
                return CreateCookie();
            default:
                throw new System.ArgumentException($"Неизвестный тип блюда: {type}");
        }
    }
}
*/