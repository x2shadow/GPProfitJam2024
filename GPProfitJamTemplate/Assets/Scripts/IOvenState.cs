using System.Collections;
using UnityEngine;

public interface IOvenState
{
    void Interact(PlayerInteraction player);
}

public class EmptyState : IOvenState
{
    Oven oven;

    public EmptyState(Oven oven)
    {
        this.oven = oven;
    }

    public void Interact(PlayerInteraction player)
    {
        if(player.hasMixedProduct)
        {
            Debug.Log("Добавлен смешанный продукт в печь.");
            SoundManager.Instance.PlaySound("AddedToOven");
            player.hasMixedProduct = false;
            oven.SetState(new FilledState(oven)); // Эмулируем добавление продукта для теста
        }
        else Debug.Log("Нечего добавлять в печь.");
    }
}

public class FilledState : IOvenState
{
    Oven oven;

    public FilledState(Oven oven)
    {
        this.oven = oven;
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Процесс запекания начался.");
        oven.SetState(new BakingState(oven));
    }
}

public class BakingState : IOvenState
{
    Oven oven;
    Coroutine bakingCoroutine;
    float bakingTime;

    public BakingState(Oven oven)
    {
        this.oven = oven;
        bakingTime = oven.bakingTime;
        StartBaking();
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Печь работает. Подождите завершения."); 
    }

    private void StartBaking()
    {
        SoundManager.Instance.PlaySound("BakingSound");
        oven.ovenSlider.gameObject.SetActive(true);
        bakingCoroutine = oven.StartCoroutine(BakingProcess());
    }

    private IEnumerator BakingProcess()
    {
        float elapsedTime = 0f;

        while (elapsedTime < bakingTime)
        {
            elapsedTime += Time.deltaTime;
            oven.ovenSlider.value = elapsedTime;
            yield return null; // Ждем до следующего кадра
        }

        FinishBaking();
    }

    private void FinishBaking()
    {
        Debug.Log("Запекание завершено! Продукт готов.");
        SoundManager.Instance.PlaySound("BakedSound");
        oven.ovenSlider.gameObject.SetActive(false);
        oven.SetState(new BakedState(oven));
    }

    // Удалить метод?
    public void StopBaking()
    {
        if (bakingCoroutine != null)
        {
            oven.StopCoroutine(bakingCoroutine);
            Debug.Log("Запекание прервано.");
        }
    }
}

public class BakedState : IOvenState
{
    Oven oven;

    public BakedState(Oven oven)
    {
        this.oven = oven;
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log("Готовое блюдо забрано.");
        SoundManager.Instance.PlaySound("AddedToOven");
        player.hasBakedDish = true;
        oven.SetState(new EmptyState(oven)); // Возврат к пустому состоянию
    }
}
