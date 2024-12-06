using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : MonoBehaviour
{
    [SerializeField] GameObject mobileUI;
    [SerializeField] PlayerMovement playerMovement;
    
    void Start()
    {
        mobileUI.SetActive(Application.isMobilePlatform);
        playerMovement.joystickActive = Application.isMobilePlatform;
    }
}
