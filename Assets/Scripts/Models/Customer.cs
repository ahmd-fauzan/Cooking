using System.Collections.Generic;
using UnityEngine;

public enum CustomerStat
{
    Idle,
    Leave,
    Order,
    Return
}

[CreateAssetMenu(menuName = "Customer")]
public class Customer : ScriptableObject
{
    #region CustomerState
    private CustomerStat currStat = CustomerStat.Idle;
    public CustomerStat CurrStat
    {
        get { return currStat; }
        set
        {
            if (value != currStat)
                OnStatChanged?.Invoke(value);
            currStat = value;
        }
    }
    #endregion

    #region Position
    private Vector3 spawnPos;
    private Vector3 homePos;
    private Vector3 orderPos;
    #endregion

    private List<Food> foods;

    [SerializeField]
    float speed;
    [SerializeField]
    float waitTime;
    [SerializeField]
    private GameObject customerPrefab;

    public float Speed => speed;
    public float WaitTime => waitTime;
    public List<Menu> MenuOrder => orderList;
    public Vector3 SpawnPos => spawnPos;
    public Vector3 HomePos => homePos;
    public Vector3 OrderPos => orderPos;
    public GameObject CustomerPrefab => customerPrefab;


    List<Menu> orderList;

    #region Event Delegate
    public delegate void CustomerStatDelegate(CustomerStat stat);
    public event CustomerStatDelegate OnStatChanged;

    public delegate void OrderCompletedDelegate(Customer customer);
    public event OrderCompletedDelegate OnOrderCompleted;

    public delegate void OrderUpdateDelegate(List<Menu> orderList);
    public event OrderUpdateDelegate OnOrderUpdate;

    public delegate void OrderCanceledDelegate(Customer customer);
    public event OrderCanceledDelegate OnOrderCancel;
    #endregion

    //Inisialisasi data awal
    public void Initialize(Vector3 spawnPos, Vector3 homePos, Vector3 orderPos, Menu[] menuList, int maxOrder)
    {
        this.spawnPos = spawnPos;
        this.homePos = homePos;
        this.orderPos = orderPos;

        MakeOrder(menuList, maxOrder);

        foods = new List<Food>();

        CurrStat = CustomerStat.Leave;
    }

    //Cancel order ketika tidak mendapatkan makanan pada rentang waktu tertentu
    public void CancelOrder()
    {
        OnOrderCancel?.Invoke(this);
    }

    //Mengubah customer state
    public void NextState()
    {
        if (CurrStat == CustomerStat.Leave)
            CurrStat = CustomerStat.Order;
        else if (CurrStat == CustomerStat.Order)
            CurrStat = CustomerStat.Return;
    }

    //Menentukan order
    void MakeOrder(Menu[] menuList, int maxOrder)
    {
        orderList = new List<Menu>();

        int orderCount = Random.Range(1, maxOrder + 1);

        for (int i = 0; i < orderCount; i++)
        {
            int index = Random.Range(0, menuList.Length);

            orderList.Add(menuList[index]);
        }
    }

    //Mendapatkan makanan dan mengecek apakah makanan yang diterima sudah semua yang di order
    public int TakeFood(Food makanan)
    {
        foods.Add(makanan);

        int price = RemoveOrder(makanan);

        if(orderList.Count == 0)
        {
            OnOrderCompleted?.Invoke(this);

            CurrStat = CustomerStat.Return;
        }

        return price;
    }

    //Mengecek apakah makanan tersebut merupakan yang dipesan
    public bool IsFoodOrder(Food makanan)
    {
        if(CurrStat == CustomerStat.Order)
        {
            foreach (Menu menu in orderList)
            {
                if (menu.IsMenuSame(makanan.toppings))
                {
                    return true;
                }
            }
        }

        return false;
    }

    //Menghapus makanan yang di order ketika sudah menerima makanan
    public int RemoveOrder(Food makanan)
    {
        foreach (Menu menu in orderList)
        {
            if (menu.IsMenuSame(makanan.toppings))
            {
                int price = menu.Price;

                orderList.Remove(menu);

                OnOrderUpdate?.Invoke(orderList);
                return price;
            }
        }
        return 0;
    }
}
