using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 5f; // Сила отталкивания
    [SerializeField] private float knockbackDuration = 0.5f; // Длительность отталкивания

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что столкновение происходит с игроком
        if (other.CompareTag("Player"))
        {
            // Получаем компонент CharacterController игрока
            CharacterController characterController = other.GetComponent<CharacterController>();

            if (characterController != null)
            {
                // Получаем точку столкновения и направление отталкивания
                Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);

                // Рассчитываем направление отталкивания: противоположное направлению столкновения
                Vector3 knockbackDirection = (other.transform.position - contactPoint).normalized;

                // Добавляем компонент по оси Y для отталкивания под углом 45 градусов
                knockbackDirection = new Vector3(knockbackDirection.x, 1f, knockbackDirection.z).normalized;

                // Плавно отталкиваем игрока
                StartCoroutine(ApplyKnockback(characterController, knockbackDirection));
            }
        }
    }

    // Корутина для плавного отталкивания
    private IEnumerator ApplyKnockback(CharacterController characterController, Vector3 direction)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = characterController.transform.position;
        
        // Двигаем игрока в течение knockbackDuration
        while (elapsedTime < knockbackDuration)
        {
            float step = knockbackForce * Time.deltaTime;
            characterController.Move(direction * step); // Перемещаем игрока
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
