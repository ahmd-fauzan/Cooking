using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Menu")]
public class Menu : ScriptableObject
{
    [SerializeField]
    private string menuName;
    [SerializeField]
    private int price;
    [SerializeField]
    Topping[] toppings;
    [SerializeField]
    GameObject menuPrefab;

    public string MenuName => menuName;
    public int Price => price;
    public Topping[] Toppings => toppings;
    public GameObject MenuPrefab => menuPrefab;

    //Cek apakah makanan bisa ditambahkan topping yang dipilih
    public bool CanAddTopping(List<Topping> toppingList, Topping topping)
    {
        if (toppingList.Count == 0)
            return true;

        if (toppingList.Count > Toppings.Length)
            return false;

        if((toppingList.Intersect(Toppings)).Count() <= 0)
            return false;

        var tempList = Toppings.Except(toppingList);

        foreach(Topping temp in tempList)
        {
            if (temp.ToppingName == topping.ToppingName)
                return true;
        }

        return false;
    }

    //Cek apakah makanan sama dengan menu
    public bool IsMenuSame(List<Topping> toppingList)
    {
        if (toppingList.Count != Toppings.Length)
            return false;

        if ((toppingList.Intersect(Toppings)).Count() != toppingList.Count)
            return false;

        return true;
    }
}
