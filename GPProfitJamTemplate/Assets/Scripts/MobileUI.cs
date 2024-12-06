using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : MonoBehaviour
{
    [SerializeField] GameObject mobileUI;
    
    void Start()
    {
        Debug.Log(SystemInfo.deviceType);

        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            mobileUI.SetActive(true);
        }
    }
}
