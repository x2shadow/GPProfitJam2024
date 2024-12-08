using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COOKIE_DESTROYER : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    // Используем OnControllerColliderHit, так как у игрока CharacterController
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collision detected with: " + hit.gameObject.name);
        
        // Проверяем, что столкновение с объектом с тегом "Player"
        if (hit.gameObject.CompareTag("Player"))
        {
            Debug.Log("COOKIE DESTROYED!");  // Проверка, что столкновение с игроком происходит
            levelManager.EndLevel(false);  // Завершаем уровень (или проигрыш)
        }
    }
}
