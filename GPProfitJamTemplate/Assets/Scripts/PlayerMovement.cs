using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 2f;       // Высота прыжка
    public float gravity = -9.81f;      // Сила гравитации
    public float gravityScale = 2f;
    public float walkSpeed = 5f;        // Скорость передвижения по плоскости

    private CharacterController characterController;
    private Vector3 velocity;           // Текущая скорость персонажа
    private bool isGrounded;            // Проверка, на земле ли персонаж

    public Joystick joystick;
    public bool joystickActive;

    float horizontal;
    float vertical;

    void Start()
    {
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
        if(joystickActive)
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }


        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Движение персонажа по плоскости
        characterController.Move(move * walkSpeed * Time.deltaTime);

        // Прыжок (если персонаж на земле)
        if(Input.GetButtonDown("Jump")) Jump();

        // Применяем гравитацию
        velocity.y += gravity * gravityScale * Time.deltaTime;

        // Перемещение с учетом гравитации
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        // Прыжок (если персонаж на земле)
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Формула для прыжка
        }
    }
}
