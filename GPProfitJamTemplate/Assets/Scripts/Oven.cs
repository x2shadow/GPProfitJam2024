using UnityEngine;
using UnityEngine.UI;

public class Oven : MonoBehaviour, IInteractable
{
    IOvenState currentState;

    public float bakingTime = 5f;         // Время запекания

    public Slider ovenSlider;

    public IOvenState EmptyState  { get; private set; }
    public IOvenState FilledState { get; private set; }
    public IOvenState BakingState { get; private set; }
    public IOvenState BakedState  { get; private set; }

    void Start()
    {
        ovenSlider.gameObject.SetActive(false);  // Скрыть слайдер при старте
        ovenSlider.maxValue = bakingTime;

        // Начальное состояние
        SetState(new EmptyState(this));
    }

    public void SetState(IOvenState newState)
    {
        currentState = newState;
    }

    public void Interact(PlayerInteraction player)
    {
        currentState.Interact(player);
    }
    

    public string GetInteractionHint()
    {
        return "Добавить или забрать из печки";
    }
}
