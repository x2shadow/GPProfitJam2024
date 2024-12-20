using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 2f;       // Высота прыжка
    public float gravity = -9.81f;      // Сила гравитации
    public float gravityScale = 2f;
    public float walkSpeed = 5f;        // Скорость передвижения по плоскости

    private CharacterController characterController;
    public Transform model;
    public Animator animator;
    private Vector3 velocity;           // Текущая скорость персонажа
    private bool isGrounded;            // Проверка, на земле ли персонаж

    private Vector3 currentMove; // Текущее движение
    public float smoothStopTime = 0.08f; // Время для остановки

    public Joystick joystick;
    public bool joystickActive;

    float horizontal;
    float vertical;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;

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
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;
        //Vector3 move = transform.right * horizontal + transform.forward * vertical;

         // Плавное изменение текущего движения к целевому
        currentMove = Vector3.Lerp(currentMove, move, Time.deltaTime / smoothStopTime);


        // Устанавливаем параметр Speed для анимации
        animator.SetFloat("Speed", currentMove.magnitude);

        // Установка параметра "Running"
        bool isRunning = horizontal != 0 || vertical != 0; // Проверка, нажата ли хотя бы одна кнопка
        animator.SetBool("Running", isRunning);


        // Двигаем персонажа
        if (move.magnitude >= 0.1f)
        {
            // Поворачиваем персонажа в направлении движения
            if(Time.timeScale > 0f)
            {
                Quaternion toRotation = Quaternion.LookRotation(currentMove, Vector3.up);
                model.rotation = Quaternion.Lerp(model.rotation, toRotation, Time.deltaTime * 10f);
            }
        }
       

        // Движение персонажа по плоскости
        characterController.Move(currentMove * walkSpeed * Time.deltaTime);

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

            // Проигрывание звука
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Столкновение с объектом: " + hit.gameObject.name);
    }
}
