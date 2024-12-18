using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private static Dictionary<DishType, Sprite> dishIcons = new Dictionary<DishType, Sprite>();

    public static void LoadIcons()
    {
        dishIcons[DishType.StrawberryCake] = Resources.Load<Sprite>("Icons/StrawberryCakeIcon");
        dishIcons[DishType.Cupcake] = Resources.Load<Sprite>("Icons/CupcakeIcon");
        dishIcons[DishType.Cookie] = Resources.Load<Sprite>("Icons/CookieIcon");
        dishIcons[DishType.ChocolateCake] = Resources.Load<Sprite>("Icons/ChocolateCakeIcon");
    }

    private static Dictionary<DishType, GameObject> dishPrefabs = new Dictionary<DishType, GameObject>(); 

    public static void LoadPrefabs()
    {
        dishPrefabs[DishType.StrawberryCake] = Resources.Load<GameObject>("Prefabs/StrawberryCake");
        dishPrefabs[DishType.Cupcake] = Resources.Load<GameObject>("Prefabs/Cupcake");
        dishPrefabs[DishType.Cookie] = Resources.Load<GameObject>("Prefabs/Cookie");
        dishPrefabs[DishType.ChocolateCake] = Resources.Load<GameObject>("Prefabs/ChocolateCake");
    }

    public static void LoadResources()
    {
        LoadIcons();
        LoadPrefabs();
    }

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

    public static DishType CreateDishFromIngredients(List<Ingredient> ingredients)
    {
        // Убедимся, что ингредиенты отсортированы для корректного сопоставления
        ingredients.Sort();

        // Пример сопоставления комбинаций ингредиентов с блюдами
        if (ingredients.Count == 3)
        {
            if (ingredients[0] == Ingredient.Milk &&
                ingredients[1] == Ingredient.Flour &&
                ingredients[2] == Ingredient.Egg)
            {
                return DishType.StrawberryCake;
            }
            if (ingredients[0] == Ingredient.Chocolate &&
                ingredients[1] == Ingredient.Flour &&
                ingredients[2] == Ingredient.Egg)
            {
                return DishType.Cupcake;
            }
            if (ingredients[0] == Ingredient.Milk &&
                ingredients[1] == Ingredient.Flour &&
                ingredients[2] == Ingredient.Chocolate)
            {
                return DishType.Cookie;
            }
            if (ingredients[0] == Ingredient.Milk &&
                ingredients[1] == Ingredient.Egg &&
                ingredients[2] == Ingredient.Chocolate)
            {
                return DishType.ChocolateCake;
            }
        }

        // Если комбинация не соответствует ни одному блюду
        return DishType.None;
    }

    public static Sprite GetDishIcon(DishType dishType)
    {
        if (dishIcons.TryGetValue(dishType, out Sprite icon))
        {
            return icon;
        }

        Debug.LogWarning($"Иконка для блюда {dishType} не найдена.");
        return null;
    }

    public static GameObject GetDishPrefab(DishType dishType)
    {
        if (dishPrefabs.TryGetValue(dishType, out GameObject dishPrefab))
        {
            return dishPrefab;
        }

        Debug.LogWarning($"Префаб для блюда {dishType} не найдена.");
        return null;
    }
}