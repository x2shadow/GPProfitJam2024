using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Ingredient currentIngredient = Ingredient.None;
    private IInteractable nearbyInteractable;

    public bool hasMixedProduct = false; // Смешанный продукт в руках
    public bool hasBakedDish = false; // Готовое блюдо в руках
    
    [SerializeField] ClientSpawnSystem clientSystem;

    [Header("Tray System")]
    [SerializeField] private GameObject tray; // Поднос персонажа
    [SerializeField] private Transform playerTrayPosition;
    [SerializeField] private Transform[] traySlots = new Transform[3]; // Три слота на подносе
    private GameObject[] slotContents = new GameObject[3]; // Хранение объектов ингредиентов на подносе
    
    private bool trayIsOnTable = false;


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

    public void HandleTrayInteraction(Transform tablePosition)
    {
        if (!trayIsOnTable)
        {
            // Кладем поднос на стол
            //tableTrayPosition = tablePosition;
            tray.transform.SetParent(tablePosition);
            tray.transform.localPosition = Vector3.zero;
            tray.transform.localRotation = Quaternion.identity;
            trayIsOnTable = true;

            Debug.Log("Поднос положен на стол.");
        }
        else
        {
            // Забираем поднос со стола
            tray.transform.SetParent(playerTrayPosition);
            tray.transform.localPosition = Vector3.zero;
            tray.transform.localRotation = Quaternion.identity;
            trayIsOnTable = false;

            Debug.Log("Поднос забран обратно.");
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
            clientSystem.OrderGiven();
            OrderUIManager.Instance.CloseOrderUI();

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

            GameObject ingredientObject = Instantiate(ingredientPrefab, traySlots[freeSlotIndex]);
            ingredientObject.transform.localPosition = Vector3.zero; // Обнуляем позицию в слоте
            slotContents[freeSlotIndex] = ingredientObject;

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
        for (int i = 0; i < slotContents.Length; i++)
        {
            if (slotContents[i] == null)
                return i; // Возвращаем индекс первого свободного слота
        }
        return -1; // Все слоты заняты
    }
    
    public void ClearTraySlot(int slotIndex)
    {
        if (slotContents[slotIndex] != null)
        {
            Destroy(slotContents[slotIndex]); // Удаляем объект из слота
            slotContents[slotIndex] = null;

            Debug.Log($"Слот {slotIndex + 1} на подносе очищен.");

            // Отключаем поднос, если он пуст
            if (IsTrayEmpty())
                tray.SetActive(false);
        }
    }

    bool IsTrayEmpty()
    {
        foreach (GameObject content in slotContents)
        {
            if (content != null)
                return false;
        }
        return true;
    }

    public void AddToMixer(Mixer mixer)
    {
        if (!mixer.isReadyToMix && currentIngredient != Ingredient.None) // Добавить в миксер
        {
            mixer.AddIngredientToMixer(currentIngredient);
            currentIngredient = Ingredient.None;
        }
        else if (mixer.isReadyToMix)
        {
            // Смешать
            mixer.TryMix();
        }
        else
        {
            Debug.Log("У вас ничего нет.");
        }
    }
}
