using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : MonoBehaviour
{
    [SerializeField] GameObject mobileUI;
    [SerializeField] GameObject mobileInteractButton;
    [SerializeField] PlayerInteraction player;
    
    void Start()
    {
        mobileUI.SetActive(Application.isMobilePlatform);
    }

    void Update()
    {
        if (player.nearbyInteractable != null)
        {
            mobileInteractButton.SetActive(true);
        }
        else mobileInteractButton.SetActive(false);
    }
}
