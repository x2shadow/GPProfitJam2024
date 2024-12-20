using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform destination; // Конечная точка
    [SerializeField] private float moveSpeed = 2f;  // Скорость движения
    [SerializeField] private bool loop = true;      // Повторять движение

    CharacterController player;
    bool isPlayerOnPlatform = false;

    private Vector3 startPosition;                 // Начальная позиция платформы
    private Vector3 targetPosition;                // Целевая позиция
    private bool movingToDestination = true;       // Направление движения
    private Vector3 lastPosition;                  // Последняя позиция платформы

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

    private void MovePlatform()
    {
        // Плавное движение платформы
        float step = moveSpeed * Time.fixedDeltaTime;
        Vector3 currentTarget = movingToDestination ? targetPosition : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (loop)
            {
                movingToDestination = !movingToDestination;
            }
        }

        if (isPlayerOnPlatform) MovePlayerWithPlatform(player);
        
        // Обновляем позицию для расчёта движения
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (destination != null) MovePlatform();
    }

    void MovePlayerWithPlatform(CharacterController characterController)
    {
        // Перемещаем объект вместе с платформой
        Vector3 platformMovement = transform.position - lastPosition;
        characterController.Move(platformMovement);
    }

    public void SetPlayerOnPlatform(CharacterController characterController)
    {
        Debug.Log("Игрок встал на платформу");
        isPlayerOnPlatform = true;
        player = characterController;
    }

    public void UnsetPlayerOnPlatform()
    {
        Debug.Log("Игрок сошел с платформы");
        isPlayerOnPlatform = false;
        player = null;
    }
}
