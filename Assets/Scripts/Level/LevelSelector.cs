using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField]
    GameObject levelPopUp;

    [SerializeField]
    GameObject stageCompletedPopUp;

    [SerializeField]
    TextMeshProUGUI levelName;

    [SerializeField]
    TextMeshProUGUI coinText;

    [SerializeField]
    TextMeshProUGUI objectiveText;

    [SerializeField]
    TextMeshProUGUI failureText;

    Level currLevel;

    #region Subscribe & Unsubscribe input event
    private void OnEnable()
    {
        if(inputManager == null)
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
        coinText.text = "Coin : " + CoinManager.Instance.LoadCoin();
    }

    //Ketika player menyentuh layar
    private void Touch(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);

        if (hit.collider == null)
            return;

        if (hit.collider.tag == "Level")
        {
            int level = 0;
            if (int.TryParse(hit.collider.name, out level))
            {
                ShowLevel(level);
            }
        }

        if (hit.collider.TryGetComponent(out Clickable clickable))
        {
            clickable.PlayAudio();
        }
    }

    //Memuncul level yang dipilih dan level yang sudah terbukan tapi belum selesai
    public void ShowLevel(int level)
    {
        LevelManager manager = LevelManager.Instance;

        currLevel = manager.GetUnlockUnCompletedLevel(level);

        if(currLevel != null)
        {
            PopUp(levelPopUp);

            levelName.text = "Level : " + currLevel.Stage + " - " + currLevel.SubLevel;

            ShowObjective(currLevel);
            ShowFailure(currLevel);
        }
        else
        {
            PopUp(stageCompletedPopUp);
        }
        
    }

    //Memunculkan daftar objective yang harus diselesaikan player pada suatu level
    public void ShowObjective(Level level)
    {
        string objective = "";
        if (level.CustomerTarget != 0)
            objective += " - Target Customer : " + level.CustomerTarget + "\n";
        if (level.FoodSaleTarget != 0)
            objective += " - Food Sale Target : " + level.FoodSaleTarget + "\n";

        objectiveText.text = objective;
    }

    //Memunculkan daftar yang harus dihindari oleh player untuk menyelesaikan suatu level
    public void ShowFailure(Level level)
    {
        string failure = "";
        if (level.CustomerTarget != 0)
            failure += " - Target " + level.CustomerTarget + " customer tidak tercapai \n";
        if (level.FoodSaleTarget != 0)
            failure += " - Target tenjualan " + level.FoodSaleTarget + " tidak tercapai \n";

        failureText.text = failure;
    }

    //Memilih level dan memulai permainan
    public void Play()
    {
        LevelManager manager = LevelManager.Instance;

        manager.LevelSelected(currLevel);

        if(manager.CanLevelPlay())
            SceneManager.LoadScene("Main");
        else
        {
            manager.CompletedLevel();
        }

        levelPopUp.SetActive(false);
    }

    //Memunculkan popup
    public void PopUp(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
}
