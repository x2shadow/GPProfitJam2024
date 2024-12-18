using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Ingredient currentIngredient = Ingredient.None;
    private IInteractable nearbyInteractable;

    DishType currentDishType;

    public bool hasMixedProduct = false; // Смешанный продукт в руках
    public bool hasBakedDish = false; // Готовое блюдо в руках
    public bool hasTray = false;    // Поднос в руках
    
    [SerializeField] ClientSpawnSystem clientSystem;

    [Header("Tray System")]
    [SerializeField] public GameObject tray; // Поднос персонажа
    [SerializeField] public Transform playerTrayPosition;
    [SerializeField] private Transform[] traySlots = new Transform[3]; // Три слота на подносе
    [SerializeField] private Transform productSlot;
    public List<Ingredient> trayIngredients = new List<Ingredient>(); // Хранение объектов ингредиентов на подносе
    
    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickIngredientSound; // Звук подбора ингредиента


    [Header("UI")]
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    public Button     interactButtonMobile; // Ссылка на кнопку Interact

    bool isMobilePlatform;

    void Start()
    {
        isMobilePlatform = Application.isMobilePlatform;
        interactionButton.SetActive(false);
        interactButtonMobile.gameObject.SetActive(false);
        tray.SetActive(false);
    }

    void Update()
    {
        if (nearbyInteractable != null && Input.GetButtonDown("Interact"))
        {
            nearbyInteractable.Interact(this);
        }
    }

    void SetInteractButton(bool status)
    {
        if (Application.isMobilePlatform)
        {
            interactButtonMobile.gameObject.SetActive(status);
        }
        else
        {
            interactionButton.SetActive(status);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, реализует ли объект интерфейс IInteractable
        nearbyInteractable = other.GetComponent<IInteractable>();
        if (nearbyInteractable != null)
        {
            // Показываем подсказку
            interactionButton.SetActive(true);
            //interactionButtonText.text = nearbyInteractable.GetInteractionHint();
            Debug.Log(nearbyInteractable.GetInteractionHint());
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() == nearbyInteractable)
        {
            nearbyInteractable = null;
            interactionButton.SetActive(false);
        }
    }


    public void TakeOrder(Client client)
    {
        // Проверка, является ли клиент первым в очереди
        if (clientSystem.queue.Peek() == client.gameObject)
        {
            Debug.Log("Взят заказ клиента.");
            OrderManager.Instance.AddNewOrder(client);
            clientSystem.OrderTaken();

            // Проигрывание звука
            SoundManager.Instance.PlaySound("OrderTaken");
        }
        else
        {
            Debug.LogWarning("Вы можете взять заказ только у первого клиента в очереди.");
        }
    }

    public void GiveDishToClient(Client client)
    {
        // Проверка, является ли клиент на точке ожидания
        if (clientSystem.waitingClient == client.gameObject)
        {
            Debug.Log("Вы отдали блюдо клиенту.");
            hasBakedDish = false;
            currentDishType = DishType.None; // Убираем блюдо с подноса
            hasTray = false;
            tray.SetActive(false);
            Destroy(productSlot.GetChild(0).gameObject);
            clientSystem.OrderGiven();
            OrderManager.Instance.CompleteOrder();


            // Сброс nearbyInteractable и отключение кнопки
            nearbyInteractable = null;
            interactionButton.SetActive(false);
            interactButtonMobile.gameObject.SetActive(false);

            // Проигрывание звука
            SoundManager.Instance.PlaySound("OrderGiven");
 
        }
        else
        {
            Debug.LogWarning("Этот клиент не ожидает заказ.");
        }
    }

    public void TakeIngredient(Ingredient ingredient, GameObject ingredientPrefab)
    {
        int freeSlotIndex = GetFreeSlotIndex();

        if (currentIngredient == ingredient)
        {
            Debug.Log($"Вы уже держите {ingredient}.");
        }
        else if (freeSlotIndex != -1)
        {
            tray.SetActive(true);
            hasTray = true;  // Есть поднос

            GameObject ingredientObject = Instantiate(ingredientPrefab, traySlots[freeSlotIndex]);
            ingredientObject.transform.localPosition = Vector3.zero; // Обнуляем позицию в слоте
            trayIngredients.Add(ingredient);

            Debug.Log($"Вы взяли {ingredient} и положили его на поднос в слот {freeSlotIndex + 1}");

            currentIngredient = ingredient;
            //Debug.Log($"Вы взяли {ingredient}.");

            // Проигрывание звука
            if (audioSource != null && pickIngredientSound != null)
            {
                audioSource.PlayOneShot(pickIngredientSound);
            }
        }
        else Debug.Log("Поднос полон");
    }

    int GetFreeSlotIndex()
    {
        int freeSlotIndex = trayIngredients.Count < 3 ? trayIngredients.Count : -1;
        return freeSlotIndex; // Все слоты заняты
    }
    
    public void ClearTraySlot(int slotIndex)
    {
        Destroy(traySlots[slotIndex].GetChild(0).gameObject);
    }

    bool IsTrayEmpty()
    {
        return trayIngredients.Count == 0 ? true : false;
    }

    public bool HasIngredients()
    {
        return trayIngredients.Count > 0;
    }

    public int GetIngredientCount()
    {
        return trayIngredients.Count;
    }

    public List<Ingredient> TakeIngredients(int count)
    {
        List<Ingredient> takenIngredients = trayIngredients.GetRange(0, count);
        //trayIngredients.RemoveRange(0, count);
        return takenIngredients;
    }

    public bool CanTakeDish()
    {
        return currentDishType == DishType.None;
    }

    public void TakeDish(DishType dishType, GameObject productPrefab)
    {
        currentDishType = dishType;
        hasTray = true;
        tray.SetActive(true);
        hasMixedProduct = true;
        GameObject ingredientObject = Instantiate(productPrefab, productSlot);
        ingredientObject.transform.localPosition = Vector3.zero; // Обнуляем позицию в слоте
        Debug.Log($"Игрок забрал блюдо: {dishType}");
    }

    public void TakeBakedDish(DishType bakedDish, GameObject productPrefab)
    {
        currentDishType = bakedDish; // Добавляем готовое блюдо на поднос
        hasTray = true;
        tray.SetActive(true);
        hasBakedDish = true;
        GameObject productObject = Instantiate(productPrefab, productSlot);
        productObject.transform.localPosition = Vector3.zero; // Обнуляем позицию в слоте
        Debug.Log($"Готовое блюдо забрано на поднос: {bakedDish}");
    }

    public void PlaceDishInOven()
    {
        currentDishType = DishType.None; // Убираем блюдо с подноса
        hasTray = false;
        tray.SetActive(false);
        Destroy(productSlot.GetChild(0).gameObject);
        Debug.Log("Смешанное блюдо положено в печку.");
    }
}
