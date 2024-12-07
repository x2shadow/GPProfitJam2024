using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Ingredient currentIngredient = Ingredient.None;
    private GameObject nearbyObject;

    public GameObject interactionButton;  // Кнопка "Взять заказ"

    void Start()
    {
        interactionButton.SetActive(false);
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
                Debug.Log("OrderTaken");
                TakeOrder(nearbyObject.GetComponent<Client>());
            }
            else if (nearbyObject.CompareTag("Mixer"))
            {
                AddToMixer(nearbyObject.GetComponent<Mixer>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EggPack") || other.CompareTag("FlourPack") || other.CompareTag("MilkPack") || other.CompareTag("Client")|| other.CompareTag("Mixer"))
        {
            nearbyObject = other.gameObject;
            interactionButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyObject)
        {
            nearbyObject = null;
            interactionButton.SetActive(false);
        }
    }

    void TakeOrder(Client client)
    {
        Debug.Log("Взят заказ клиента.");
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

}
