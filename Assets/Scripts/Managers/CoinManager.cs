using UnityEngine;

public class CoinManager
{
    private const string COIN_KEY = "Coin";

    private static CoinManager instance;

    private int currCoin;

    public static CoinManager Instance
    {
        get
        {
            if(instance == null)
                instance = new CoinManager();

            return instance;
        }
    }

    //Reset coin sekarang
    public void ResetCoin()
    {
        currCoin = 0;
    }

    //Menambahkan coin dengan coin yang baru didapatkan
    public void AddCoin(int coin)
    {
        currCoin += coin;
    }

    //Mengembalikan coin yang baru saja di dapatkan
    public int Getcoin()
    {
        return currCoin;
    }

    //Menyimpan coin dengan menambahkan coin yang sudah dimiliki dengan coin yang baru saja di dapatkan
    public void SaveCoin()
    {
        int oldCoin = LoadCoin();

        PlayerPrefs.SetInt(COIN_KEY, oldCoin + currCoin);
    }

    //Mengembalikan coin yang disimpan
    public int LoadCoin()
    {
        return PlayerPrefs.GetInt(COIN_KEY, 0);
    }
}
