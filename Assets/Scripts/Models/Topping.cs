using UnityEngine;

[CreateAssetMenu(menuName = "Topping")]
public class Topping : ScriptableObject
{
    [SerializeField]
    private string toppingName;
    [SerializeField]
    private GameObject toppingPrefab;
    [SerializeField]
    private GameObject toppingObjectPrefab;

    public string ToppingName => toppingName;
    public GameObject ToppingPrefab => toppingPrefab;
    public GameObject ToppingObjectPrefab => toppingObjectPrefab;
}
