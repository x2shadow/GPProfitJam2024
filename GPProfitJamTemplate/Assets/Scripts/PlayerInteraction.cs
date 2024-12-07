using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Ingredient currentIngredient = Ingredient.None;
    private GameObject nearbyObject;

    public bool hasBakedDish = false; // Готовое блюдо в руках
    public bool hasOrder = false;

    [SerializeField] ClientSpawnSystem clientSystem;

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
                    clientSystem.OrderGiven();
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
        if (other.CompareTag("EggPack") || other.CompareTag("FlourPack") || other.CompareTag("MilkPack") 
            || other.CompareTag("Client")|| other.CompareTag("Mixer") || other.CompareTag("Oven"))
        {
            nearbyObject = other.gameObject;
            interactionButton.SetActive(true);
            interactButtonMobile.gameObject.SetActive(true);
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
        Debug.Log("Взят заказ клиента.");
        hasOrder = true;
        clientSystem.OrderTaken();
        OrderManager.Instance.InitializeOrder(client.dishType);
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
