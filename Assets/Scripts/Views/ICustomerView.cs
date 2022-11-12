using UnityEngine;
using System.Collections.Generic;

public interface ICustomerView
{
    public void Turn();
    public void Order(List<Menu> orders, float waitTime);

    public void Return();

    public void PlayWalkAnimation(float velocity);

    public void PlayHappyAnimation(float happyTime);

    public void ShowOrder(bool isActive);
}
