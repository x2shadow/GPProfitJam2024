using UnityEngine;
using UnityEngine.UI;

public class Oven : MonoBehaviour, IInteractable
{
    public bool hasMixedProduct = false; // Проверяет, есть ли продукт для запекания
    public bool hasBakedDish = false; // Готовое блюдо в печи
    public bool isBaking = false;        // Проверяет, идет ли процесс запекания
    public float bakingTime = 5f;         // Время запекания
    private float bakingTimer = 0f;

    [SerializeField] Slider ovenSlider;

    [Header("AUDIO")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ovenBakingSound;
    [SerializeField] AudioClip ovenBakedSound;
    [SerializeField] AudioClip ovenFilledSound;

    void Start()
    {
        ovenSlider.gameObject.SetActive(false);
        ovenSlider.maxValue = bakingTime;
    }

    void Update()
    {
        if (isBaking)
        {
            bakingTimer += Time.deltaTime;
            ovenSlider.value = bakingTimer;

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

            // Проигрывание звука
            if (audioSource != null && ovenFilledSound != null)
            {
                audioSource.PlayOneShot(ovenFilledSound);
            }
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
            ovenSlider.gameObject.SetActive(true);

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
        ovenSlider.gameObject.SetActive(false);

        hasBakedDish = true; // Блюдо готово

        // Проигрывание звука
        if (audioSource != null && ovenBakedSound != null)
        {
            audioSource.PlayOneShot(ovenBakedSound);
        }
    }

    public void Interact(PlayerInteraction player)
    {
        if (hasMixedProduct)
        {
            StartBaking();
        }
        else if (hasBakedDish)
        {
            player.TakeBakedDish(this);
        }
        else if (isBaking)
        {
            Debug.Log("Продукт всё ещё запекается.");
        }
        else
        {
            Debug.Log("В печи ничего нет.");
        }
    }

    public string GetInteractionHint()
    {
        return "Добавить или забрать из печки";
    }
}
