using System;
using UnityEngine;

[System.Serializable]
public class Client : MonoBehaviour, IInteractable
{
    public DishType dishType;

    public void Interact(PlayerInteraction player)
    {
        if(player.hasOrder == false)
        {
            player.TakeOrder(this);
        }
        else if (player.hasBakedDish)
        {
            player.GiveDishToClient(this);
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
}