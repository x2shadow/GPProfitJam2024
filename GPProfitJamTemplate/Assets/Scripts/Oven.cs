using UnityEngine;

public class Oven : MonoBehaviour
{
    public bool hasMixedProduct = false; // Проверяет, есть ли продукт для запекания
    public bool hasBakedDish = false; // Готовое блюдо в печи
    public bool isBaking = false;        // Проверяет, идет ли процесс запекания
    public float bakingTime = 5f;         // Время запекания
    private float bakingTimer = 0f;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ovenBakingSound;
    [SerializeField] AudioClip ovenBakedSound;

    void Update()
    {
        if (isBaking)
        {
            bakingTimer += Time.deltaTime;

            if (bakingTimer >= bakingTime)
            {
                FinishBaking();
            }
        }
    }

    public void AddMixedProduct()
    {
        if (!hasMixedProduct)
        {
            Debug.Log("Смешанный продукт добавлен в печь.");
            hasMixedProduct = true;
        }
        else
        {
            Debug.LogWarning("В печи уже есть продукт!");
        }
    }

    public void StartBaking()
    {
        if (hasMixedProduct && !isBaking)
        {
            Debug.Log("Процесс запекания начался.");
            isBaking = true;

            // Проигрывание звука
            if (audioSource != null && ovenBakingSound != null)
            {
                audioSource.PlayOneShot(ovenBakingSound);
            }
        }
        else if (!hasMixedProduct)
        {
            Debug.LogWarning("В печь ничего не положено для запекания.");
        }
        else
        {
            Debug.LogWarning("Запекание уже идет!");
        }
    }

    private void FinishBaking()
    {
        Debug.Log("Запекание завершено! Продукт готов.");
        hasMixedProduct = false;
        isBaking = false;
        bakingTimer = 0f;

        hasBakedDish = true; // Блюдо готово

        // Проигрывание звука
        if (audioSource != null && ovenBakedSound != null)
        {
            audioSource.PlayOneShot(ovenBakedSound);
        }
    }
}
