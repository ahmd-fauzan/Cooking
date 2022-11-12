using UnityEngine;

public interface ICustomerPresenter
{
    public void Initialize(Transform spawnPos, Transform homePos, Transform orderPos, Customer customer, Menu[] menuList, int maxOrder);

    public void Move(CustomerStat stat);

    public void Return();
}
