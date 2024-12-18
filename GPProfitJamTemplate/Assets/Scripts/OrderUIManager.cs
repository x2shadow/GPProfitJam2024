using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;

    [Header("New UI")]
    [SerializeField] private GameObject newOrderUIPrefab;
    [SerializeField] private Transform ordersParent; // Родительский объект для всех заказов (UI)
    [SerializeField] private float orderSpacing = 300f;
    float currentXPosition = 0f;

    private List<OrderUI> activeOrderUIs = new List<OrderUI>(); // Список активных UI для заказов

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

    public void AddNewOrderUI(Client client)
    {
        // Создаем новый UI для нового заказа
        GameObject newOrderUI = Instantiate(newOrderUIPrefab, ordersParent);
        OrderUI orderUI = newOrderUI.GetComponentInChildren<OrderUI>();
        orderUI.Initialize(client);

        // Устанавливаем позицию нового UI элемента
        newOrderUI.transform.localPosition = new Vector3(currentXPosition, newOrderUI.transform.localPosition.y, newOrderUI.transform.localPosition.z);
        
        // Обновляем позицию для следующего заказа
        currentXPosition += orderSpacing;

        activeOrderUIs.Add(orderUI); // Добавляем в список активных UI
    }

    public void RemoveOrderUI(Client client)
    {
        // Удаляем UI заказ из списка, если заказ завершен
        OrderUI orderUI = activeOrderUIs.Find(ui => ui.client == client); // Найдем UI по клиенту
        if (orderUI != null)
        {
            activeOrderUIs.Remove(orderUI);
            Destroy(orderUI.gameObject); // Удаляем UI объект
            UpdateOrderUI();
        }
        else Debug.Log("Не найден!");
    }

    public void UpdateOrderUI()
    {
        currentXPosition = 0f; // Сбросим текущую позицию в начало

        // Пересчитываем позиции всех оставшихся заказов
        for (int i = 0; i < activeOrderUIs.Count; i++)
        {
            GameObject uiObject = activeOrderUIs[i].gameObject;
            uiObject.transform.localPosition = new Vector3(currentXPosition, uiObject.transform.localPosition.y, uiObject.transform.localPosition.z);
            currentXPosition += orderSpacing;
        }
    }

    void OrderFailed()
    {
        Debug.Log("Время истекло! Заказ не выполнен.");
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
