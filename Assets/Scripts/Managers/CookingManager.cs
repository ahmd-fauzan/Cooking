using System.Collections.Generic;
using UnityEngine;

public class CookingManager : Singleton<CookingManager>
{
    #region Instance
    //Instance input manager
    InputManager inputManager;

    //Instance level manager
    LevelManager levelManager;
    #endregion

    #region Gameobject
    //gameobject food
    [SerializeField]
    GameObject foodPrefab;

    //list gameobject kompor
    [SerializeField]
    Transform[] stoves;

    //list transform piring
    [SerializeField]
    Transform[] plates;
    #endregion

    List<Food> foods;

    List<Customer> customers;

    #region AudioClip
    [SerializeField]
    AudioClip serveAudio;
    [SerializeField]
    AudioClip foodOnPlateAudio;
    #endregion

    AudioSource audioSource;

    #region Event Delegate
    public delegate void FoodServeDelegate(int price);
    public event FoodServeDelegate OnFoodServed;

    public delegate void CustomerOrderedDelegate();
    public event CustomerOrderedDelegate OnCustomerOrderCompleted;
    #endregion

    #region Subscribe & Unscubscribe input event
    private void OnEnable()
    {
        if (inputManager == null)
            inputManager = InputManager.Instance;
        inputManager.OnTouch += Touch;
    }
    private void OnDisable()
    {
        inputManager.OnTouch -= Touch;
    }
    #endregion

    private void Start()
    {
        foods = new List<Food>();

        customers = new List<Customer>();

        audioSource = GetComponent<AudioSource>();

        levelManager = LevelManager.Instance;
    }

    //Mengecek ketika player menyentuh layar
    private void Touch(Vector2 position)
    {
        RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);

        if (hit2d.collider != null)
        {
            //Jika menyentuh adonan maka adonan akan di masak
            if (hit2d.transform.tag == "Batter")
            {
                Cook();
            }

            //Jika menyentuh makanan sudah matang maka akan dimasukan piring
            //Jika menyentuh makanan yang sudah dipiring maka akan disajikan kepada customer
            if (hit2d.transform.TryGetComponent(out Food food))
            {
                if (food.CurrStatus == FoodState.Ripe)
                    PutFood(food);
                else if (food.CurrStatus == FoodState.Serve)
                    ServeFood(food);
            }

            //Jika player menyentuh topping maka akan ditambahkan pada makanan yang sudah ada dipiring
            if(hit2d.transform.tag == "Topping")
            {
                if(hit2d.collider.TryGetComponent(out ToppingManager toppingManager))
                {
                    AddTopping(toppingManager.Topping);
                }
            }
        }
    }

    //Menambah list order customer
    public void Order(Customer customer)
    {
        customer.OnOrderCompleted += OnOrderCompleted;
        customer.OnOrderCancel += OnOrderCancel;
        customers.Add(customer);
    }

    //Memasak makanan dan mengecek apakah ada kompor yang masih kosong
    public void Cook()
    {
        Transform stove = GetChildZero(stoves);

        if (stove != null)
        {
            GameObject go = Instantiate(foodPrefab, stove);
            foods.Add(go.GetComponent<Food>());
        }
    }

    //Memberikan makanan kepada customer
    public void PutFood(Food food)
    {
        Transform plate = GetChildZero(plates);

        if (plate != null)
        {
            audioSource.clip = foodOnPlateAudio;
            audioSource.Play();

            food.CurrStatus = FoodState.Serve;
            food.transform.SetParent(plate);
            food.transform.localPosition = Vector3.zero;
        }
    }

    //Menambahkan topping ketika terdapat makanan yang sudah ada dipiring
    public void AddTopping(Topping topping)
    {
        foreach (Food food in foods)
        {
            if (food.CurrStatus == FoodState.Serve)
            {

                foreach (Menu menu in levelManager.GetCurrentLevel().Menus)
                {
                    if (menu.CanAddTopping(food.toppings, topping))
                    {
                        if (food != null)
                        {
                            food.AddTopping(topping);
                            return;
                        }
                    }
                }
            }
        }
    }

    //Mengecek apakah terdapat parent yang tidak mempunyai child digunakan untuk mengecek kompor dan piring kosong
    public Transform GetChildZero(Transform[] list)
    {
        foreach(Transform l in list)
        {
            if (l.childCount == 0)
                return l;
        }

        return null;
    }

    //Memberikan makanan pada customer
    public void ServeFood(Food makanan)
    {
        foreach (Customer customer in customers)
        {
            if (customer.IsFoodOrder(makanan))
            {
                audioSource.clip = serveAudio;
                audioSource.Play();

                int price = customer.TakeFood(makanan);
                OnFoodServed?.Invoke(price);
                UpdateCustomerStack(customer);
                Destroy(makanan.gameObject);
                return;
            }
        }
    }

    //Update urutan customer yang melakukan order
    void UpdateCustomerStack(Customer customer)
    {
        customers.Remove(customer);
        customers.Insert(customers.Count, customer);
    }

    //Menghapus customer dari daftar order ketika order sudah beres
    public void OnOrderCompleted(Customer customer)
    {
        OnCustomerOrderCompleted?.Invoke();
        customers.Remove(customer);
    }

    //Menghapus customer ketika customer cancel order
    public void OnOrderCancel(Customer customer)
    {
        customers.Remove(customer);
    }
}
