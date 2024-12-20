using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTriggerDetection : MonoBehaviour
{
    [SerializeField] MovingPlatform movingPlatform;

    private void OnTriggerEnter(Collider other)
    {
        //if (movingPlatform.isFalling) return; // Проверка состояния платформы

        Debug.Log("MovingPlatform: Столкновение с: " + other.gameObject.name + ", тег: " + other.gameObject.tag);  

        CharacterController characterController = other.GetComponent<CharacterController>();
        
        if (characterController != null)
        {
            Debug.Log("Игрок встал на платформу: " + other.gameObject.name);
            movingPlatform.SetPlayerOnPlatform(characterController); // Устанавливаем игрока на платформу
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) movingPlatform.UnsetPlayerOnPlatform();
    }
}
