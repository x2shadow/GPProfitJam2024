using UnityEngine;

public class IngredientPack : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient ingredientType;
    [SerializeField] private GameObject ingredientPrefab;

    public void Interact(PlayerInteraction player)
    {
        player.TakeIngredient(ingredientType, ingredientPrefab);   
    }

    public string GetInteractionHint()
    {
        return $"Взять {ingredientType}";
    }

}
