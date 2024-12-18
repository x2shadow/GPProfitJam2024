using UnityEngine;

public class IngredientPack : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient ingredientType;
    [SerializeField] private GameObject ingredientPrefab;

    public void Interact(PlayerInteraction player)
    {
        //player.TakeIngredient(ingredientType, ingredientPrefab);   

        int freeSlotIndex = player.trayManager.GetFreeSlotIndex();

        if (player.currentIngredient == ingredientType)
        {
            Debug.Log($"Вы уже держите {ingredientType}.");
        }
        else if (freeSlotIndex != -1)
        {
            player.trayManager.tray.SetActive(true);
            player.hasTray = true;  // Есть поднос

            GameObject ingredientObject = Instantiate(ingredientPrefab, player.trayManager.traySlots[freeSlotIndex]);
            ingredientObject.transform.localPosition = Vector3.zero; // Обнуляем позицию в слоте
            player.trayManager.trayIngredients.Add(ingredientType);

            Debug.Log($"Вы взяли {ingredientType} и положили его на поднос в слот {freeSlotIndex + 1}");

            player.currentIngredient = ingredientType;

            // Проигрывание звука
            SoundManager.Instance.PlaySound("PickIngredientSound");
        }
        else Debug.Log("Поднос полон");
    }

    public string GetInteractionHint()
    {
        return $"Взять {ingredientType}";
    }

    public void TakeIngredient(Ingredient ingredient, GameObject ingredientPrefab)
    {

    }
}
