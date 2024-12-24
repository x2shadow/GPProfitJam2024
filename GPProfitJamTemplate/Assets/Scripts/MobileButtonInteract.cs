using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileButtonInteract : MonoBehaviour
{
    [SerializeField] PlayerInteraction player;


    public void MobileInteract()
    {
        if (player.nearbyInteractable != null)
        {
            player.nearbyInteractable.Interact(player);
        }
    }
}
