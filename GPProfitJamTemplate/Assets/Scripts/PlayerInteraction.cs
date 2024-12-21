using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Ingredient currentIngredient = Ingredient.None;
    public IInteractable nearbyInteractable;

    public DishType currentDishType;

    public ClientSpawnSystem clientSystem;
   
    public TrayManager trayManager; // Менеджер подноса

    [SerializeField] Animator animator;

    public bool hasMixedProduct = false; // Смешанный продукт в руках
    public bool hasBakedDish = false; // Готовое блюдо в руках
    public bool hasTray = false;    // Поднос в руках 

    [Header("UI")]
    public GameObject interactionButton;  // Кнопка "Взять заказ"
    public Button     interactButtonMobile; // Ссылка на кнопку Interact

    void Start()
    {
        interactionButton.SetActive(false);
        interactButtonMobile.gameObject.SetActive(false);
        trayManager.tray.SetActive(false);
    }

    void Update()
    {
        animator.SetBool("hasTray", hasTray);


        if (nearbyInteractable != null && Input.GetButtonDown("Interact"))
        {
            nearbyInteractable.Interact(this);
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
}
