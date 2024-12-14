using System;
using UnityEngine;

[System.Serializable]
public class Client : MonoBehaviour, IInteractable
{
    public DishType dishType;

    public void Interact(PlayerInteraction player)
    {
        player.TakeOrder(this);
    }

    public string GetInteractionHint()
    {
        return "Возьмите заказ у клиета!";
    }
}