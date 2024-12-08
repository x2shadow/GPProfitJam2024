using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Ingredient currentIngredient = Ingredient.None;
    private GameObject nearbyObject;

    public bool hasBakedDish = false; // Готовое блюдо в руках
    public bool hasOrder = false;
    
    [SerializeField] ClientSpawnSystem clientSystem;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip takeOrderSound;
    [SerializeField] AudioClip giveOrderSound; // Звук отдачи заказа


    [Header("UI")]
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    public Button     interactButtonMobile; // Ссылка на кнопку Interact

    void Start()
    {
        interactionButton.SetActive(false);
        interactButtonMobile.gameObject.SetActive(false);
        interactButtonMobile.onClick.AddListener(OnInteractButtonPressed);
    }

    void Update()
    {
        if (nearbyObject != null && Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyObject.CompareTag("EggPack"))
            {
                TakeIngredient(Ingredient.Egg);
            }
            else if (nearbyObject.CompareTag("FlourPack"))
            {
                TakeIngredient(Ingredient.Flour);
            }
            else if (nearbyObject.CompareTag("MilkPack"))
            {
                TakeIngredient(Ingredient.Milk);
            }
            else if (nearbyObject.CompareTag("ChocolatePack"))
            {
                TakeIngredient(Ingredient.Chocolate);
            }
            else if(nearbyObject.CompareTag("Client"))
            {
                Client client = nearbyObject.GetComponent<Client>();

                if(hasOrder == false)
                {
                    TakeOrder(client);
                }
                else if (hasBakedDish)
                {
                    GiveDishToClient(client);
                }
                else
                {
                    Debug.Log("У вас нет готового блюда, чтобы отдать клиенту.");
                }

            }
            else if (nearbyObject.CompareTag("Mixer"))
            {
                AddToMixer(nearbyObject.GetComponent<Mixer>());
            }
            else if (nearbyObject.CompareTag("Oven"))
            {
                Oven oven = nearbyObject.GetComponent<Oven>();
                if (oven != null)
                {
                    if (oven.hasMixedProduct)
                    {
                        oven.StartBaking();
                    }
                    else if (oven.hasBakedDish)
                    {
                        TakeBakedDish(oven);
                    }
                    else if (oven.isBaking)
                    {
                        Debug.Log("Продукт всё ещё запекается.");
                    }
                    else
                    {
                        Debug.Log("В печи ничего нет.");
                    }
                }
            }
        }
    }

    void OnInteractButtonPressed()
    {
        if (nearbyObject != null)
        {
            if (nearbyObject.CompareTag("EggPack"))
            {
                TakeIngredient(Ingredient.Egg);
            }
            else if (nearbyObject.CompareTag("FlourPack"))
            {
                TakeIngredient(Ingredient.Flour);
            }
            else if (nearbyObject.CompareTag("MilkPack"))
            {
                TakeIngredient(Ingredient.Milk);
            }
            else if(nearbyObject.CompareTag("Client"))
            {
                if(hasOrder == false)
                {
                    TakeOrder(nearbyObject.GetComponent<Client>());
                }
                else if (hasBakedDish)
                {
                    Debug.Log($"Вы отдали блюдо клиенту!");
                    hasBakedDish = false;
                    hasOrder = false;
                    OrderManager.Instance.CloseOrderUI();
                }
                else
                {
                    Debug.Log("У вас нет готового блюда, чтобы отдать клиенту.");
                }

            }
            else if (nearbyObject.CompareTag("Mixer"))
            {
                AddToMixer(nearbyObject.GetComponent<Mixer>());
            }
            else if (nearbyObject.CompareTag("Oven"))
            {
                Oven oven = nearbyObject.GetComponent<Oven>();
                if (oven != null)
                {
                    if (oven.hasMixedProduct)
                    {
                        oven.StartBaking();
                    }
                    else if (oven.hasBakedDish)
                    {
                        TakeBakedDish(oven);
                    }
                    else if (oven.isBaking)
                    {
                        Debug.Log("Продукт всё ещё запекается.");
                    }
                    else
                    {
                        Debug.Log("В печи ничего нет.");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EggPack") || other.CompareTag("FlourPack") || other.CompareTag("MilkPack") || 
            other.CompareTag("ChocolatePack") || other.CompareTag("Mixer") || other.CompareTag("Oven"))
        {
            nearbyObject = other.gameObject;
            interactionButton.SetActive(true);
            interactButtonMobile.gameObject.SetActive(true);
        }
        else if (other.CompareTag("Client"))
        {
            Client client = other.GetComponent<Client>();
            // Отображаем кнопку только для клиента на выдаче или первого в очереди
            if (clientSystem.waitingClient == client.gameObject || 
                (clientSystem.queue.Count > 0 && clientSystem.queue.Peek() == client.gameObject))
            {
                nearbyObject = other.gameObject;
                interactionButton.SetActive(true);
                interactButtonMobile.gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyObject)
        {
            nearbyObject = null;
            interactionButton.SetActive(false);
            interactButtonMobile.gameObject.SetActive(false);
        }
    }

    void TakeOrder(Client client)
    {
        // Проверка, является ли клиент первым в очереди
        if (clientSystem.queue.Peek() == client.gameObject)
        {
            Debug.Log("Взят заказ клиента.");
            hasOrder = true;
            clientSystem.OrderTaken();
            OrderManager.Instance.InitializeOrder(client.dishType);

            // Проигрывание звука
            if (audioSource != null && takeOrderSound != null)
            {
                audioSource.PlayOneShot(takeOrderSound);
            }
        }
        else
        {
            Debug.LogWarning("Вы можете взять заказ только у первого клиента в очереди.");
        }
    }

    void GiveDishToClient(Client client)
    {
        // Проверка, является ли клиент на точке ожидания
        if (clientSystem.waitingClient == client.gameObject)
        {
            Debug.Log("Вы отдали блюдо клиенту.");
            hasBakedDish = false;
            hasOrder = false;
            clientSystem.OrderGiven();
            OrderManager.Instance.CloseOrderUI();

            // Сброс nearbyObject и отключение кнопки
            nearbyObject = null;
            interactionButton.SetActive(false);
            interactButtonMobile.gameObject.SetActive(false);

            // Проигрывание звука
            if (audioSource != null && giveOrderSound != null)
            {
                audioSource.PlayOneShot(giveOrderSound);
            }
        }
        else
        {
            Debug.LogWarning("Этот клиент не ожидает заказ.");
        }
    }

    void TakeIngredient(Ingredient ingredient)
    {
        if (currentIngredient == ingredient)
        {
            Debug.Log($"Вы уже держите {ingredient}.");
        }
        else
        {
            currentIngredient = ingredient;
            Debug.Log($"Вы взяли {ingredient}.");
        }
    }


    void AddToMixer(Mixer mixer)
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

    void TakeBakedDish(Oven oven)
    {
        if (!hasBakedDish)
        {
            hasBakedDish = true;
            oven.hasBakedDish = false;
            Debug.Log("Вы забрали готовое блюдо из печи.");
        }
        else
        {
            Debug.LogWarning("Вы уже держите готовое блюдо!");
        }
    }

}
