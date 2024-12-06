using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 2f;       // Высота прыжка
    public float gravity = -9.81f;      // Сила гравитации
    public float gravityScale = 2f;
    public float walkSpeed = 5f;        // Скорость передвижения по плоскости
    public float rotationSpeed = 700f;  // Скорость поворота персонажа

    private CharacterController characterController;
    private Vector3 velocity;           // Текущая скорость персонажа
    private bool isGrounded;            // Проверка, на земле ли персонаж

    void Start()
    {
        // Получаем компонент CharacterController
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Проверка, находится ли персонаж на земле
        isGrounded = characterController.isGrounded;

        // Если на земле, сбрасываем скорость по оси Y (чтобы избежать накопления гравитации)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Обработка движения персонажа
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Движение персонажа по плоскости
        characterController.Move(move * walkSpeed * Time.deltaTime);

        // Прыжок (если персонаж на земле)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Формула для прыжка
        }

        // Применяем гравитацию
        velocity.y += gravity * gravityScale * Time.deltaTime;

        // Перемещение с учетом гравитации
        characterController.Move(velocity * Time.deltaTime);
    }
}
