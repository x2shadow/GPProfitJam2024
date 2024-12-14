using UnityEngine;
using UnityEngine.UI;

public class Oven : MonoBehaviour, IInteractable
{
    private IOvenState currentState;

    public bool hasMixedProduct = false;  // Продукт для запекания
    public bool hasBakedDish = false;     // Готовое блюдо
    public bool isBaking = false;         // Процесс запекания
    public float bakingTime = 5f;         // Время запекания
    public float bakingTimer = 0f;        // Таймер для запекания

    [SerializeField] public Slider ovenSlider;

    void Start()
    {
        ovenSlider.gameObject.SetActive(false);
        ovenSlider.maxValue = bakingTime;
        ChangeState(new EmptyState());  // Начальное состояние печи — пустое
    }

    void Update()
    {
        currentState.Handle(this);
    }
    
    public void Baking()
    {
        bakingTimer += Time.deltaTime; 
        ovenSlider.value = bakingTimer;

        // Проверка на завершение запекания
        if (bakingTimer >= bakingTime)
        {
            isBaking = false;
        }
    }

    // Метод для смены состояния
    public void ChangeState(IOvenState newState)
    {
        currentState = newState;  // Устанавливаем новое состояние
        currentState.Handle(this);  // Обрабатываем новое состояние
    }

    // Метод для добавления продукта в печь
    public void AddMixedProduct()
    {
        if (!hasMixedProduct)  // Если продукт еще не был добавлен
        {
            Debug.Log("Продукт добавлен в печь.");
            hasMixedProduct = true;  // Продукт добавлен в печь
        }
        else
        {
            Debug.LogWarning("В печи уже есть продукт!");
        }
    }

    // Интеракция с печью
    public void Interact(PlayerInteraction player)
    {
        if (!hasMixedProduct && !isBaking)  // Печь пуста и не идет процесс запекания
        {
            AddMixedProduct();  // Добавляем продукт в печь
        }
        else if (hasBakedDish)  // Если блюдо готово, забираем его
        {
            player.TakeBakedDish(this);
        }
        else
        {
            Debug.Log("Печь уже в процессе запекания.");
        }
    }

    public string GetInteractionHint()
    {
        return "Добавить или забрать из печки";
    }
}
