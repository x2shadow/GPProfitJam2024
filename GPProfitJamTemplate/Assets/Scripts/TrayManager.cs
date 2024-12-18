using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    [SerializeField] public GameObject tray; // Поднос персонажа
    [SerializeField] public Transform playerTrayPosition;
    [SerializeField] public Transform[] traySlots = new Transform[3]; // Три слота на подносе
    [SerializeField] public Transform productSlot;
    public List<Ingredient> trayIngredients = new List<Ingredient>(); // Хранение объектов ингредиентов на подносе

    public bool HasIngredients() => trayIngredients.Count > 0;

    public int GetIngredientCount() => trayIngredients.Count;

    public List<Ingredient> TakeIngredients(int count)
    {
        List<Ingredient> takenIngredients = trayIngredients.GetRange(0, Mathf.Min(count, trayIngredients.Count));
        trayIngredients.RemoveRange(0, takenIngredients.Count);
        //UpdateTraySlots();
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
    
    public void UpdateTray(PlayerInteraction player, bool trayActive, DishType dishType = DishType.None, GameObject productPrefab = null, bool isBaked = false)
    {
        player.currentDishType = dishType;
        player.hasTray = trayActive;
        tray.SetActive(trayActive);

        if (productPrefab != null && productSlot.childCount == 0)
        {
            GameObject productObject = Instantiate(productPrefab, productSlot);
            productObject.transform.localPosition = Vector3.zero; // Обнуляем позицию
            player.hasMixedProduct = !isBaked;
            player.hasBakedDish = isBaked;
        }
        else if (productPrefab == null && productSlot.childCount > 0)
        {
            Destroy(productSlot.GetChild(0).gameObject);
            player.hasMixedProduct = false;
            player.hasBakedDish = false;
        }
    }

    public int GetFreeSlotIndex() => trayIngredients.Count < 3 ? trayIngredients.Count : -1;

    public void ClearTraySlot(int slotIndex)
    {
        Destroy(traySlots[slotIndex].GetChild(0).gameObject);
    }

    bool IsTrayEmpty()
    {
        return trayIngredients.Count == 0 ? true : false;
    }

    public void TakeTray()
    {
        tray.SetActive(true);
    }

    public void RemoveTray()
    {
        tray.SetActive(false);
    }
}
