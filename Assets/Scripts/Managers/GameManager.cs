using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Play,
    Pause,
    Stop
}

public class GameManager : Singleton<GameManager>
{
    public GameState CurrGameState;

    [SerializeField]
    GameObject PopUp;

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    TextMeshProUGUI missionText;

    [SerializeField]
    TextMeshProUGUI coinText;

    [SerializeField]
    GameObject retryButton;
    [SerializeField]
    Transform[] toppingSpawns;

    LevelManager levelManager;

    CookingManager cookingManager;

    CoinManager coinManager;

    private int customerTarget;
    private int foodSaleTarget;

    private void Start()
    {
        CurrGameState = GameState.Play;

        levelManager = LevelManager.Instance;

        cookingManager = CookingManager.Instance;

        coinManager = CoinManager.Instance;

        SpawnTopping(levelManager.GetCurrentLevel().Toppings);

        cookingManager.OnFoodServed += OnSaleCompleted;
        cookingManager.OnCustomerOrderCompleted += OnCustomerCompleted;

        StartCoroutine(Timer(levelManager.GetCurrentLevel().Timer));

        coinManager.ResetCoin();

        UpdateCoin(coinManager.Getcoin());
    }

    //Memunculkan object topping yang bisa di sentuh atau dipilih oleh player
    private void SpawnTopping(Topping[] toppings)
    {
        for(int i = 0; i < toppings.Length && i < toppingSpawns.Length; i++)
        {
            Instantiate(toppings[i].ToppingObjectPrefab, toppingSpawns[i]);
        }
    }

    //Memulai timer
    private IEnumerator Timer(float time)
    {
        float counter = time;

        while (counter >= 0)
        {
            yield return new WaitForSeconds(1f);
            timerText.text = "Timer : " + counter;
            counter--;
        }

        GameFinished();
    }

    //Mengecek kondisi hasil bermain player ketika waktu habis
    void GameFinished()
    {
        Time.timeScale = 0;

        CurrGameState = GameState.Stop;

        PopUp.SetActive(true);

        if (levelManager.IsLevelWin(customerTarget, foodSaleTarget))
        {
            missionText.text = "Mission Completed";

            retryButton.SetActive(false);

            levelManager.CompletedLevel();
        }
        else
        {
            missionText.text = "Mission Failed";
            retryButton.SetActive(true);
        }
    }
    
    //Ketika terdapat makanan yyang berhasil dijual
    void OnSaleCompleted(int price)
    {
        coinManager.AddCoin(price);

        UpdateCoin(coinManager.Getcoin());

        foodSaleTarget++;
    }

    //Ketika terdapat customer yang sudah mendapatkan semua ordernya
    void OnCustomerCompleted()
    {
        customerTarget++;
    }

    //Mengapdate koin yang didapat dari setiap berhasil menjual makanan
    void UpdateCoin(int coin)
    {
        coinText.text = "Coin : " + coin;
    }

    //Mengulangi level dan jumlah customer akan ditambah 1
    public void Retry()
    {
        Time.timeScale = 1;

        levelManager.addCustomer++;
        SceneManager.LoadScene("Main");
    }

    //Kembali ke main menu atau level selector
    public void Back()
    {
        Time.timeScale = 1;

        coinManager.SaveCoin();
        levelManager.addCustomer = 0;
        SceneManager.LoadScene("Menu");
    }
    
}
