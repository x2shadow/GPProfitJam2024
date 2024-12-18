using System;
using UnityEngine;

[System.Serializable]
public class Client : MonoBehaviour, IInteractable
{
    public DishType dishType;

    public void Interact(PlayerInteraction player)
    {
        if(!player.hasBakedDish)
        {
            TakeOrder(player, this);
        }
        else if (player.hasBakedDish)
        {
            GiveDishToClient(player, this);
        }
        else
        {
            Debug.Log("У вас нет готового блюда, чтобы отдать клиенту.");
        }
    }
    
    public string GetInteractionHint()
    {
        return "Возьмите заказ у клиета!";
    }

    void TakeOrder(PlayerInteraction player, Client client)
    {
        // Проверка, является ли клиент первым в очереди
        if (player.clientSystem.queue.Peek() == client.gameObject)
        {
            Debug.Log("Взят заказ клиента.");
            OrderManager.Instance.AddNewOrder(client);
            player.clientSystem.OrderTaken();

            // Проигрывание звука
            SoundManager.Instance.PlaySound("OrderTaken");
        }
        else
        {
            Debug.LogWarning("Вы можете взять заказ только у первого клиента в очереди.");
        }
    }

    void GiveDishToClient(PlayerInteraction player, Client client)
    {
        // Проверка, является ли клиент на точке ожидания
        if (player.clientSystem.waitingClient == client.gameObject)
        {
            Debug.Log("Вы отдали блюдо клиенту.");
            player.hasBakedDish = false;
            player.currentDishType = DishType.None; // Убираем блюдо с подноса
            player.hasTray = false;
            player.trayManager.tray.SetActive(false);
            Destroy(player.trayManager.productSlot.GetChild(0).gameObject);
            player.clientSystem.OrderGiven();
            OrderManager.Instance.CompleteOrder();


            // Сброс nearbyInteractable и отключение кнопки
            player.nearbyInteractable = null;
            player.interactionButton.SetActive(false);
            player.interactButtonMobile.gameObject.SetActive(false);

            // Проигрывание звука
            SoundManager.Instance.PlaySound("OrderGiven");
 
        }
        else
        {
            Debug.LogWarning("Этот клиент не ожидает заказ.");
        }
    }
}