using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : MonoBehaviour
{
    [SerializeField] GameObject mobileUI;
    
    void Start()
    {
        mobileUI.SetActive(Application.isMobilePlatform);
    }
}
