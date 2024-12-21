using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteraction player)
    {
        TrayManager trayManager = player.trayManager;

        if(player.hasTray)
        {
            player.hasTray = false;
            player.hasMixedProduct = false;
            player.hasBakedDish = false;
            player.currentIngredient = Ingredient.None;
    
            // Удаляем все из слотов
            for(int i = 0; i < trayManager.trayIngredients.Count; i++)
            {
                trayManager.ClearTraySlot(i);
            }
            
            trayManager.UpdateTray(player, false);
            trayManager.trayIngredients.Clear();
            SoundManager.Instance.PlaySound("PickIngredientSound");
        }
        else Debug.Log("У вас нет подноса");
    }

    public string GetInteractionHint()
    {
        return "Помойка";
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
