using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Client client { get; private set; }

    public Image dishIcon; // Ссылка на UI элемент для иконки блюда
    public Image[] ingredientIcons; // Массив для иконок ингредиентов
    
    [SerializeField] private Slider timeSlider; // Слайдер для отсчета времени

    [SerializeField] private float orderTime = 5f; // Время на заказ (можно сделать переменной)
    private float currentTime;

    public void Initialize(Client client)
    {
        // Инициализация заказа (например, название блюда)
        this.client = client;
        // Получаем данные о заказе
        DishType dishType = client.dishType; 
        Ingredient[] ingredients = Dish.GetIngredients(dishType);

        // Устанавливаем иконку блюда
        dishIcon.sprite = Dish.GetDishPicture(dishType);

        // Устанавливаем иконки ингредиентов
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (i < ingredients.Length)
            {
                ingredientIcons[i].sprite = IngredientIconManager.GetIngredientIcon(ingredients[i]);
                ingredientIcons[i].gameObject.SetActive(true);
            }
            else
            {
                ingredientIcons[i].gameObject.SetActive(false); // Отключаем лишние иконки
            }
        }

        currentTime = orderTime;
        timeSlider.maxValue = orderTime; // Устанавливаем максимальное время слайдера
        timeSlider.value = currentTime; // Устанавливаем текущее значение слайдера
    }

    void Update()
    {
        // Обновляем слайдер времени
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timeSlider.value = currentTime;
        }
        else
        {
            // Таймер истек, можно обработать завершение заказа
            //Debug.Log("Время на заказ истекло!");
            OrderManager.Instance.CompleteOrder();
        }
    }
}
