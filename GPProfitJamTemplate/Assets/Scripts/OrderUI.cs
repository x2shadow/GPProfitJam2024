using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Client client { get; private set; }
    
    [SerializeField] private Slider timeSlider; // Слайдер для отсчета времени
    [SerializeField] private Text dishNameText; // Текст для имени блюда

    [SerializeField] private float orderTime = 5f; // Время на заказ (можно сделать переменной)
    private float currentTime;

    public void Initialize(Client client)
    {
        // Инициализация заказа (например, название блюда)
        this.client = client;
        //dishNameText.text = dishType.ToString(); // Отображаем название блюда
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
