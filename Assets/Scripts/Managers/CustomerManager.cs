using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //Instance levelManager
    LevelManager levelManager;

    //Instance GameManager
    GameManager gameManager;

    //Lokasi muncul customer pertama kali
    [SerializeField]
    Transform[] spawnCustomers;

    //Lokasi customer untuk melakukan order
    [SerializeField]
    Transform[] targetPosCustomers;

    //List customer yang bisa di spawn
    private Customer[] customers;

    //Jumlah customer yang sudah di spawn
    private int customerCount;

    private void Start()
    {
        levelManager = LevelManager.Instance;

        gameManager = GameManager.Instance;

        customers = levelManager.GetCurrentLevel().Customers;

        StartCoroutine(SpawnCustomerTimer(levelManager.GetCurrentLevel().CustomerSpawnTime));
    }

    //Spawn customer pada rentang waktu tertentu yang bisa di set di asset level
    //Customer akan di spawn tanpa batas ketika pada asset level tidak di set max customer yang muncul pada level
    //Jumlah customer yang bisa di spawn akan ditambah 1 ketika player melakukan retry level ketika kalah
    IEnumerator SpawnCustomerTimer(float time)
    {
        while (((customerCount < (levelManager.GetCurrentLevel().MaxCustomer + levelManager.addCustomer)) || levelManager.IsUnlimitedCustomer()) && gameManager.CurrGameState == GameState.Play)
        {
            yield return new WaitForSeconds(time);

            if (IsTargetEmpty())
            {
                SpawnCustomer();
                customerCount++;
            }
        }
    }


    //Memunculkan customer secara random dari list customer yang sudah di set di asset level
    //Menentukan posisi customer untuk melakukan order
    //Inisialisasi customer presenter
    private void SpawnCustomer()
    {
        int index = Random.Range(0, spawnCustomers.Length);

        int customerIndex = Random.Range(0, customers.Length);

        GameObject go = Instantiate(customers[customerIndex].CustomerPrefab, spawnCustomers[index].position, Quaternion.identity);
            
        Transform target = GetTarget();

        go.transform.SetParent(target);

        ICustomerPresenter customerPresenter = go.GetComponent<CustomerPresenter>();

        customerPresenter.Initialize(spawnCustomers[index], spawnCustomers[(spawnCustomers.Length - 1) - index], target, customers[customerIndex], levelManager.GetCurrentLevel().Menus, levelManager.GetCurrentLevel().MaxOrder);
    }

    //Mendapatkan posisi customer untuk melakukan order
    //Posisi untuk melakukan order hanya bisa ditempati oleh satu customer
    private Transform GetTarget()
    {
        int index = Random.Range(0, targetPosCustomers.Length);

        if(targetPosCustomers[index].childCount != 0)
        {
            return GetTarget();
        }

        return targetPosCustomers[index];
    }

    //Mengecek apakah ada posisi yang masih kosong untuk melakukan order
    private bool IsTargetEmpty()
    {
        foreach(Transform target in targetPosCustomers)
        {
            if (target.childCount == 0)
                return true;
        }

        return false;
    }
}
