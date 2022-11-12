using System.Collections.Generic;
using UnityEngine;

public class CustomerPresenter : MonoBehaviour, ICustomerPresenter
{
    ICustomerView view;
    [SerializeField]
    Customer model;

    [SerializeField]
    Vector3 target;

    private void Update()
    {
        if (target == Vector3.zero)
            return;
        
        //Bergerak menuju tujuan atau target posisi
        transform.position = Vector3.MoveTowards(transform.position, target, model.Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) <= 0.2f)
        {
            if (model.CurrStat == CustomerStat.Return)
            {
                Destroy(this.gameObject);

                model.CancelOrder();
            }

            model.NextState();
            target = Vector3.zero;
        }
    }

    //Inisialisasi data awal
    public void Initialize(Transform spawnPos, Transform homePos, Transform orderPos, Customer customer, Menu[] menuList, int maxOrder)
    {
        view = GetComponent<CustomerView>();

        this.model = Instantiate(customer);

        model.OnStatChanged += Move;

        model.OnOrderUpdate += UpdateOrder;

        if(spawnPos.position.x > homePos.position.x)
        {
            view.Turn();
        }

        model.Initialize(spawnPos.position, homePos.position, orderPos.position, menuList, maxOrder);

        CookingManager manager = CookingManager.Instance;

        manager.Order(model);
    }

    //Menentukan langkah customer selanjutnya, berangkat -> order -> pulang
    public void Move(CustomerStat stat)
    {
        switch (stat)
        {
            case CustomerStat.Leave:
                view.PlayWalkAnimation(1f);
                target = model.OrderPos;
                break;
            case CustomerStat.Order:
                view.PlayWalkAnimation(0f);
                view.Order(model.MenuOrder, model.WaitTime);
                break;
            case CustomerStat.Return:
                view.PlayWalkAnimation(1f);
                target = model.HomePos;
                view.ShowOrder(false);
                break;
        }
    }

    //Pulang ketika tidak mendapatkan makanan yang di order
    public void Return()
    {
        model.CurrStat = CustomerStat.Return;

        model.CancelOrder();
    }

    //Update tampilan order
    private void UpdateOrder(List<Menu> orderList)
    {
        view.Order(orderList, model.WaitTime);

        view.PlayHappyAnimation(2f);
    }
}
