using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Ingredient currentIngredient = Ingredient.None;
    private IInteractable nearbyInteractable;

    public bool hasBakedDish = false; // Готовое блюдо в руках
    public bool hasOrder = false;
    
    [SerializeField] ClientSpawnSystem clientSystem;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip takeOrderSound;
    [SerializeField] AudioClip giveOrderSound; // Звук отдачи заказа
    [SerializeField] AudioClip pickIngredientSound; // Звук подбора ингредиента
    [SerializeField] AudioClip pickFromOvenSound; // Звук сбора из печи


    [Header("UI")]
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    public Button     interactButtonMobile; // Ссылка на кнопку Interact

    bool isMobilePlatform;

    void Start()
    {
        isMobilePlatform = Application.isMobilePlatform;
        interactionButton.SetActive(false);
        interactButtonMobile.gameObject.SetActive(false);
        //interactButtonMobile.onClick.AddListener(OnInteractButtonPressed);
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
            hasOrder = true;
            clientSystem.OrderTaken();
            OrderManager.Instance.InitializeOrder(client.dishType);

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
            hasOrder = false;
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

    public void TakeIngredient(Ingredient ingredient)
    {
        if (currentIngredient == ingredient)
        {
            Debug.Log($"Вы уже держите {ingredient}.");
        }
        else
        {
            currentIngredient = ingredient;
            Debug.Log($"Вы взяли {ingredient}.");

            // Проигрывание звука
            if (audioSource != null && pickIngredientSound != null)
            {
                audioSource.PlayOneShot(pickIngredientSound);
            }
        }
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

    public void TakeBakedDish(Oven oven)
    {
        if (!hasBakedDish)
        {
            hasBakedDish = true;
            oven.hasBakedDish = false;
            Debug.Log("Вы забрали готовое блюдо из печи.");

            // Проигрывание звука
            if (audioSource != null && pickFromOvenSound != null)
            {
                audioSource.PlayOneShot(pickFromOvenSound);
            }
        }
        else
        {
            Debug.LogWarning("Вы уже держите готовое блюдо!");
        }
    }

}
