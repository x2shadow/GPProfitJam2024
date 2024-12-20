using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    [SerializeField] FallingPlatform fallingPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if (fallingPlatform.isFalling) return; // Проверка состояния платформы

        Debug.Log("Столкновение с: " + other.gameObject.name + ", тег: " + other.gameObject.tag);  
        
        if (other.gameObject.CompareTag("Player"))fallingPlatform.StartFallProcess();
    }
}
