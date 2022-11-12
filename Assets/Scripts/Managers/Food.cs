using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodState
{
    Batter,
    Cook,
    Ripe,
    Serve
}

public class Food : MonoBehaviour
{
    #region Gameobject prefab
    [SerializeField]
    GameObject batterPrefab;
    [SerializeField]
    GameObject foodPrefab;
    #endregion

    #region Audio
    [SerializeField]
    AudioClip fryClip;
    [SerializeField]
    AudioClip cookFinishClip;

    AudioSource audioSource;
    #endregion

    GameObject currGo;

    private float cookTime = 2f;

    private FoodState currStatus;

    public FoodState CurrStatus
    {
        get
        {
            return currStatus;
        }

        set
        {
            currStatus = value;
        }
    }

    public List<Topping> toppings;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Ketika pertama kali di instantiate akan langsung memanggil method cook
    private void Start()
    {
        StartCoroutine(Cook(cookTime));

        toppings = new List<Topping>();
    }

    //Menambahkan topping
    public void AddTopping(Topping topping)
    {
        Instantiate(topping.ToppingPrefab, transform);

        toppings.Add(topping);
    }

    //Memasak (menguubah adonan menjadi makanan dalam rentang waktu tertentu)
    //Memulai audio memasak, dan ketika sudah matang akan memulai audio sudah matang
    IEnumerator Cook(float time)
    {
        audioSource.clip = fryClip;
        audioSource.Play();

        //Menampilkan tampilan adonan
        CurrStatus = FoodState.Cook;
        currGo = Instantiate(batterPrefab, transform);

        yield return new WaitForSeconds(time);

        //Destroy tampilan makanan sebelumnya (adonan)
        Destroy(currGo);

        //Menampilkan tampilan sudah matang
        currGo = Instantiate(foodPrefab, transform);
        CurrStatus = FoodState.Ripe;

        audioSource.Stop();
        audioSource.clip = cookFinishClip;
        audioSource.Play();
    }
}
