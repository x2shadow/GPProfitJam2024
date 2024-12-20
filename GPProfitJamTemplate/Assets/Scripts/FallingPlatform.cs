using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    public bool isFalling = false;

    [SerializeField] private float fallDelay = 2f;
    [SerializeField] private float respawnDelay = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void StartFallProcess()
    {
        Debug.Log("StartFallProcess вызван");

        if (isFalling)
        {
            Debug.LogWarning("Платформа уже падает!");
            return;
        }

        Debug.Log("Игрок активировал падение платформы.");
        Invoke(nameof(StartFalling), fallDelay);
        isFalling = true; // Проверяем, что это состояние изменяется
        Debug.Log("isFalling: " + isFalling);
    }


    private void StartFalling()
    {
        Debug.Log("Платформа начинает падать.");
        rb.isKinematic = false;
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        Debug.Log("Платформа восстанавливается.");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isFalling = false;
        Debug.Log("Платформа восстановлена.");
    }

}
