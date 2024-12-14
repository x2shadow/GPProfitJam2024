using UnityEngine;

public class IngredientPack : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient ingredientType;

    public void Interact(PlayerInteraction player)
    {
        player.TakeIngredient(ingredientType);   
    }

    public string GetInteractionHint()
    {
        return $"Взять {ingredientType}";
    }

}
