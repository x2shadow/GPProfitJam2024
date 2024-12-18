using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform trayPosition; // Позиция подноса на столе

    bool isEmpty = true;


    public void Interact(PlayerInteraction player)
    {
        if (isEmpty)
        {
            if (player.hasTray)
            {
                // Кладем поднос на стол
                player.trayManager.tray.transform.SetParent(trayPosition);
                player.trayManager.tray.transform.localPosition = Vector3.zero;
                player.trayManager.tray.transform.localRotation = Quaternion.identity;
                isEmpty = false;

                Debug.Log("Поднос положен на стол.");
                player.hasTray = false;
            }
            else Debug.Log("Стол пуст нечего класть или забирать");

        }
        else
        {
            // Забираем поднос со стола
            player.trayManager.tray.transform.SetParent(player.trayManager.playerTrayPosition);
            player.trayManager.tray.transform.localPosition = Vector3.zero;
            player.trayManager.tray.transform.localRotation = Quaternion.identity;
            isEmpty = true;

            Debug.Log("Поднос забран обратно.");
            player.hasTray = true;
        }
    }

    public string GetInteractionHint()
    {
        return "Ты около стола";
    }
}
