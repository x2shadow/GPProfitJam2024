using System.Collections;
using UnityEngine;

public class LiftPlatform : MonoBehaviour
{
    [SerializeField] private Transform destination; // Конечная точка для лифта
    [SerializeField] private float moveSpeed = 2f;  // Скорость движения лифта
    [SerializeField] private bool isMovingUp = false; // Направление движения лифта (вверх/вниз)
    [SerializeField] private float delayBeforeMovingDown = 1f; // Задержка перед движением вниз
    [SerializeField] private float delayBeforeMovingUp   = 1f;


    private CharacterController player;              // Ссылка на игрока
    private bool isPlayerOnLift = false;             // Флаг, проверяющий, стоит ли игрок на лифте

    private Vector3 startPosition;                   // Начальная позиция лифта
    private Vector3 targetPosition;
    private Vector3 lastPosition;                    // Для расчёта движения платформы

    private void Start()
    {
        startPosition = transform.position;
        lastPosition = startPosition;

        if (destination != null)
        {
            targetPosition = destination.position;
        }
        else
        {
            Debug.LogError("Destination не задан. Добавьте дочерний объект и назначьте его в инспекторе.");
        }
    }

    private void FixedUpdate()
    {
        if (destination != null)
        {
            MoveLift();
        }
    }

    private void MoveLift()
    {
        // Плавное движение лифта вверх или вниз
        float step = moveSpeed * Time.fixedDeltaTime;
        Vector3 currentTarget = isMovingUp ? targetPosition : startPosition;

        // Перемещаем платформу к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

        // Если лифт достиг конечной точки
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (isMovingUp)
            {
                if (!isPlayerOnLift)
                {  
                    StartCoroutine(WaitBeforeMovingDown());
                }
            }
        }

        // Перемещаем игрока с лифтом, если он на нем
        if (isPlayerOnLift) MovePlayerWithLift(player);

        // Обновляем последнюю позицию лифта для расчёта движения
        lastPosition = transform.position;
    }

    private void MovePlayerWithLift(CharacterController characterController)
    {
        // Перемещаем игрока вместе с лифтом
        Vector3 platformMovement = transform.position - lastPosition;
        characterController.Move(platformMovement);
    }

    IEnumerator WaitBeforeMovingDown()
    {
        // Задержка перед движением вниз
        yield return new WaitForSeconds(delayBeforeMovingDown);

        // После задержки начать движение вниз
        isMovingUp = false;
    }

    IEnumerator WaitBeforeMovingUp()
    {
        yield return new WaitForSeconds(delayBeforeMovingUp);

        isMovingUp = true;
    }

    public void SetPlayerOnLift(CharacterController characterController)
    {
        Debug.Log("Игрок встал на лифт");
        isPlayerOnLift = true;
        player = characterController;
        StartCoroutine(WaitBeforeMovingUp());
    }

    public void UnsetPlayerOnLift()
    {
        Debug.Log("Игрок сошел с лифта");
        isPlayerOnLift = false;
        player = null;
    }
}
