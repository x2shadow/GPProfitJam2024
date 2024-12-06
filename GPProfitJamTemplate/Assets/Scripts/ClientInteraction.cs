using UnityEngine;
using UnityEngine.UI;  // Для работы с UI

public class CustomerInteraction : MonoBehaviour
{
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    private bool playerInRange = false;  // Проверка, находится ли игрок в зоне взаимодействия

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
        Debug.Log("Вы взяли заказ!");
        // Здесь можно добавить логику для выполнения заказа, например, отображение информации о заказе
        // или начало выполнения задания.
    }
}