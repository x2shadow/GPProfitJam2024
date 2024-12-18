using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    [SerializeField] private Transform[] traySlots;
    private List<Ingredient> trayIngredients = new List<Ingredient>();

    public bool HasIngredients() => trayIngredients.Count > 0;

    public int GetIngredientCount() => trayIngredients.Count;

    public List<Ingredient> TakeIngredients(int count)
    {
        List<Ingredient> takenIngredients = trayIngredients.GetRange(0, Mathf.Min(count, trayIngredients.Count));
        trayIngredients.RemoveRange(0, takenIngredients.Count);
        UpdateTraySlots();
        return takenIngredients;
    }

    public void AddIngredient(Ingredient ingredient, GameObject ingredientPrefab)
    {
        if (trayIngredients.Count < traySlots.Length)
        {
            trayIngredients.Add(ingredient);
            GameObject obj = Instantiate(ingredientPrefab, traySlots[trayIngredients.Count - 1]);
            obj.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Поднос полон!");
        }
    }

    private void UpdateTraySlots()
    {
        for (int i = 0; i < traySlots.Length; i++)
        {
            if (i < trayIngredients.Count)
            {
                // Обновить слот (например, иконку)
            }
            else
            {
                // Очистить слот
                if (traySlots[i].childCount > 0)
                {
                    Destroy(traySlots[i].GetChild(0).gameObject);
                }
            }
        }
    }
}
