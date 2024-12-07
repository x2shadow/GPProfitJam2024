using UnityEngine;
using UnityEngine.UI;  // Для работы с UI

public class ClientInteraction : MonoBehaviour
{
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    private bool playerInRange = false;  // Проверка, находится ли игрок в зоне взаимодействия
    [SerializeField] Client client;

    void Start()
    {
        // Изначально кнопка скрыта
        interactionButton.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Используем E для взаимодействия
        {
            TakeOrder();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionButton.SetActive(true);  // Показываем кнопку, когда игрок в зоне
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionButton.SetActive(false);  // Скрываем кнопку, когда игрок выходит из зоны
        }
    }

    void TakeOrder()
    {
        DishType order = client.dishType;

        GameManager.Instance.order = order;

        Debug.Log("Заказ: " + order);

        GameManager.Instance.orderUI.SetActive(true);
        GameManager.Instance.dishName.text = order.ToString();
        
        Ingredient[] ingredients =  Dish.GetIngredients(order);
        GameManager.Instance.ingridients.text = "";

        for(int i = 0; i < ingredients.Length; i++) GameManager.Instance.ingridients.text += "- "+ ingredients[i].ToString() + "\n";

        // Здесь можно добавить логику для выполнения заказа, например, отображение информации о заказе
        // или начало выполнения задания.
    }
}