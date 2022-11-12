using UnityEngine;

public class LevelManager : SingletonPersistent<LevelManager>
{
    [SerializeField]
    Level[] levels;

    public int currLevelIndex;

    public int addCustomer;

    //Memilih level
    public void LevelSelected(Level levelSelected)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].Stage == levelSelected.Stage && levels[i].SubLevel == levelSelected.SubLevel)
            {
                currLevelIndex = i;
                return;
            }
        }

    }

    //menyelesaikan level
    public void CompletedLevel()
    {
        levels[currLevelIndex].LevelCompleted();

        UnlockLevel(levels[currLevelIndex].Stage, levels[currLevelIndex].SubLevel + 1);

        if (levels[currLevelIndex].SubLevel == 1)
            UnlockLevel(levels[currLevelIndex].Stage + 1, 1);
    }

    //Membuka level
    public void UnlockLevel(int level, int subLevel)
    {
        foreach(Level l in levels)
        {
            if(l.Stage == level && l.SubLevel == subLevel)
            {
                l.LevelUnlocked();
            }
        }
    }

    //Mengembalikan level yang sekarang sedang dimainkan
    public Level GetCurrentLevel()
    {
        return levels[currLevelIndex];
    }

    //Mengembalikan level yang sudah dibuka dan belum di selesaikan
    public Level GetUnlockUnCompletedLevel(int group)
    {
        foreach(Level level in levels)
        {
            if(level.Stage == group)
            {
                if (level.IsUnlock && !level.IsCompleted)
                    return level;
            }
        }

        return null;
    }

    //Cek apakah level selesai
    public bool IsLevelWin(int targetCustomer = 0, int foodSale = 0)
    {
        Level level = levels[currLevelIndex];

        bool isWin = false;

        if (targetCustomer != 0 && level.CustomerTarget != 0)
            isWin = targetCustomer >= level.CustomerTarget;

        if (foodSale != 0 && level.FoodSaleTarget != 0)
            isWin = foodSale >= level.FoodSaleTarget;

        return isWin;
    }

    //Cek apakah level bisa dimainkan
    public bool CanLevelPlay()
    {
        return GetCurrentLevel().Customers != null && GetCurrentLevel().Customers.Length > 0;
    }

    //Customer yang dimunculkan unlimited
    public bool IsUnlimitedCustomer()
    {
        return GetCurrentLevel().MaxCustomer == 0 && GetCurrentLevel().CustomerTarget == 0;
    }
}
