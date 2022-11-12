using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    private int stage;
    [SerializeField]
    private int subLevel;
    [SerializeField]
    private float timer;
    [SerializeField]
    private int foodSale;
    [SerializeField]
    private int customerTarget;
    [SerializeField]
    private bool unlock;
    [SerializeField]
    private bool completed;
    [SerializeField]
    private int maxCustomer;
    [SerializeField]
    private float customerSpawnTime;
    [SerializeField]
    private Customer[] customers;
    [SerializeField]
    private Menu[] menus;
    [SerializeField]
    private Topping[] toppings;
    [SerializeField]
    private int maxOrder;

    public int Stage => stage;
    public int SubLevel => subLevel;
    public float Timer => timer;
    public int FoodSaleTarget => foodSale;
    public int CustomerTarget => customerTarget;
    public bool IsUnlock => unlock;
    public bool IsCompleted => completed;
    public int MaxCustomer => maxCustomer;
    public float CustomerSpawnTime => customerSpawnTime;
    public Customer[] Customers => customers;
    public Menu[] Menus => menus;
    public Topping[] Toppings => toppings;

    public int MaxOrder => maxOrder;
    public bool Finish
    {
        get
        {
            return completed;
        }
        set
        {
            completed = value;
        }
    }

    public void LevelUnlocked()
    {
        unlock = true;
    }

    public void LevelCompleted()
    {
        completed = true;
    }
}
