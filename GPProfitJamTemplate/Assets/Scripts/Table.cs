using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform trayPosition; // Позиция подноса на столе

    public void Interact(PlayerInteraction player)
    {
        // Передаем позицию подноса на столе
        player.HandleTrayInteraction(trayPosition);
    }

    public string GetInteractionHint()
    {
        return "Ты около стола";
    }
}
