using System;

[System.Serializable]
public class Client
{
    public DishType dishType;
    public float arrivalTime;

    public Dish GetDish()
    {
        return DishFactory.CreateDishByType(dishType);
    }
}